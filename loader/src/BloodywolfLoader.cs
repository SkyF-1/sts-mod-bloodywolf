using System.Reflection;
using System.Runtime.Loader;
using System.Text;
using System.Text.Json;
using System.Text.Json.Nodes;
using Godot;
using HarmonyLib;
using MegaCrit.Sts2.Core.Debug;
using MegaCrit.Sts2.Core.Helpers;
using MegaCrit.Sts2.Core.Logging;
using MegaCrit.Sts2.Core.Modding;
using Logger = MegaCrit.Sts2.Core.Logging.Logger;

namespace StsModBloodywolf.Loader;

[ModInitializer(nameof(Initialize))]
public static class BloodywolfLoader
{
    private const string ModId = "StsModBloodywolf";
    private const string LoaderId = ModId + ".Loader";

    private static readonly Logger Logger = new(LoaderId, LogType.Generic);
    private static readonly MethodInfo? AssociateAssemblyWithModMethod = typeof(ModManager).GetMethod(
        "AssociateAssemblyWithMod",
        BindingFlags.Public | BindingFlags.Static,
        null,
        [typeof(string), typeof(Assembly)],
        null);

    private static readonly Lazy<(bool Patched, Exception? Exception)> ReflectionHelperPatch = new(
        () =>
        {
            Logger.Info($"Falling back to patch {typeof(ReflectionHelper).FullName} for type discovery.");
            try
            {
                var harmony = new Harmony($"{LoaderId}.{nameof(ReflectionHelper)}");
                harmony.Patch(
                    typeof(ReflectionHelper).GetProperty(nameof(ReflectionHelper.ModTypes))!.GetMethod,
                    postfix: new HarmonyMethod(AddArtifactTypesToModTypes));
                return (true, null);
            }
            catch (Exception exception)
            {
                return (false, exception);
            }
        },
        true);

    private static readonly List<Type> ArtifactTypes = [];

    public static void Initialize()
    {
        var loaderAssembly = typeof(BloodywolfLoader).Assembly;
        var modDirectory = Path.GetDirectoryName(loaderAssembly.Location);
        if (string.IsNullOrEmpty(modDirectory))
        {
            throw new ModLoadException("Could not resolve mod directory.");
        }

        var gameVersion = ReleaseInfoManager.Instance.SemVer;
        var artifact = SelectArtifact(modDirectory, gameVersion);
        if (artifact is null)
        {
            throw new ModLoadException(
                $"Game version: {gameVersion?.ToString() ?? "unknown"}, no compatible artifact available.");
        }

        Logger.Info(
            $"Game version: {gameVersion?.ToString() ?? "unknown"}, " +
            $"selected artifact supporting min_game_version={artifact.MinGameVersion}.");

        var pckPath = artifact.FullPckPath(modDirectory);
        if (pckPath is not null)
        {
            if (!File.Exists(pckPath))
            {
                throw new ModLoadException($"Missing artifact PCK: {pckPath}");
            }

            if (!ProjectSettings.LoadResourcePack(pckPath))
            {
                throw new ModLoadException($"Failed to load artifact PCK: {pckPath}");
            }
        }

        var dllPath = artifact.FullDllPath(modDirectory);
        if (dllPath is null)
        {
            return;
        }

        if (!File.Exists(dllPath))
        {
            throw new ModLoadException($"Missing artifact DLL: {dllPath}");
        }

        var loadContext = AssemblyLoadContext.GetLoadContext(loaderAssembly) ?? AssemblyLoadContext.Default;
        var artifactAssembly = loadContext.LoadFromAssemblyPath(dllPath);
        var types = ResolveAssemblyTypes(artifactAssembly);
        ArtifactTypes.AddRange(types);
        TryAssociateAssemblyWithMod(artifactAssembly);
        CallModInitializer(artifactAssembly, types);
    }

    private static ModArtifact? SelectArtifact(string modDirectory, SemanticVersion? gameVersion)
    {
        var artifacts = ResolveArtifacts(modDirectory);
        if (artifacts.Count == 0)
        {
            return null;
        }

        artifacts.Sort(static (left, right) =>
            SemanticVersion.FromString(right.MinGameVersion)
                .CompareTo(SemanticVersion.FromString(left.MinGameVersion)));

        if (gameVersion is null)
        {
            Logger.Info("Game version unknown; using latest artifact.");
            return artifacts[0];
        }

        return artifacts.Find(artifact =>
            SemanticVersion.FromString(artifact.MinGameVersion).CompareTo(gameVersion) <= 0);
    }

    private static List<ModArtifact> ResolveArtifacts(string modDirectory)
    {
        var manifestPath = Path.Combine(modDirectory, $"{ModId}.json");
        if (!File.Exists(manifestPath))
        {
            throw new ModLoadException($"Missing mod manifest: {manifestPath}");
        }

        try
        {
            var manifestText = File.ReadAllText(manifestPath, Encoding.UTF8);
            if (JsonNode.Parse(manifestText) is not JsonObject manifest)
            {
                throw new ModLoadException($"Failed to parse mod manifest as JSON object: {manifestPath}");
            }

            var artifacts = manifest[ModId]?["artifacts"]?.Deserialize<List<ModArtifact>>();
            if (artifacts is null)
            {
                throw new ModLoadException($"Failed to resolve {ModId} artifacts from {manifestPath}");
            }

            return artifacts.Where(artifact => IsValidArtifact(modDirectory, artifact)).ToList();
        }
        catch (Exception exception) when (exception is not ModLoadException)
        {
            throw new ModLoadException($"Failed to resolve artifacts from {manifestPath}: {exception.Message}", exception);
        }
    }

    private static bool IsValidArtifact(string modDirectory, ModArtifact artifact)
    {
        if (!SemanticVersion.TryFromString(artifact.MinGameVersion, out _))
        {
            Logger.Error($"Skipping artifact with invalid min_game_version: {artifact.MinGameVersion}");
            return false;
        }

        var dllPath = artifact.FullDllPath(modDirectory);
        if (dllPath is not null && !IsPathUnderDirectory(dllPath, modDirectory))
        {
            Logger.Error($"Skipping artifact with DLL outside mod directory: {artifact.DllPath}");
            return false;
        }

        var pckPath = artifact.FullPckPath(modDirectory);
        if (pckPath is not null && !IsPathUnderDirectory(pckPath, modDirectory))
        {
            Logger.Error($"Skipping artifact with PCK outside mod directory: {artifact.PckPath}");
            return false;
        }

        return true;
    }

    private static bool IsPathUnderDirectory(string path, string directory)
    {
        path = path.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
               + Path.DirectorySeparatorChar;
        directory = directory.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar)
                    + Path.DirectorySeparatorChar;
        return path.StartsWith(directory, StringComparison.OrdinalIgnoreCase);
    }

    private static Type[] ResolveAssemblyTypes(Assembly assembly)
    {
        try
        {
            return assembly.GetTypes();
        }
        catch (ReflectionTypeLoadException exception)
        {
            Logger.Error($"Failed to load every type from artifact assembly {assembly.Location}: {exception}");
            return exception.Types.OfType<Type>().ToArray();
        }
    }

    private static void TryAssociateAssemblyWithMod(Assembly assembly)
    {
        if (AssociateAssemblyWithModMethod is not null)
        {
            try
            {
                var mod = ModManager.Mods.FirstOrDefault(candidate =>
                    string.Equals(candidate.manifest?.id, ModId, StringComparison.Ordinal));
                if (mod is null)
                {
                    throw new ModLoadException($"Could not find mod {ModId} in {nameof(ModManager)}.");
                }

                AssociateAssemblyWithModMethod.Invoke(null, [ModId, assembly]);
                var assemblies = typeof(Mod)
                    .GetField("assemblies", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                    ?.GetValue(mod) as IList<Assembly>;
                if (assemblies?.Any(candidate => ReferenceEquals(candidate, assembly)) == true)
                {
                    Logger.Info($"Associated artifact assembly {assembly.Location} with {ModId}.");
                    return;
                }

                Logger.Warn($"Could not associate artifact assembly {assembly.Location} with {ModId}.");
            }
            catch (Exception exception)
            {
                Logger.Warn($"Assembly association failed for {assembly.Location}: {exception.Message}");
            }
        }

        var patch = ReflectionHelperPatch.Value;
        if (patch.Patched)
        {
            return;
        }

        if (patch.Exception is not null)
        {
            throw new ModLoadException(
                $"Failed to patch {nameof(ReflectionHelper)}: {patch.Exception.Message}",
                patch.Exception);
        }
    }

    private static void AddArtifactTypesToModTypes(ref Type[] __result)
    {
        if (ArtifactTypes.Count > 0)
        {
            __result = __result.Concat(ArtifactTypes).Distinct().ToArray();
        }
    }

    private static void CallModInitializer(Assembly assembly, Type[] types)
    {
        var initializerTypes = types
            .Where(type => type.GetCustomAttribute<ModInitializerAttribute>() is not null)
            .ToList();
        if (initializerTypes.Count == 0)
        {
            throw new ModLoadException(
                $"No type with {nameof(ModInitializerAttribute)} found in {assembly.Location}.");
        }

        if (!initializerTypes.Any(ModManager.CallModInitializer))
        {
            throw new ModLoadException($"Failed to call an artifact mod initializer in {assembly.Location}.");
        }
    }
}

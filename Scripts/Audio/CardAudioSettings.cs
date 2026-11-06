using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;
using Godot;
using StsModBloodywolf.Scripts.Config;

namespace StsModBloodywolf.Scripts.Audio;

public static class CardAudioSettings
{
    private const int CurrentVersion = 1;
    private const string FileName = "StsModBloodywolf.CardAudioOverrides.json";

    private static readonly Dictionary<string, CardAudioOverride> Overrides =
        new(StringComparer.OrdinalIgnoreCase);
    private static readonly string SettingsPath =
        Path.Combine(OS.GetUserDataDir(), "mod_configs", FileName);
    private static bool _initialized;

    public static event Action? OverridesCleared;

    public static void Initialize()
    {
        if (_initialized)
            return;
        _initialized = true;
        Load();
    }

    public static CardAudioOverride GetOverride(string cardId)
    {
        return Overrides.GetValueOrDefault(NormalizeCardId(cardId), CardAudioOverride.FollowDefault);
    }

    public static void SetOverride(string cardId, CardAudioOverride value)
    {
        var key = NormalizeCardId(cardId);
        if (value == CardAudioOverride.FollowDefault)
            Overrides.Remove(key);
        else
            Overrides[key] = value;
        Save();
    }

    public static void ClearOverrides()
    {
        Overrides.Clear();
        Save();
        OverridesCleared?.Invoke();
    }

    public static CardAudioVariant? ResolveVariant(string cardId)
    {
        return GetOverride(cardId) switch
        {
            CardAudioOverride.Disabled => null,
            CardAudioOverride.CardName => CardAudioVariant.CardName,
            CardAudioOverride.Flavor => CardAudioVariant.Flavor,
            _ => BloodywolfModConfig.DefaultCardAudioVariant
        };
    }

    public static string NormalizeCardId(string cardId)
    {
        return cardId.Trim().ToUpperInvariant();
    }

    private static void Load()
    {
        Overrides.Clear();
        if (!File.Exists(SettingsPath))
            return;

        try
        {
            var data = JsonSerializer.Deserialize<SettingsFile>(File.ReadAllText(SettingsPath));
            if (data?.Overrides == null)
                return;

            foreach (var (cardId, rawValue) in data.Overrides)
            {
                if (Enum.TryParse<CardAudioOverride>(rawValue, ignoreCase: true, out var value)
                    && value != CardAudioOverride.FollowDefault)
                {
                    Overrides[NormalizeCardId(cardId)] = value;
                }
            }
        }
        catch (Exception exception)
        {
            GD.PushWarning($"[BloodywolfAudio] Could not read card audio overrides: {exception.Message}");
        }
    }

    private static void Save()
    {
        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(SettingsPath)!);
            var data = new SettingsFile
            {
                Version = CurrentVersion,
                Overrides = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
            };

            foreach (var (cardId, value) in Overrides)
                data.Overrides[cardId] = value.ToString();

            var json = JsonSerializer.Serialize(data, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            var temporaryPath = SettingsPath + ".tmp";
            File.WriteAllText(temporaryPath, json, new UTF8Encoding(encoderShouldEmitUTF8Identifier: false));
            File.Move(temporaryPath, SettingsPath, overwrite: true);
        }
        catch (Exception exception)
        {
            GD.PushWarning($"[BloodywolfAudio] Could not save card audio overrides: {exception.Message}");
        }
    }

    private sealed class SettingsFile
    {
        public int Version { get; set; } = CurrentVersion;
        public Dictionary<string, string> Overrides { get; set; } = new();
    }
}

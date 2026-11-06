using System;
using System.Collections.Generic;
using System.IO;
using Godot;

namespace StsModBloodywolf.Scripts.Audio;

public static class CardAudioCatalog
{
    private const string AudioRoot = "res://StsModBloodywolf/audio/cards";

    private static readonly Dictionary<CardAudioVariant, Dictionary<string, string>> PathsByVariant = new()
    {
        [CardAudioVariant.CardName] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase),
        [CardAudioVariant.Flavor] = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
    };

    public static int Count { get; private set; }

    public static void Initialize()
    {
        foreach (var paths in PathsByVariant.Values)
            paths.Clear();
        Count = 0;

        IndexVariant(CardAudioVariant.CardName, "name");
        IndexVariant(CardAudioVariant.Flavor, "flavor");

        GD.Print($"[BloodywolfAudio] Indexed {Count} card audio files.");
    }

    public static string? Find(string cardId, CardAudioVariant variant)
    {
        var key = NormalizeCardId(cardId);
        return PathsByVariant[variant].GetValueOrDefault(key);
    }

    public static bool Has(string cardId, CardAudioVariant variant)
    {
        return Find(cardId, variant) != null;
    }

    public static string NormalizeCardId(string cardId)
    {
        return cardId.Trim().ToLowerInvariant();
    }

    private static void IndexVariant(CardAudioVariant variant, string directoryName)
    {
        var directoryPath = $"{AudioRoot}/{directoryName}";
        var directory = DirAccess.Open(directoryPath);
        if (directory == null)
        {
            GD.Print($"[BloodywolfAudio] Card audio directory not found yet: {directoryPath}");
            return;
        }

        directory.ListDirBegin();
        while (true)
        {
            var file = directory.GetNext();
            if (string.IsNullOrEmpty(file))
                break;
            if (file == "." || file == ".." || directory.CurrentIsDir())
                continue;

            if (IsAudioFile(file))
            {
                AddPath(variant, file, $"{directoryPath}/{file}");
            }
            else if (file.EndsWith(".import", StringComparison.OrdinalIgnoreCase))
            {
                // Exported Godot resources can be exposed without their source extension.
                var sourceName = file[..^".import".Length];
                if (IsAudioFile(sourceName))
                    AddPath(variant, sourceName, $"{directoryPath}/{sourceName}");
            }
        }

        directory.ListDirEnd();
    }

    private static void AddPath(CardAudioVariant variant, string fileName, string path)
    {
        var key = NormalizeCardId(Path.GetFileNameWithoutExtension(fileName));
        var paths = PathsByVariant[variant];
        if (paths.ContainsKey(key))
            return;

        paths[key] = path;
        Count++;
    }

    private static bool IsAudioFile(string fileName)
    {
        var extension = Path.GetExtension(fileName).ToLowerInvariant();
        return extension is ".ogg" or ".wav" or ".mp3";
    }
}

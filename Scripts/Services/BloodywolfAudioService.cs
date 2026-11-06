using System.Collections.Generic;
using Godot;
using MegaCrit.Sts2.Core.Models;
using StsModBloodywolf.Scripts.Audio;
using StsModBloodywolf.Scripts.Config;

namespace StsModBloodywolf.Scripts.Services
{
    public static class BloodywolfAudioService
    {
        private const string LegacyAudioRoot = "res://StsModBloodywolf/audio";
        private static readonly Dictionary<string, AudioStream> _streamCache = new();
        private static Node? _audioHost;
        private static long _playerCounter;
        private static bool _initialized;

        public static void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            CardAudioCatalog.Initialize();
        }

        public static void PlayCard(CardModel card, float volume = 1f)
        {
            if (!BloodywolfModConfig.EnableCardVoice) return;

            var variant = CardAudioSettings.ResolveVariant(card.Id.Entry);
            if (variant == null)
                return;

            var path = CardAudioCatalog.Find(card.Id.Entry, variant.Value)
                ?? CardAudioCatalog.Find(card.Id.Entry, GetOtherVariant(variant.Value))
                ?? FindLegacyPath(card.GetType().Name);

            if (path != null)
                TryPlayPath(path, volume * (float)BloodywolfModConfig.MasterVolume);
        }

        public static bool TryPlay(string key, float volume = 1f)
        {
            if (string.IsNullOrEmpty(key)) return false;
            var path = FindLegacyPath(key);
            if (path == null) return false;
            return TryPlayPath(path, volume);
        }

        public static bool HasLegacyAudio(string fileName)
        {
            return FindLegacyPath(fileName) != null;
        }

        private static bool TryPlayPath(string path, float volume)
        {
            var stream = GetOrLoadStream(path);
            if (stream == null) return false;

            var host = EnsureHost();
            if (host == null) return false;

            var player = new AudioStreamPlayer
            {
                Name = $"BWAudio_{++_playerCounter}",
                Stream = stream,
                VolumeDb = LinearToDb(volume)
            };
            host.AddChild(player);
            player.Finished += player.QueueFree;
            player.Play();
            return true;
        }

        private static CardAudioVariant GetOtherVariant(CardAudioVariant variant)
        {
            return variant == CardAudioVariant.CardName
                ? CardAudioVariant.Flavor
                : CardAudioVariant.CardName;
        }

        private static AudioStream? GetOrLoadStream(string path)
        {
            if (_streamCache.TryGetValue(path, out var cached))
                return cached;
            var stream = ResourceLoader.Load<AudioStream>(path);
            if (stream != null)
                _streamCache[path] = stream;
            return stream;
        }

        private static Node? EnsureHost()
        {
            if (_audioHost != null && GodotObject.IsInstanceValid(_audioHost) && _audioHost.IsInsideTree())
                return _audioHost;

            var tree = Engine.GetMainLoop() as SceneTree;
            if (tree?.Root == null) return null;

            _audioHost = tree.Root.GetNodeOrNull<Node>("BloodywolfAudioHost");
            if (_audioHost == null)
            {
                _audioHost = new Node { Name = "BloodywolfAudioHost", ProcessMode = Node.ProcessModeEnum.Always };
                tree.Root.AddChild(_audioHost);
            }
            return _audioHost;
        }

        private static string? FindLegacyPath(string fileName)
        {
            var key = System.IO.Path.GetFileNameWithoutExtension(fileName).ToLowerInvariant();
            var basePath = $"{LegacyAudioRoot}/{key}";
            foreach (var extension in new[] { ".ogg", ".wav", ".mp3", string.Empty })
            {
                var candidate = basePath + extension;
                if (ResourceLoader.Exists(candidate))
                    return candidate;
            }

            return null;
        }

        private static float LinearToDb(float linear)
        {
            return linear <= 0 ? -80f : Mathf.LinearToDb(Mathf.Max(linear, 0.0001f));
        }
    }
}

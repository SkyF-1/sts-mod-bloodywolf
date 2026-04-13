using System.Collections.Generic;
using Godot;
using MegaCrit.Sts2.Core.Runs; // 如果需要判断多人模式
using StsModBloodywolf.Scripts.Config;

namespace StsModBloodywolf.Scripts.Services
{
    public static class BloodywolfAudioService
    {
        private const string AudioRoot = "res://StsModBloodywolf/audio";
        private static readonly Dictionary<string, string> _audioPathByKey = new();
        private static readonly Dictionary<string, AudioStream> _streamCache = new();
        private static Node? _audioHost;
        private static long _playerCounter;
        private static bool _initialized;

        public static void Initialize()
        {
            if (_initialized) return;
            _initialized = true;
            GD.Print("[BloodywolfAudio] Start indexing audio from: " + AudioRoot);
            IndexDirectory(AudioRoot);
            GD.Print($"[BloodywolfAudio] Indexed {_audioPathByKey.Count} keys: {string.Join(", ", _audioPathByKey.Keys)}");
        }

        public static void PlayCard(string cardKey, float volume = 1f)
        {
            if (!BloodywolfModConfig.EnableCardVoice) return;
            TryPlay(cardKey, volume * (float)BloodywolfModConfig.MasterVolume);
        }

        public static bool TryPlay(string key, float volume = 1f)
        {
            if (string.IsNullOrEmpty(key)) return false;
            if (!_audioPathByKey.TryGetValue(NormalizeKey(key), out var path))
            {
                GD.PrintErr($"[BloodywolfAudio] Missing audio key: {key}");
                return false;
            }
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

        private static void IndexDirectory(string dirPath)
        {
            var dir = DirAccess.Open(dirPath);
            if (dir == null) return;
            dir.ListDirBegin();
            while (true)
            {
                var file = dir.GetNext();
                if (string.IsNullOrEmpty(file)) break;
                if (file == "." || file == "..") continue;
                var fullPath = dirPath + "/" + file;
                if (dir.CurrentIsDir())
                    IndexDirectory(fullPath);
                else if (IsAudioFile(file))
                {
                    AddKey(file, fullPath);
                }
                else if (file.EndsWith(".import", StringComparison.OrdinalIgnoreCase))
                {
                    // 处理 .import 文件：提取原始文件名（去掉 .import）
                    var originalName = file.Substring(0, file.Length - ".import".Length);
                    if (IsAudioFile(originalName))
                    {
                        // 路径指向不带扩展名的文件（Godot 资源系统可以加载）
                        var basePath = dirPath + "/" + originalName;
                        AddKey(originalName, basePath);
                    }
                }
            }
            dir.ListDirEnd();
        }

        private static void AddKey(string fileName, string path)
        {
            var key = NormalizeKey(fileName);
            if (!_audioPathByKey.ContainsKey(key))
                _audioPathByKey[key] = path;
        }

        private static bool IsAudioFile(string fileName)
        {
            var ext = System.IO.Path.GetExtension(fileName).ToLowerInvariant();
            return ext == ".mp3" || ext == ".wav" || ext == ".ogg";
        }

        private static string NormalizeKey(string key)
        {
            return System.IO.Path.GetFileNameWithoutExtension(key).ToLowerInvariant();
        }

        private static float LinearToDb(float linear)
        {
            return linear <= 0 ? -80f : Mathf.LinearToDb(Mathf.Max(linear, 0.0001f));
        }
    }
}
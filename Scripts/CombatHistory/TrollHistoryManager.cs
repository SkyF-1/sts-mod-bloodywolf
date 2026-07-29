using System;
using System.Collections.Generic;

namespace StsModBloodywolf.Scripts.CombatHistory
{
    public static class TrollHistoryManager
    {
        private static readonly List<TrollUsedEntry> _entries = new();

        public static IReadOnlyList<TrollUsedEntry> Entries => _entries;
        public static int Count => _entries.Count;

        /// <summary>记录一次 Troll 使用</summary>
        public static void Record(TrollUsedEntry entry)
        {
            _entries.Add(entry);
        }

        /// <summary>获取从指定索引之后的新条目数量</summary>
        public static int GetNewCountSince(int startIndex)
        {
            return Math.Max(0, Count - startIndex);
        }

        /// <summary>获取当前条目总数（用作快照索引）</summary>
        public static int GetSnapshotIndex() => Count;

        /// <summary>战斗开始时清空</summary>
        public static void Clear()
        {
            _entries.Clear();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryMeta
{
    public class DictionaryMetadata<TKey, TValue>
    {
        private readonly int[] buckets;
        private readonly DictionaryEntry<TKey, TValue>[] entries;
        private readonly int count;
        private readonly int freeList;
        private readonly int freeCount;

        public DictionaryMetadata(int[] buckets, DictionaryEntry<TKey, TValue>[] entries, int count, int freeList, int freeCount)
        {
            this.buckets = buckets;
            this.entries = entries;
            this.count = count;
            this.freeList = freeList;
            this.freeCount = freeCount;
        }

        public int[] Buckets { get { return buckets; } }
        public DictionaryEntry<TKey, TValue>[] Entries { get { return entries; } }
        public int Count { get { return count; } }
        public int FreeList { get { return freeList; } }
        public int FreeCount { get { return freeCount; } }
    }
}
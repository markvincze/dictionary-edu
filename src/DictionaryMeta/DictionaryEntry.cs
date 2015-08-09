using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryMeta
{
    public class DictionaryEntry<TKey, TValue>
    {
        private readonly int hashCode;
        private readonly int next;
        private readonly TKey key;
        private readonly TValue value;

        public DictionaryEntry(int hashCode, int next, TKey key, TValue value)
        {
            this.hashCode = hashCode;
            this.next = next;
            this.key = key;
            this.value = value;
        }

        public int HashCode
        {
            get { return hashCode; }
        }

        public int Next
        {
            get { return next; }
        }

        public TKey Key
        {
            get { return key; }
        }

        public TValue Value
        {
            get { return value; }
        }
    }
}
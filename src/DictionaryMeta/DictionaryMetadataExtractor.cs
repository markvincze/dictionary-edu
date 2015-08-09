using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace DictionaryMeta
{
    public class DictionaryMetadataExtractor<TKey, TValue>
    {
        public DictionaryMetadata<TKey, TValue> ExtractMetadata(Dictionary<TKey, TValue> dictionary)
        {
            var type = typeof (Dictionary<TKey, TValue>).GetNestedType("Entry", BindingFlags.NonPublic);
            type = type.MakeGenericType(typeof (TKey), typeof (TValue));

            var entries = GetFieldValue<IEnumerable>(dictionary, "entries");

            var convertedEntries = new List<DictionaryEntry<TKey, TValue>>();
            foreach (var entry in entries)
            {
                convertedEntries.Add(new DictionaryEntry<TKey, TValue>(
                        GetFieldValue<int>(entry, type, "hashCode"),
                        GetFieldValue<int>(entry, type, "next"),
                        GetFieldValue<TKey>(entry, type, "key"),
                        GetFieldValue<TValue>(entry, type, "value")));
            }

            return new DictionaryMetadata<TKey, TValue>(
                GetFieldValue<int[]>(dictionary, "buckets"),
                convertedEntries.ToArray(),
                GetFieldValue<int>(dictionary, "count"),
                GetFieldValue<int>(dictionary, "freeList"),
                GetFieldValue<int>(dictionary, "freeCount"));
        }

        private TField GetFieldValue<TField>(Dictionary<TKey, TValue> obj, string fieldName)
        {
            return this.GetFieldValue<Dictionary<TKey, TValue>, TField>(obj, fieldName);
        }

        private TField GetFieldValue<TObj, TField>(TObj obj, string fieldName)
        {
            var type = typeof (TObj);

            return GetFieldValue<TField>(obj, type, fieldName);
        }

        private TField GetFieldValue<TField>(object obj, Type type, string fieldName)
        {
            var fi = type.GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            return (TField)fi.GetValue(obj);
        }
    }
}
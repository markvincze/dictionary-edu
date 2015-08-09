using System;
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

            var entries = GetFieldValue<object[]>(dictionary, "entries");

            var convertedEntries = entries
                .Select(e =>
                    new DictionaryEntry<TKey, TValue>(
                        GetFieldValue<int>(e, type, "hashCode"),
                        GetFieldValue<int>(e, type, "next"),
                        GetFieldValue<TKey>(e, type, "key"),
                        GetFieldValue<TValue>(e, type, "value")))
                .ToArray();

            return new DictionaryMetadata<TKey, TValue>(
                GetFieldValue<int[]>(dictionary, "buckets"),
                convertedEntries,
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
            var fi = type.GetField(fieldName, BindingFlags.Instance | BindingFlags.NonPublic);

            return (TField)fi.GetValue(obj);
        }
    }
}
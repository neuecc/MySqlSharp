using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlSharp.Internal
{
    public class ParameterDictionary
    {
        struct Entry
        {
            public uint HashCode;
            public int Next;
            public string Key;
            public object Value;
        }

        int[] buckets;
        Entry[] entries;
        uint indexFor;
        int count;

        readonly float loadFactor;

        public ParameterDictionary()
            : this(8, 0.72f)
        {

        }

        ParameterDictionary(int capacity, float loadFactor)
        {
            var tableSize = CalculateCapacity(capacity, loadFactor);
            this.loadFactor = loadFactor;
            this.buckets = new int[tableSize];
            this.entries = new Entry[tableSize];
            this.indexFor = (uint)buckets.Length - 1;

            for (int i = 0; i < this.buckets.Length; i++)
            {
                this.buckets[i] = -1;
                this.entries[i].Next = -1;
            }
        }

        public void Add(string key, object value)
        {
            if (!TryAddInternal(key, value))
            {
                throw new ArgumentException("Key was already exists. Key:" + key);
            }
        }

        bool TryAddInternal(string key, object value)
        {
            var table = buckets;
            var hash = GetFNVHashCode(key);

            // duplicate check
            for (int i = table[hash & indexFor]; i >= 0; i = entries[i].Next)
            {
                if (entries[i].HashCode == hash)
                {
                    ref var v = ref entries[i];
                    if (v.Key == key) return false; // duplicate
                }
            }

            // rehash
            var newLength = CalculateCapacity(count, loadFactor);
            if (buckets.Length < newLength)
            {
                var newBuckets = new int[newLength];
                var newEntries = new Entry[newLength];
                for (int i = 0; i < newBuckets.Length; i++)
                {
                    newBuckets[i] = -1;
                    newEntries[i].Next = -1;
                }

                this.count = 0;
                for (int i = 0; i < entries.Length; i++)
                {
                    if (!object.ReferenceEquals(entries[i].Key, null))
                    {
                        var lastIndex = -1;
                        for (int j = newBuckets[newEntries[i].HashCode & indexFor]; j >= 0; j = newEntries[j].Next)
                        {
                            lastIndex = j;
                        }

                        newEntries[count] = entries[i];
                        if (lastIndex != -1 && !object.ReferenceEquals(entries[lastIndex].Key, null))
                        {
                            newEntries[lastIndex].Next = count;
                        }
                        else
                        {
                            newBuckets[entries[i].HashCode & indexFor] = count;
                        }

                        this.count++;
                    }
                }

                this.buckets = newBuckets;
                this.entries = newEntries;
                this.indexFor = (uint)(newLength - 1);
            }

            // add new
            {
                var lastIndex = -1;
                for (int i = buckets[hash & indexFor]; i >= 0; i = entries[i].Next)
                {
                    lastIndex = i;
                }

                var entry = new Entry { Key = key, Value = value, HashCode = hash };
                this.entries[count] = entry;
                if (lastIndex != -1 && !object.ReferenceEquals(entries[lastIndex].Key, null))
                {
                    entries[lastIndex].Next = count;
                }
                else
                {
                    this.buckets[hash & indexFor] = count;
                }
            }

            count++;
            return true;
        }

        public object GetValue(char[] key, int offset, int length)
        {
            var table = buckets;
            var hash = GetFNVHashCode(key, offset, length);

            for (int i = table[hash & indexFor]; i >= 0; i = entries[i].Next)
            {
                if (entries[i].HashCode == hash)
                {
                    ref var v = ref entries[i];

                    if (v.Key.Length != length) goto NEXT;

                    var index = 0;
                    for (int j = offset; j < length; j++)
                    {
                        if (v.Key[index++] != key[j]) goto NEXT;
                    }
                    return v.Value;
                }

                NEXT:
                continue;
            }

            return null;
        }

        static uint GetFNVHashCode(char[] text, int offset, int length)
        {
            uint hashCode = 2166136261;
            var end = offset + length;

            for (int i = offset; i < end; i++)
            {
                hashCode = unchecked((hashCode ^ text[i]) * 16777619);
            }

            return hashCode;
        }

        static uint GetFNVHashCode(string text)
        {
            uint hashCode = 2166136261;
            var end = text.Length;

            for (int i = 0; i < end; i++)
            {
                hashCode = unchecked((hashCode ^ text[i]) * 16777619);
            }

            return hashCode;
        }

        static int CalculateCapacity(int collectionSize, float loadFactor)
        {
            var initialCapacity = (int)(((float)collectionSize) / loadFactor);
            var capacity = 1;
            while (capacity < initialCapacity)
            {
                capacity <<= 1;
            }

            if (capacity < 8)
            {
                return 8;
            }

            return capacity;
        }
    }
}
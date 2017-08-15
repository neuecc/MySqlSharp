using MySqlSharp.Internal;
using System.Linq;
using MySqlSharp.Protocol;
using System;
using System.Buffers;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;

namespace MySqlSharp.Mapper
{
    public static class ObjectMapper
    {
        public static T[] QueryOne<T>(this MySqlDriver driver, FormattableString query)
        {
            driver.Open();
            var reader = driver.Query(query);
            // reader.ColumnDefinitions
            // reader.ColumnDefinitions

            // var pool = System.Buffers.ArrayPool<T>.Shared.Rent(1024);
            // reader.ColumnDefinitions
            // System.Buffers.ArrayPool<T>.Shared.Return(pool, true);
            // create final buffer.
            // (query.Format),
            // return Cache<T>.convert(reader);

            // Array.Clear(0, 

            // reader.ColumnDefinitions

            return null;
        }

        public static IEnumerable<T> QueryEnumerable<T>(this MySqlDriver driver, FormattableString query)
        {
            driver.Open();
            var reader = driver.Query(query);
            var converter = ConverterCache<T>.GetConverter(reader);

            while (reader.Read())
            {
                var v = converter(reader);
                yield return v;
            }
        }

        public static T[] Query<T>(this MySqlDriver driver, FormattableString query)
        {
            driver.Open();
            var reader = driver.Query(query);

            var converter = ConverterCache<T>.GetConverter(reader);
            var array = ThreadStaticArrayPool<T>.GetArray();

            var count = 0;
            while (reader.Read())
            {
                if (count == array.Length)
                {
                    Array.Resize(ref array, checked((int)(count * 1.5)));
                }

                var v = converter(reader);
                array[count++] = v;
            }

            if (array == ThreadStaticArrayPool<T>.GetArray())
            {
                var result = new T[count];
                Array.Copy(array, result, count);
                Array.Clear(array, 0, count); // null clear
                return result;
            }
            else
            {
                Array.Resize(ref array, count);
                return array;
            }
        }

        static class ConverterCache<T>
        {
            static readonly AsymmetricKeyHashTable<byte[][], ColumnDefinition41[], Lazy<Func<TextResultSet, T>>> cache;

            public static Func<TextResultSet, T> GetConverter(TextResultSet reader)
            {
                if (reader.ColumnCount == 1)
                {
                    // single...
                    throw new NotImplementedException();
                }
                else
                {
                    if (cache.TryGetValue(reader.ColumnDefinitions, out var convert))
                    {
                        return convert.Value;
                    }
                    else
                    {
                        var key = reader.ColumnDefinitions.Select(x => x.column.ToArray()).ToArray();
                        var value = cache.AddOrGet(key, x => new Lazy<Func<TextResultSet, T>>(() => (Func<TextResultSet, T>)ConverterCacheHelper.GetFunc(typeof(T))));

                        return value.Value;
                    }
                }
            }
        }

        static class ConverterCacheHelper
        {
            // Func<TextResultSet, T>
            public static object GetFunc(Type type)
            {
                if (type == typeof(int))
                {
                    return new Func<TextResultSet, int>(x => x.GetInt32(0));
                }

                // TODO:when object...

                throw new NotImplementedException();
            }
        }

        static class ThreadStaticArrayPool<T>
        {
            [ThreadStatic]
            static T[] array;

            public static T[] GetArray()
            {
                if (array == null)
                {
                    array = new T[1024];
                }
                return array;
            }
        }
    }

    internal sealed class MapperKeyEqualityComparer : IAsymmetricEqualityComparer<byte[][], ColumnDefinition41[]>
    {
        static readonly bool Is32Bit = (IntPtr.Size == 4);

        public bool Equals(byte[][] x, byte[][] y)
        {
            if (x.Length != y.Length) return false;

            for (int i = 0; i < x.Length; i++)
            {
                if (x[i].Length != y[i].Length) return false;

                for (int j = 0; j < x[i].Length; j++)
                {
                    if (x[i][j] != y[i][j]) return false;
                }
            }

            return true;
        }

        public bool Equals(byte[][] x, ColumnDefinition41[] y)
        {
            if (x.Length != y.Length) return false;

            for (int i = 0; i < x.Length; i++)
            {
                ref var yColumn = ref y[i].column;

                if (x[i].Length != yColumn.Count) return false;

                var yOffset = yColumn.Offset;
                for (int j = 0; j < x[i].Length; j++)
                {
                    if (x[i][j] != yColumn.Array[yOffset++]) return false;
                }
            }

            return true;
        }

        public int GetHashCode(byte[][] key1)
        {
            if (Is32Bit)
            {
                uint hash = 1;
                for (int i = 0; i < key1.Length; i++)
                {
                    hash = unchecked((31 * hash) + FarmHash.Hash32(key1[i], 0, key1[i].Length));
                }

                return unchecked((int)hash);
            }
            else
            {
                ulong hash = 1;
                for (int i = 0; i < key1.Length; i++)
                {
                    hash = unchecked((31 * hash) + FarmHash.Hash64(key1[i], 0, key1[i].Length));
                }

                return unchecked((int)hash);
            }
        }

        public int GetHashCode(ColumnDefinition41[] key2)
        {
            if (Is32Bit)
            {
                uint hash = 1;
                for (int i = 0; i < key2.Length; i++)
                {
                    ref var column = ref key2[i].column;
                    hash = unchecked((31 * hash) + FarmHash.Hash32(column.Array, column.Offset, column.Count));
                }

                return unchecked((int)hash);
            }
            else
            {
                ulong hash = 1;
                for (int i = 0; i < key2.Length; i++)
                {
                    ref var column = ref key2[i].column;
                    hash = unchecked((31 * hash) + FarmHash.Hash64(column.Array, column.Offset, column.Count));
                }

                return unchecked((int)hash);
            }
        }
    }
}
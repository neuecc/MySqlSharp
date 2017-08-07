using MySqlSharp.Internal;
using System.Reflection;
using MySqlSharp.Protocol;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlSharp
{
    public struct MySqlParameter
    {

        // TODO:parameters...
        // byte[] bytes;



        public static implicit operator MySqlParameter(int value)
        {
            // BinaryUtil.WriteInt32(

            return default(MySqlParameter);
        }

        public static implicit operator MySqlParameter(string value)
        {


            return default(MySqlParameter);
        }
    }

    internal struct BitArraySlim
    {
        int[] array;
        int length;

        public BitArraySlim(int length)
        {
            this.array = new int[GetArrayLength(length, 32)];
            this.length = length;
        }

        public bool this[int index]
        {
            get
            {
                return this.Get(index);
            }
            set
            {
                this.Set(index, value);
            }
        }

        bool Get(int index)
        {
            return (this.array[index / 32] & 1 << index % 32) > 0;
        }

        void Set(int index, bool value)
        {
            if (value)
            {
                this.array[index / 32] |= 1 << index % 32;
            }
            else
            {
                this.array[index / 32] &= ~(1 << index % 32);
            }
        }

        public void WriteTo(ref PacketWriter writer)
        {
            int arrayLength = GetArrayLength(this.length, 8);

            writer.EnsureCapacity(arrayLength);
            for (int i = 0; i < arrayLength; i++)
            {
                writer.WriteByteUnsafe((byte)(this.array[i / 4] >> i % 4 * 8 & 255));
            }
        }

        static int GetArrayLength(int n, int div)
        {
            if (n <= 0)
            {
                return 0;
            }
            return (n - 1) / div + 1;
        }
    }


    public static class MySqlParameterWriter
    {
        public static void Write(ref PacketWriter writer, object value)
        {
            if (value == null)
            {
                // write null
            }

            switch (value)
            {
                case Int16 x:
                    break;
                case Int32 x:
                    writer.WriteInt32(x);
                    break;
                case Int64 x:
                    break;
                case UInt16 x:
                    break;
                case UInt32 x:
                    break;
                case UInt64 x:
                    break;
                case Double x:
                    break;
                case Single x:
                    break;
                // TODO:...
                default:
                    break;
            }

        }
    }
}

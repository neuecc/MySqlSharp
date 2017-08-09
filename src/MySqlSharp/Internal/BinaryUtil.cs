using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;

namespace MySqlSharp.Internal
{
    internal static class StringEncoding
    {
        public static Encoding UTF8 = new UTF8Encoding(false);
    }

    internal static class BinaryUtil
    {
        static BinaryUtil()
        {
            if (!BitConverter.IsLittleEndian)
            {
                throw new Exception("Currently only supports Little-Endian environments.");
            }
        }

        public static void EnsureCapacity(ref byte[] bytes, int offset, int appendLength)
        {
            var newLength = offset + appendLength;

            // If null(most case fisrt time) fill byte.
            if (bytes == null)
            {
                bytes = new byte[newLength];
                return;
            }

            // like MemoryStream.EnsureCapacity
            var current = bytes.Length;
            if (newLength > current)
            {
                int num = newLength;
                if (num < 256)
                {
                    num = 256;
                    FastResize(ref bytes, num);
                    return;
                }
                if (num < current * 2)
                {
                    num = current * 2;
                }

                FastResize(ref bytes, num);
            }
        }

        // Buffer.BlockCopy version of Array.Resize
        public static void FastResize(ref byte[] array, int newSize)
        {
            if (newSize < 0) throw new ArgumentOutOfRangeException("newSize");

            byte[] array2 = array;
            if (array2 == null)
            {
                array = new byte[newSize];
                return;
            }

            if (array2.Length != newSize)
            {
                byte[] array3 = new byte[newSize];
                Buffer.BlockCopy(array2, 0, array3, 0, (array2.Length > newSize) ? newSize : array2.Length);
                array = array3;
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBoolean(ref byte[] bytes, int offset, bool value)
        {
            EnsureCapacity(ref bytes, offset, 1);

            bytes[offset] = (byte)(value ? 1 : 0);
            return 1;
        }

        /// <summary>
        /// Unsafe! don't ensure capacity and don't return size.
        /// </summary>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBooleanUnsafe(ref byte[] bytes, int offset, bool value)
        {
            bytes[offset] = (byte)(value ? 1 : 0);
        }

        /// <summary>
        /// Unsafe! don't ensure capacity and don't return size.
        /// </summary>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBooleanTrueUnsafe(ref byte[] bytes, int offset)
        {
            bytes[offset] = (byte)(1);
        }

        /// <summary>
        /// Unsafe! don't ensure capacity and don't return size.
        /// </summary>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void WriteBooleanFalseUnsafe(ref byte[] bytes, int offset)
        {
            bytes[offset] = (byte)(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool ReadBoolean(byte[] bytes, int offset)
        {
            return (bytes[offset] == 0) ? false : true;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteByte(ref byte[] bytes, int offset, byte value)
        {
            EnsureCapacity(ref bytes, offset, 1);

            bytes[offset] = value;
            return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref byte ReadByte(byte[] bytes, int offset)
        {
            return ref bytes[offset];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteBytes(ref byte[] bytes, int offset, byte[] value)
        {
            EnsureCapacity(ref bytes, offset, value.Length);
            Buffer.BlockCopy(value, 0, bytes, offset, value.Length);
            return value.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static byte[] ReadBytes(byte[] bytes, int offset, int count)
        {
            var dest = new byte[count];
            Buffer.BlockCopy(bytes, offset, dest, 0, count);
            return dest;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteSByte(ref byte[] bytes, int offset, sbyte value)
        {
            EnsureCapacity(ref bytes, offset, 1);

            bytes[offset] = (byte)value;
            return 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static sbyte ReadSByte(byte[] bytes, int offset)
        {
            return (sbyte)bytes[offset];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteSingle(ref byte[] bytes, int offset, float value)
        {
            EnsureCapacity(ref bytes, offset, 4);

            fixed (byte* ptr = bytes)
            {
                *(float*)(ptr + offset) = value;
            }

            return 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe float ReadSingle(byte[] bytes, int offset)
        {
            fixed (byte* ptr = bytes)
            {
                return *(float*)(ptr + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteDouble(ref byte[] bytes, int offset, double value)
        {
            EnsureCapacity(ref bytes, offset, 8);

            fixed (byte* ptr = bytes)
            {
                *(double*)(ptr + offset) = value;
            }

            return 8;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe double ReadDouble(byte[] bytes, int offset)
        {
            fixed (byte* ptr = bytes)
            {
                return *(double*)(ptr + offset);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteInt16(ref byte[] bytes, int offset, short value)
        {
            EnsureCapacity(ref bytes, offset, 2);

            fixed (byte* ptr = bytes)
            {
                *(short*)(ptr + offset) = value;
            }

            return 2;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe short ReadInt16(byte[] bytes, int offset)
        {
            fixed (byte* ptr = bytes)
            {
                return *(short*)(ptr + offset);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteInt32(ref byte[] bytes, int offset, int value)
        {
            EnsureCapacity(ref bytes, offset, 4);

            fixed (byte* ptr = bytes)
            {
                *(int*)(ptr + offset) = value;
            }

            return 4;
        }

        /// <summary>
        /// Unsafe! don't ensure capacity and don't return size.
        /// </summary>

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe void WriteInt32Unsafe(ref byte[] bytes, int offset, int value)
        {
            fixed (byte* ptr = bytes)
            {
                *(int*)(ptr + offset) = value;
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int ReadInt32(byte[] bytes, int offset)
        {
            fixed (byte* ptr = bytes)
            {
                return *(int*)(ptr + offset);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteInt64(ref byte[] bytes, int offset, long value)
        {
            EnsureCapacity(ref bytes, offset, 8);

            fixed (byte* ptr = bytes)
            {
                *(long*)(ptr + offset) = value;
            }

            return 8;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe long ReadInt64(byte[] bytes, int offset)
        {
            fixed (byte* ptr = bytes)
            {
                return *(long*)(ptr + offset);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteUInt16(ref byte[] bytes, int offset, ushort value)
        {
            EnsureCapacity(ref bytes, offset, 2);

            fixed (byte* ptr = bytes)
            {
                *(ushort*)(ptr + offset) = value;
            }

            return 2;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ushort ReadUInt16(byte[] bytes, int offset)
        {
            fixed (byte* ptr = bytes)
            {
                return *(ushort*)(ptr + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteUInt32(ref byte[] bytes, int offset, uint value)
        {
            EnsureCapacity(ref bytes, offset, 4);

            fixed (byte* ptr = bytes)
            {
                *(uint*)(ptr + offset) = value;
            }

            return 4;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe uint ReadUInt32(byte[] bytes, int offset)
        {
            fixed (byte* ptr = bytes)
            {
                return *(uint*)(ptr + offset);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int WriteUInt64(ref byte[] bytes, int offset, ulong value)
        {
            EnsureCapacity(ref bytes, offset, 8);

            fixed (byte* ptr = bytes)
            {
                *(ulong*)(ptr + offset) = value;
            }

            return 8;
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe ulong ReadUInt64(byte[] bytes, int offset)
        {
            fixed (byte* ptr = bytes)
            {
                return *(ulong*)(ptr + offset);
            }
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteChar(ref byte[] bytes, int offset, char value)
        {
            return WriteUInt16(ref bytes, offset, (ushort)value);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static char ReadChar(byte[] bytes, int offset)
        {
            return (char)ReadUInt16(bytes, offset);
        }


        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteString(ref byte[] bytes, int offset, string value)
        {
            var ensureSize = StringEncoding.UTF8.GetMaxByteCount(value.Length);
            EnsureCapacity(ref bytes, offset, ensureSize);

            return StringEncoding.UTF8.GetBytes(value, 0, value.Length, bytes, offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteString(ref byte[] bytes, int offset, char[] value, int charCount)
        {
            var ensureSize = StringEncoding.UTF8.GetMaxByteCount(charCount);
            EnsureCapacity(ref bytes, offset, ensureSize);

            return StringEncoding.UTF8.GetBytes(value, 0, charCount, bytes, offset);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadString(byte[] bytes, int offset, int count)
        {
            return StringEncoding.UTF8.GetString(bytes, offset, count);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int WriteNullTerminatedString(ref byte[] bytes, int offset, string value)
        {
            var ensureSize = StringEncoding.UTF8.GetMaxByteCount(value.Length) + 1; // + null
            EnsureCapacity(ref bytes, offset, ensureSize);

            var length = StringEncoding.UTF8.GetBytes(value, 0, value.Length, bytes, offset);
            bytes[offset + length] = 0; // null terminate
            return length + 1;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string ReadNullTerminatedString(byte[] bytes, int offset, out int readCount)
        {
            var startOffset = offset;

            while (bytes[offset] != 0)
            {
                offset++;
            }

            var len = offset - startOffset;
            readCount = len + 1;
            return StringEncoding.UTF8.GetString(bytes, startOffset, len);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Read3BytesInt32(byte[] bytes, int offset)
        {
            checked
            {
                return (int)bytes[offset] + (int)(bytes[offset + 1] << 8) + (int)(bytes[offset + 2] << 16);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static unsafe int Write3BytesInt32(ref byte[] bytes, int offset, int value)
        {
            EnsureCapacity(ref bytes, offset, 3);

            bytes[offset] = unchecked((byte)value);
            bytes[offset + 1] = unchecked((byte)(value >> 8));
            bytes[offset + 2] = unchecked((byte)(value >> 16));
            return 3;
        }
    }
}

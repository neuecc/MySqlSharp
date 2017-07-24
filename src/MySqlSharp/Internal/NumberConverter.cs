using System;

namespace MySqlSharp.Internal
{
    /// <summary>
    /// UTF8(Ascii) ByteArray to Number Converter to avoid utf8 convert and number parsing.
    /// </summary>
    internal static partial class NumberConverter
    {
        // loop unrolling(we know number length)

        const byte Minus = 45;
        const byte Dot = 46;
        const byte Zero = 48;

        public static SByte ToSByte(byte[] bytes, int offset, int count)
        {
            // Min: -128
            // Max: 127
            // Digits: 3
 
            if (bytes[offset] != Minus)
            {
                switch (count)
                {
                    case 1:
                        return (System.SByte)(((SByte)(bytes[offset] - Zero)));
                    case 2:
                        return (System.SByte)(((SByte)(bytes[offset] - Zero) * 10) + ((SByte)(bytes[offset + 1] - Zero)));
                    case 3:
                        return (System.SByte)(((SByte)(bytes[offset] - Zero) * 100) + ((SByte)(bytes[offset + 1] - Zero) * 10) + ((SByte)(bytes[offset + 2] - Zero)));
                    default:
                        throw new ArgumentException("SByte out of range count");
                }
            }
 
            else
            {
                switch (count)
                {
                    case 2:
                        return (System.SByte)((((SByte)(bytes[offset + 1] - Zero))) * -1);
                    case 3:
                        return (System.SByte)((((SByte)(bytes[offset + 1] - Zero) * 10) + ((SByte)(bytes[offset + 2] - Zero))) * -1);
                    case 4:
                        return (System.SByte)((((SByte)(bytes[offset + 1] - Zero) * 100) + ((SByte)(bytes[offset + 2] - Zero) * 10) + ((SByte)(bytes[offset + 3] - Zero))) * -1);
                    default:
                        throw new ArgumentException("SByte out of range count");
                }
            }
        }
        public static Byte ToByte(byte[] bytes, int offset, int count)
        {
            // Min: 0
            // Max: 255
            // Digits: 3
            {
                switch (count)
                {
                    case 1:
                        return (System.Byte)(((Byte)(bytes[offset] - Zero)));
                    case 2:
                        return (System.Byte)(((Byte)(bytes[offset] - Zero) * 10) + ((Byte)(bytes[offset + 1] - Zero)));
                    case 3:
                        return (System.Byte)(((Byte)(bytes[offset] - Zero) * 100) + ((Byte)(bytes[offset + 1] - Zero) * 10) + ((Byte)(bytes[offset + 2] - Zero)));
                    default:
                        throw new ArgumentException("Byte out of range count");
                }
            }
        }
        public static Int16 ToInt16(byte[] bytes, int offset, int count)
        {
            // Min: -32768
            // Max: 32767
            // Digits: 5
 
            if (bytes[offset] != Minus)
            {
                switch (count)
                {
                    case 1:
                        return (System.Int16)(((Int16)(bytes[offset] - Zero)));
                    case 2:
                        return (System.Int16)(((Int16)(bytes[offset] - Zero) * 10) + ((Int16)(bytes[offset + 1] - Zero)));
                    case 3:
                        return (System.Int16)(((Int16)(bytes[offset] - Zero) * 100) + ((Int16)(bytes[offset + 1] - Zero) * 10) + ((Int16)(bytes[offset + 2] - Zero)));
                    case 4:
                        return (System.Int16)(((Int16)(bytes[offset] - Zero) * 1000) + ((Int16)(bytes[offset + 1] - Zero) * 100) + ((Int16)(bytes[offset + 2] - Zero) * 10) + ((Int16)(bytes[offset + 3] - Zero)));
                    case 5:
                        return (System.Int16)(((Int16)(bytes[offset] - Zero) * 10000) + ((Int16)(bytes[offset + 1] - Zero) * 1000) + ((Int16)(bytes[offset + 2] - Zero) * 100) + ((Int16)(bytes[offset + 3] - Zero) * 10) + ((Int16)(bytes[offset + 4] - Zero)));
                    default:
                        throw new ArgumentException("Int16 out of range count");
                }
            }
 
            else
            {
                switch (count)
                {
                    case 2:
                        return (System.Int16)((((Int16)(bytes[offset + 1] - Zero))) * -1);
                    case 3:
                        return (System.Int16)((((Int16)(bytes[offset + 1] - Zero) * 10) + ((Int16)(bytes[offset + 2] - Zero))) * -1);
                    case 4:
                        return (System.Int16)((((Int16)(bytes[offset + 1] - Zero) * 100) + ((Int16)(bytes[offset + 2] - Zero) * 10) + ((Int16)(bytes[offset + 3] - Zero))) * -1);
                    case 5:
                        return (System.Int16)((((Int16)(bytes[offset + 1] - Zero) * 1000) + ((Int16)(bytes[offset + 2] - Zero) * 100) + ((Int16)(bytes[offset + 3] - Zero) * 10) + ((Int16)(bytes[offset + 4] - Zero))) * -1);
                    case 6:
                        return (System.Int16)((((Int16)(bytes[offset + 1] - Zero) * 10000) + ((Int16)(bytes[offset + 2] - Zero) * 1000) + ((Int16)(bytes[offset + 3] - Zero) * 100) + ((Int16)(bytes[offset + 4] - Zero) * 10) + ((Int16)(bytes[offset + 5] - Zero))) * -1);
                    default:
                        throw new ArgumentException("Int16 out of range count");
                }
            }
        }
        public static UInt16 ToUInt16(byte[] bytes, int offset, int count)
        {
            // Min: 0
            // Max: 65535
            // Digits: 5
            {
                switch (count)
                {
                    case 1:
                        return (System.UInt16)(((UInt16)(bytes[offset] - Zero)));
                    case 2:
                        return (System.UInt16)(((UInt16)(bytes[offset] - Zero) * 10) + ((UInt16)(bytes[offset + 1] - Zero)));
                    case 3:
                        return (System.UInt16)(((UInt16)(bytes[offset] - Zero) * 100) + ((UInt16)(bytes[offset + 1] - Zero) * 10) + ((UInt16)(bytes[offset + 2] - Zero)));
                    case 4:
                        return (System.UInt16)(((UInt16)(bytes[offset] - Zero) * 1000) + ((UInt16)(bytes[offset + 1] - Zero) * 100) + ((UInt16)(bytes[offset + 2] - Zero) * 10) + ((UInt16)(bytes[offset + 3] - Zero)));
                    case 5:
                        return (System.UInt16)(((UInt16)(bytes[offset] - Zero) * 10000) + ((UInt16)(bytes[offset + 1] - Zero) * 1000) + ((UInt16)(bytes[offset + 2] - Zero) * 100) + ((UInt16)(bytes[offset + 3] - Zero) * 10) + ((UInt16)(bytes[offset + 4] - Zero)));
                    default:
                        throw new ArgumentException("UInt16 out of range count");
                }
            }
        }
        public static Int32 ToInt32(byte[] bytes, int offset, int count)
        {
            // Min: -2147483648
            // Max: 2147483647
            // Digits: 10
 
            if (bytes[offset] != Minus)
            {
                switch (count)
                {
                    case 1:
                        return (System.Int32)(((Int32)(bytes[offset] - Zero)));
                    case 2:
                        return (System.Int32)(((Int32)(bytes[offset] - Zero) * 10) + ((Int32)(bytes[offset + 1] - Zero)));
                    case 3:
                        return (System.Int32)(((Int32)(bytes[offset] - Zero) * 100) + ((Int32)(bytes[offset + 1] - Zero) * 10) + ((Int32)(bytes[offset + 2] - Zero)));
                    case 4:
                        return (System.Int32)(((Int32)(bytes[offset] - Zero) * 1000) + ((Int32)(bytes[offset + 1] - Zero) * 100) + ((Int32)(bytes[offset + 2] - Zero) * 10) + ((Int32)(bytes[offset + 3] - Zero)));
                    case 5:
                        return (System.Int32)(((Int32)(bytes[offset] - Zero) * 10000) + ((Int32)(bytes[offset + 1] - Zero) * 1000) + ((Int32)(bytes[offset + 2] - Zero) * 100) + ((Int32)(bytes[offset + 3] - Zero) * 10) + ((Int32)(bytes[offset + 4] - Zero)));
                    case 6:
                        return (System.Int32)(((Int32)(bytes[offset] - Zero) * 100000) + ((Int32)(bytes[offset + 1] - Zero) * 10000) + ((Int32)(bytes[offset + 2] - Zero) * 1000) + ((Int32)(bytes[offset + 3] - Zero) * 100) + ((Int32)(bytes[offset + 4] - Zero) * 10) + ((Int32)(bytes[offset + 5] - Zero)));
                    case 7:
                        return (System.Int32)(((Int32)(bytes[offset] - Zero) * 1000000) + ((Int32)(bytes[offset + 1] - Zero) * 100000) + ((Int32)(bytes[offset + 2] - Zero) * 10000) + ((Int32)(bytes[offset + 3] - Zero) * 1000) + ((Int32)(bytes[offset + 4] - Zero) * 100) + ((Int32)(bytes[offset + 5] - Zero) * 10) + ((Int32)(bytes[offset + 6] - Zero)));
                    case 8:
                        return (System.Int32)(((Int32)(bytes[offset] - Zero) * 10000000) + ((Int32)(bytes[offset + 1] - Zero) * 1000000) + ((Int32)(bytes[offset + 2] - Zero) * 100000) + ((Int32)(bytes[offset + 3] - Zero) * 10000) + ((Int32)(bytes[offset + 4] - Zero) * 1000) + ((Int32)(bytes[offset + 5] - Zero) * 100) + ((Int32)(bytes[offset + 6] - Zero) * 10) + ((Int32)(bytes[offset + 7] - Zero)));
                    case 9:
                        return (System.Int32)(((Int32)(bytes[offset] - Zero) * 100000000) + ((Int32)(bytes[offset + 1] - Zero) * 10000000) + ((Int32)(bytes[offset + 2] - Zero) * 1000000) + ((Int32)(bytes[offset + 3] - Zero) * 100000) + ((Int32)(bytes[offset + 4] - Zero) * 10000) + ((Int32)(bytes[offset + 5] - Zero) * 1000) + ((Int32)(bytes[offset + 6] - Zero) * 100) + ((Int32)(bytes[offset + 7] - Zero) * 10) + ((Int32)(bytes[offset + 8] - Zero)));
                    case 10:
                        return (System.Int32)(((Int32)(bytes[offset] - Zero) * 1000000000) + ((Int32)(bytes[offset + 1] - Zero) * 100000000) + ((Int32)(bytes[offset + 2] - Zero) * 10000000) + ((Int32)(bytes[offset + 3] - Zero) * 1000000) + ((Int32)(bytes[offset + 4] - Zero) * 100000) + ((Int32)(bytes[offset + 5] - Zero) * 10000) + ((Int32)(bytes[offset + 6] - Zero) * 1000) + ((Int32)(bytes[offset + 7] - Zero) * 100) + ((Int32)(bytes[offset + 8] - Zero) * 10) + ((Int32)(bytes[offset + 9] - Zero)));
                    default:
                        throw new ArgumentException("Int32 out of range count");
                }
            }
 
            else
            {
                switch (count)
                {
                    case 2:
                        return (System.Int32)((((Int32)(bytes[offset + 1] - Zero))) * -1);
                    case 3:
                        return (System.Int32)((((Int32)(bytes[offset + 1] - Zero) * 10) + ((Int32)(bytes[offset + 2] - Zero))) * -1);
                    case 4:
                        return (System.Int32)((((Int32)(bytes[offset + 1] - Zero) * 100) + ((Int32)(bytes[offset + 2] - Zero) * 10) + ((Int32)(bytes[offset + 3] - Zero))) * -1);
                    case 5:
                        return (System.Int32)((((Int32)(bytes[offset + 1] - Zero) * 1000) + ((Int32)(bytes[offset + 2] - Zero) * 100) + ((Int32)(bytes[offset + 3] - Zero) * 10) + ((Int32)(bytes[offset + 4] - Zero))) * -1);
                    case 6:
                        return (System.Int32)((((Int32)(bytes[offset + 1] - Zero) * 10000) + ((Int32)(bytes[offset + 2] - Zero) * 1000) + ((Int32)(bytes[offset + 3] - Zero) * 100) + ((Int32)(bytes[offset + 4] - Zero) * 10) + ((Int32)(bytes[offset + 5] - Zero))) * -1);
                    case 7:
                        return (System.Int32)((((Int32)(bytes[offset + 1] - Zero) * 100000) + ((Int32)(bytes[offset + 2] - Zero) * 10000) + ((Int32)(bytes[offset + 3] - Zero) * 1000) + ((Int32)(bytes[offset + 4] - Zero) * 100) + ((Int32)(bytes[offset + 5] - Zero) * 10) + ((Int32)(bytes[offset + 6] - Zero))) * -1);
                    case 8:
                        return (System.Int32)((((Int32)(bytes[offset + 1] - Zero) * 1000000) + ((Int32)(bytes[offset + 2] - Zero) * 100000) + ((Int32)(bytes[offset + 3] - Zero) * 10000) + ((Int32)(bytes[offset + 4] - Zero) * 1000) + ((Int32)(bytes[offset + 5] - Zero) * 100) + ((Int32)(bytes[offset + 6] - Zero) * 10) + ((Int32)(bytes[offset + 7] - Zero))) * -1);
                    case 9:
                        return (System.Int32)((((Int32)(bytes[offset + 1] - Zero) * 10000000) + ((Int32)(bytes[offset + 2] - Zero) * 1000000) + ((Int32)(bytes[offset + 3] - Zero) * 100000) + ((Int32)(bytes[offset + 4] - Zero) * 10000) + ((Int32)(bytes[offset + 5] - Zero) * 1000) + ((Int32)(bytes[offset + 6] - Zero) * 100) + ((Int32)(bytes[offset + 7] - Zero) * 10) + ((Int32)(bytes[offset + 8] - Zero))) * -1);
                    case 10:
                        return (System.Int32)((((Int32)(bytes[offset + 1] - Zero) * 100000000) + ((Int32)(bytes[offset + 2] - Zero) * 10000000) + ((Int32)(bytes[offset + 3] - Zero) * 1000000) + ((Int32)(bytes[offset + 4] - Zero) * 100000) + ((Int32)(bytes[offset + 5] - Zero) * 10000) + ((Int32)(bytes[offset + 6] - Zero) * 1000) + ((Int32)(bytes[offset + 7] - Zero) * 100) + ((Int32)(bytes[offset + 8] - Zero) * 10) + ((Int32)(bytes[offset + 9] - Zero))) * -1);
                    case 11:
                        return (System.Int32)((((Int32)(bytes[offset + 1] - Zero) * 1000000000) + ((Int32)(bytes[offset + 2] - Zero) * 100000000) + ((Int32)(bytes[offset + 3] - Zero) * 10000000) + ((Int32)(bytes[offset + 4] - Zero) * 1000000) + ((Int32)(bytes[offset + 5] - Zero) * 100000) + ((Int32)(bytes[offset + 6] - Zero) * 10000) + ((Int32)(bytes[offset + 7] - Zero) * 1000) + ((Int32)(bytes[offset + 8] - Zero) * 100) + ((Int32)(bytes[offset + 9] - Zero) * 10) + ((Int32)(bytes[offset + 10] - Zero))) * -1);
                    default:
                        throw new ArgumentException("Int32 out of range count");
                }
            }
        }
        public static UInt32 ToUInt32(byte[] bytes, int offset, int count)
        {
            // Min: 0
            // Max: 4294967295
            // Digits: 10
            {
                switch (count)
                {
                    case 1:
                        return (System.UInt32)(((UInt32)(bytes[offset] - Zero)));
                    case 2:
                        return (System.UInt32)(((UInt32)(bytes[offset] - Zero) * 10) + ((UInt32)(bytes[offset + 1] - Zero)));
                    case 3:
                        return (System.UInt32)(((UInt32)(bytes[offset] - Zero) * 100) + ((UInt32)(bytes[offset + 1] - Zero) * 10) + ((UInt32)(bytes[offset + 2] - Zero)));
                    case 4:
                        return (System.UInt32)(((UInt32)(bytes[offset] - Zero) * 1000) + ((UInt32)(bytes[offset + 1] - Zero) * 100) + ((UInt32)(bytes[offset + 2] - Zero) * 10) + ((UInt32)(bytes[offset + 3] - Zero)));
                    case 5:
                        return (System.UInt32)(((UInt32)(bytes[offset] - Zero) * 10000) + ((UInt32)(bytes[offset + 1] - Zero) * 1000) + ((UInt32)(bytes[offset + 2] - Zero) * 100) + ((UInt32)(bytes[offset + 3] - Zero) * 10) + ((UInt32)(bytes[offset + 4] - Zero)));
                    case 6:
                        return (System.UInt32)(((UInt32)(bytes[offset] - Zero) * 100000) + ((UInt32)(bytes[offset + 1] - Zero) * 10000) + ((UInt32)(bytes[offset + 2] - Zero) * 1000) + ((UInt32)(bytes[offset + 3] - Zero) * 100) + ((UInt32)(bytes[offset + 4] - Zero) * 10) + ((UInt32)(bytes[offset + 5] - Zero)));
                    case 7:
                        return (System.UInt32)(((UInt32)(bytes[offset] - Zero) * 1000000) + ((UInt32)(bytes[offset + 1] - Zero) * 100000) + ((UInt32)(bytes[offset + 2] - Zero) * 10000) + ((UInt32)(bytes[offset + 3] - Zero) * 1000) + ((UInt32)(bytes[offset + 4] - Zero) * 100) + ((UInt32)(bytes[offset + 5] - Zero) * 10) + ((UInt32)(bytes[offset + 6] - Zero)));
                    case 8:
                        return (System.UInt32)(((UInt32)(bytes[offset] - Zero) * 10000000) + ((UInt32)(bytes[offset + 1] - Zero) * 1000000) + ((UInt32)(bytes[offset + 2] - Zero) * 100000) + ((UInt32)(bytes[offset + 3] - Zero) * 10000) + ((UInt32)(bytes[offset + 4] - Zero) * 1000) + ((UInt32)(bytes[offset + 5] - Zero) * 100) + ((UInt32)(bytes[offset + 6] - Zero) * 10) + ((UInt32)(bytes[offset + 7] - Zero)));
                    case 9:
                        return (System.UInt32)(((UInt32)(bytes[offset] - Zero) * 100000000) + ((UInt32)(bytes[offset + 1] - Zero) * 10000000) + ((UInt32)(bytes[offset + 2] - Zero) * 1000000) + ((UInt32)(bytes[offset + 3] - Zero) * 100000) + ((UInt32)(bytes[offset + 4] - Zero) * 10000) + ((UInt32)(bytes[offset + 5] - Zero) * 1000) + ((UInt32)(bytes[offset + 6] - Zero) * 100) + ((UInt32)(bytes[offset + 7] - Zero) * 10) + ((UInt32)(bytes[offset + 8] - Zero)));
                    case 10:
                        return (System.UInt32)(((UInt32)(bytes[offset] - Zero) * 1000000000) + ((UInt32)(bytes[offset + 1] - Zero) * 100000000) + ((UInt32)(bytes[offset + 2] - Zero) * 10000000) + ((UInt32)(bytes[offset + 3] - Zero) * 1000000) + ((UInt32)(bytes[offset + 4] - Zero) * 100000) + ((UInt32)(bytes[offset + 5] - Zero) * 10000) + ((UInt32)(bytes[offset + 6] - Zero) * 1000) + ((UInt32)(bytes[offset + 7] - Zero) * 100) + ((UInt32)(bytes[offset + 8] - Zero) * 10) + ((UInt32)(bytes[offset + 9] - Zero)));
                    default:
                        throw new ArgumentException("UInt32 out of range count");
                }
            }
        }
        public static Int64 ToInt64(byte[] bytes, int offset, int count)
        {
            // Min: -9223372036854775808
            // Max: 9223372036854775807
            // Digits: 19
 
            if (bytes[offset] != Minus)
            {
                switch (count)
                {
                    case 1:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero)));
                    case 2:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 10) + ((Int64)(bytes[offset + 1] - Zero)));
                    case 3:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 100) + ((Int64)(bytes[offset + 1] - Zero) * 10) + ((Int64)(bytes[offset + 2] - Zero)));
                    case 4:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 1000) + ((Int64)(bytes[offset + 1] - Zero) * 100) + ((Int64)(bytes[offset + 2] - Zero) * 10) + ((Int64)(bytes[offset + 3] - Zero)));
                    case 5:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 10000) + ((Int64)(bytes[offset + 1] - Zero) * 1000) + ((Int64)(bytes[offset + 2] - Zero) * 100) + ((Int64)(bytes[offset + 3] - Zero) * 10) + ((Int64)(bytes[offset + 4] - Zero)));
                    case 6:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 100000) + ((Int64)(bytes[offset + 1] - Zero) * 10000) + ((Int64)(bytes[offset + 2] - Zero) * 1000) + ((Int64)(bytes[offset + 3] - Zero) * 100) + ((Int64)(bytes[offset + 4] - Zero) * 10) + ((Int64)(bytes[offset + 5] - Zero)));
                    case 7:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 1000000) + ((Int64)(bytes[offset + 1] - Zero) * 100000) + ((Int64)(bytes[offset + 2] - Zero) * 10000) + ((Int64)(bytes[offset + 3] - Zero) * 1000) + ((Int64)(bytes[offset + 4] - Zero) * 100) + ((Int64)(bytes[offset + 5] - Zero) * 10) + ((Int64)(bytes[offset + 6] - Zero)));
                    case 8:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 10000000) + ((Int64)(bytes[offset + 1] - Zero) * 1000000) + ((Int64)(bytes[offset + 2] - Zero) * 100000) + ((Int64)(bytes[offset + 3] - Zero) * 10000) + ((Int64)(bytes[offset + 4] - Zero) * 1000) + ((Int64)(bytes[offset + 5] - Zero) * 100) + ((Int64)(bytes[offset + 6] - Zero) * 10) + ((Int64)(bytes[offset + 7] - Zero)));
                    case 9:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 100000000) + ((Int64)(bytes[offset + 1] - Zero) * 10000000) + ((Int64)(bytes[offset + 2] - Zero) * 1000000) + ((Int64)(bytes[offset + 3] - Zero) * 100000) + ((Int64)(bytes[offset + 4] - Zero) * 10000) + ((Int64)(bytes[offset + 5] - Zero) * 1000) + ((Int64)(bytes[offset + 6] - Zero) * 100) + ((Int64)(bytes[offset + 7] - Zero) * 10) + ((Int64)(bytes[offset + 8] - Zero)));
                    case 10:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 1000000000) + ((Int64)(bytes[offset + 1] - Zero) * 100000000) + ((Int64)(bytes[offset + 2] - Zero) * 10000000) + ((Int64)(bytes[offset + 3] - Zero) * 1000000) + ((Int64)(bytes[offset + 4] - Zero) * 100000) + ((Int64)(bytes[offset + 5] - Zero) * 10000) + ((Int64)(bytes[offset + 6] - Zero) * 1000) + ((Int64)(bytes[offset + 7] - Zero) * 100) + ((Int64)(bytes[offset + 8] - Zero) * 10) + ((Int64)(bytes[offset + 9] - Zero)));
                    case 11:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 10000000000) + ((Int64)(bytes[offset + 1] - Zero) * 1000000000) + ((Int64)(bytes[offset + 2] - Zero) * 100000000) + ((Int64)(bytes[offset + 3] - Zero) * 10000000) + ((Int64)(bytes[offset + 4] - Zero) * 1000000) + ((Int64)(bytes[offset + 5] - Zero) * 100000) + ((Int64)(bytes[offset + 6] - Zero) * 10000) + ((Int64)(bytes[offset + 7] - Zero) * 1000) + ((Int64)(bytes[offset + 8] - Zero) * 100) + ((Int64)(bytes[offset + 9] - Zero) * 10) + ((Int64)(bytes[offset + 10] - Zero)));
                    case 12:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 100000000000) + ((Int64)(bytes[offset + 1] - Zero) * 10000000000) + ((Int64)(bytes[offset + 2] - Zero) * 1000000000) + ((Int64)(bytes[offset + 3] - Zero) * 100000000) + ((Int64)(bytes[offset + 4] - Zero) * 10000000) + ((Int64)(bytes[offset + 5] - Zero) * 1000000) + ((Int64)(bytes[offset + 6] - Zero) * 100000) + ((Int64)(bytes[offset + 7] - Zero) * 10000) + ((Int64)(bytes[offset + 8] - Zero) * 1000) + ((Int64)(bytes[offset + 9] - Zero) * 100) + ((Int64)(bytes[offset + 10] - Zero) * 10) + ((Int64)(bytes[offset + 11] - Zero)));
                    case 13:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 1000000000000) + ((Int64)(bytes[offset + 1] - Zero) * 100000000000) + ((Int64)(bytes[offset + 2] - Zero) * 10000000000) + ((Int64)(bytes[offset + 3] - Zero) * 1000000000) + ((Int64)(bytes[offset + 4] - Zero) * 100000000) + ((Int64)(bytes[offset + 5] - Zero) * 10000000) + ((Int64)(bytes[offset + 6] - Zero) * 1000000) + ((Int64)(bytes[offset + 7] - Zero) * 100000) + ((Int64)(bytes[offset + 8] - Zero) * 10000) + ((Int64)(bytes[offset + 9] - Zero) * 1000) + ((Int64)(bytes[offset + 10] - Zero) * 100) + ((Int64)(bytes[offset + 11] - Zero) * 10) + ((Int64)(bytes[offset + 12] - Zero)));
                    case 14:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 10000000000000) + ((Int64)(bytes[offset + 1] - Zero) * 1000000000000) + ((Int64)(bytes[offset + 2] - Zero) * 100000000000) + ((Int64)(bytes[offset + 3] - Zero) * 10000000000) + ((Int64)(bytes[offset + 4] - Zero) * 1000000000) + ((Int64)(bytes[offset + 5] - Zero) * 100000000) + ((Int64)(bytes[offset + 6] - Zero) * 10000000) + ((Int64)(bytes[offset + 7] - Zero) * 1000000) + ((Int64)(bytes[offset + 8] - Zero) * 100000) + ((Int64)(bytes[offset + 9] - Zero) * 10000) + ((Int64)(bytes[offset + 10] - Zero) * 1000) + ((Int64)(bytes[offset + 11] - Zero) * 100) + ((Int64)(bytes[offset + 12] - Zero) * 10) + ((Int64)(bytes[offset + 13] - Zero)));
                    case 15:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 100000000000000) + ((Int64)(bytes[offset + 1] - Zero) * 10000000000000) + ((Int64)(bytes[offset + 2] - Zero) * 1000000000000) + ((Int64)(bytes[offset + 3] - Zero) * 100000000000) + ((Int64)(bytes[offset + 4] - Zero) * 10000000000) + ((Int64)(bytes[offset + 5] - Zero) * 1000000000) + ((Int64)(bytes[offset + 6] - Zero) * 100000000) + ((Int64)(bytes[offset + 7] - Zero) * 10000000) + ((Int64)(bytes[offset + 8] - Zero) * 1000000) + ((Int64)(bytes[offset + 9] - Zero) * 100000) + ((Int64)(bytes[offset + 10] - Zero) * 10000) + ((Int64)(bytes[offset + 11] - Zero) * 1000) + ((Int64)(bytes[offset + 12] - Zero) * 100) + ((Int64)(bytes[offset + 13] - Zero) * 10) + ((Int64)(bytes[offset + 14] - Zero)));
                    case 16:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 1000000000000000) + ((Int64)(bytes[offset + 1] - Zero) * 100000000000000) + ((Int64)(bytes[offset + 2] - Zero) * 10000000000000) + ((Int64)(bytes[offset + 3] - Zero) * 1000000000000) + ((Int64)(bytes[offset + 4] - Zero) * 100000000000) + ((Int64)(bytes[offset + 5] - Zero) * 10000000000) + ((Int64)(bytes[offset + 6] - Zero) * 1000000000) + ((Int64)(bytes[offset + 7] - Zero) * 100000000) + ((Int64)(bytes[offset + 8] - Zero) * 10000000) + ((Int64)(bytes[offset + 9] - Zero) * 1000000) + ((Int64)(bytes[offset + 10] - Zero) * 100000) + ((Int64)(bytes[offset + 11] - Zero) * 10000) + ((Int64)(bytes[offset + 12] - Zero) * 1000) + ((Int64)(bytes[offset + 13] - Zero) * 100) + ((Int64)(bytes[offset + 14] - Zero) * 10) + ((Int64)(bytes[offset + 15] - Zero)));
                    case 17:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 10000000000000000) + ((Int64)(bytes[offset + 1] - Zero) * 1000000000000000) + ((Int64)(bytes[offset + 2] - Zero) * 100000000000000) + ((Int64)(bytes[offset + 3] - Zero) * 10000000000000) + ((Int64)(bytes[offset + 4] - Zero) * 1000000000000) + ((Int64)(bytes[offset + 5] - Zero) * 100000000000) + ((Int64)(bytes[offset + 6] - Zero) * 10000000000) + ((Int64)(bytes[offset + 7] - Zero) * 1000000000) + ((Int64)(bytes[offset + 8] - Zero) * 100000000) + ((Int64)(bytes[offset + 9] - Zero) * 10000000) + ((Int64)(bytes[offset + 10] - Zero) * 1000000) + ((Int64)(bytes[offset + 11] - Zero) * 100000) + ((Int64)(bytes[offset + 12] - Zero) * 10000) + ((Int64)(bytes[offset + 13] - Zero) * 1000) + ((Int64)(bytes[offset + 14] - Zero) * 100) + ((Int64)(bytes[offset + 15] - Zero) * 10) + ((Int64)(bytes[offset + 16] - Zero)));
                    case 18:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 100000000000000000) + ((Int64)(bytes[offset + 1] - Zero) * 10000000000000000) + ((Int64)(bytes[offset + 2] - Zero) * 1000000000000000) + ((Int64)(bytes[offset + 3] - Zero) * 100000000000000) + ((Int64)(bytes[offset + 4] - Zero) * 10000000000000) + ((Int64)(bytes[offset + 5] - Zero) * 1000000000000) + ((Int64)(bytes[offset + 6] - Zero) * 100000000000) + ((Int64)(bytes[offset + 7] - Zero) * 10000000000) + ((Int64)(bytes[offset + 8] - Zero) * 1000000000) + ((Int64)(bytes[offset + 9] - Zero) * 100000000) + ((Int64)(bytes[offset + 10] - Zero) * 10000000) + ((Int64)(bytes[offset + 11] - Zero) * 1000000) + ((Int64)(bytes[offset + 12] - Zero) * 100000) + ((Int64)(bytes[offset + 13] - Zero) * 10000) + ((Int64)(bytes[offset + 14] - Zero) * 1000) + ((Int64)(bytes[offset + 15] - Zero) * 100) + ((Int64)(bytes[offset + 16] - Zero) * 10) + ((Int64)(bytes[offset + 17] - Zero)));
                    case 19:
                        return (System.Int64)(((Int64)(bytes[offset] - Zero) * 1000000000000000000) + ((Int64)(bytes[offset + 1] - Zero) * 100000000000000000) + ((Int64)(bytes[offset + 2] - Zero) * 10000000000000000) + ((Int64)(bytes[offset + 3] - Zero) * 1000000000000000) + ((Int64)(bytes[offset + 4] - Zero) * 100000000000000) + ((Int64)(bytes[offset + 5] - Zero) * 10000000000000) + ((Int64)(bytes[offset + 6] - Zero) * 1000000000000) + ((Int64)(bytes[offset + 7] - Zero) * 100000000000) + ((Int64)(bytes[offset + 8] - Zero) * 10000000000) + ((Int64)(bytes[offset + 9] - Zero) * 1000000000) + ((Int64)(bytes[offset + 10] - Zero) * 100000000) + ((Int64)(bytes[offset + 11] - Zero) * 10000000) + ((Int64)(bytes[offset + 12] - Zero) * 1000000) + ((Int64)(bytes[offset + 13] - Zero) * 100000) + ((Int64)(bytes[offset + 14] - Zero) * 10000) + ((Int64)(bytes[offset + 15] - Zero) * 1000) + ((Int64)(bytes[offset + 16] - Zero) * 100) + ((Int64)(bytes[offset + 17] - Zero) * 10) + ((Int64)(bytes[offset + 18] - Zero)));
                    default:
                        throw new ArgumentException("Int64 out of range count");
                }
            }
 
            else
            {
                switch (count)
                {
                    case 2:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero))) * -1);
                    case 3:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 10) + ((Int64)(bytes[offset + 2] - Zero))) * -1);
                    case 4:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 100) + ((Int64)(bytes[offset + 2] - Zero) * 10) + ((Int64)(bytes[offset + 3] - Zero))) * -1);
                    case 5:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 1000) + ((Int64)(bytes[offset + 2] - Zero) * 100) + ((Int64)(bytes[offset + 3] - Zero) * 10) + ((Int64)(bytes[offset + 4] - Zero))) * -1);
                    case 6:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 10000) + ((Int64)(bytes[offset + 2] - Zero) * 1000) + ((Int64)(bytes[offset + 3] - Zero) * 100) + ((Int64)(bytes[offset + 4] - Zero) * 10) + ((Int64)(bytes[offset + 5] - Zero))) * -1);
                    case 7:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 100000) + ((Int64)(bytes[offset + 2] - Zero) * 10000) + ((Int64)(bytes[offset + 3] - Zero) * 1000) + ((Int64)(bytes[offset + 4] - Zero) * 100) + ((Int64)(bytes[offset + 5] - Zero) * 10) + ((Int64)(bytes[offset + 6] - Zero))) * -1);
                    case 8:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 1000000) + ((Int64)(bytes[offset + 2] - Zero) * 100000) + ((Int64)(bytes[offset + 3] - Zero) * 10000) + ((Int64)(bytes[offset + 4] - Zero) * 1000) + ((Int64)(bytes[offset + 5] - Zero) * 100) + ((Int64)(bytes[offset + 6] - Zero) * 10) + ((Int64)(bytes[offset + 7] - Zero))) * -1);
                    case 9:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 10000000) + ((Int64)(bytes[offset + 2] - Zero) * 1000000) + ((Int64)(bytes[offset + 3] - Zero) * 100000) + ((Int64)(bytes[offset + 4] - Zero) * 10000) + ((Int64)(bytes[offset + 5] - Zero) * 1000) + ((Int64)(bytes[offset + 6] - Zero) * 100) + ((Int64)(bytes[offset + 7] - Zero) * 10) + ((Int64)(bytes[offset + 8] - Zero))) * -1);
                    case 10:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 100000000) + ((Int64)(bytes[offset + 2] - Zero) * 10000000) + ((Int64)(bytes[offset + 3] - Zero) * 1000000) + ((Int64)(bytes[offset + 4] - Zero) * 100000) + ((Int64)(bytes[offset + 5] - Zero) * 10000) + ((Int64)(bytes[offset + 6] - Zero) * 1000) + ((Int64)(bytes[offset + 7] - Zero) * 100) + ((Int64)(bytes[offset + 8] - Zero) * 10) + ((Int64)(bytes[offset + 9] - Zero))) * -1);
                    case 11:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 1000000000) + ((Int64)(bytes[offset + 2] - Zero) * 100000000) + ((Int64)(bytes[offset + 3] - Zero) * 10000000) + ((Int64)(bytes[offset + 4] - Zero) * 1000000) + ((Int64)(bytes[offset + 5] - Zero) * 100000) + ((Int64)(bytes[offset + 6] - Zero) * 10000) + ((Int64)(bytes[offset + 7] - Zero) * 1000) + ((Int64)(bytes[offset + 8] - Zero) * 100) + ((Int64)(bytes[offset + 9] - Zero) * 10) + ((Int64)(bytes[offset + 10] - Zero))) * -1);
                    case 12:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 10000000000) + ((Int64)(bytes[offset + 2] - Zero) * 1000000000) + ((Int64)(bytes[offset + 3] - Zero) * 100000000) + ((Int64)(bytes[offset + 4] - Zero) * 10000000) + ((Int64)(bytes[offset + 5] - Zero) * 1000000) + ((Int64)(bytes[offset + 6] - Zero) * 100000) + ((Int64)(bytes[offset + 7] - Zero) * 10000) + ((Int64)(bytes[offset + 8] - Zero) * 1000) + ((Int64)(bytes[offset + 9] - Zero) * 100) + ((Int64)(bytes[offset + 10] - Zero) * 10) + ((Int64)(bytes[offset + 11] - Zero))) * -1);
                    case 13:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 100000000000) + ((Int64)(bytes[offset + 2] - Zero) * 10000000000) + ((Int64)(bytes[offset + 3] - Zero) * 1000000000) + ((Int64)(bytes[offset + 4] - Zero) * 100000000) + ((Int64)(bytes[offset + 5] - Zero) * 10000000) + ((Int64)(bytes[offset + 6] - Zero) * 1000000) + ((Int64)(bytes[offset + 7] - Zero) * 100000) + ((Int64)(bytes[offset + 8] - Zero) * 10000) + ((Int64)(bytes[offset + 9] - Zero) * 1000) + ((Int64)(bytes[offset + 10] - Zero) * 100) + ((Int64)(bytes[offset + 11] - Zero) * 10) + ((Int64)(bytes[offset + 12] - Zero))) * -1);
                    case 14:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 1000000000000) + ((Int64)(bytes[offset + 2] - Zero) * 100000000000) + ((Int64)(bytes[offset + 3] - Zero) * 10000000000) + ((Int64)(bytes[offset + 4] - Zero) * 1000000000) + ((Int64)(bytes[offset + 5] - Zero) * 100000000) + ((Int64)(bytes[offset + 6] - Zero) * 10000000) + ((Int64)(bytes[offset + 7] - Zero) * 1000000) + ((Int64)(bytes[offset + 8] - Zero) * 100000) + ((Int64)(bytes[offset + 9] - Zero) * 10000) + ((Int64)(bytes[offset + 10] - Zero) * 1000) + ((Int64)(bytes[offset + 11] - Zero) * 100) + ((Int64)(bytes[offset + 12] - Zero) * 10) + ((Int64)(bytes[offset + 13] - Zero))) * -1);
                    case 15:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 10000000000000) + ((Int64)(bytes[offset + 2] - Zero) * 1000000000000) + ((Int64)(bytes[offset + 3] - Zero) * 100000000000) + ((Int64)(bytes[offset + 4] - Zero) * 10000000000) + ((Int64)(bytes[offset + 5] - Zero) * 1000000000) + ((Int64)(bytes[offset + 6] - Zero) * 100000000) + ((Int64)(bytes[offset + 7] - Zero) * 10000000) + ((Int64)(bytes[offset + 8] - Zero) * 1000000) + ((Int64)(bytes[offset + 9] - Zero) * 100000) + ((Int64)(bytes[offset + 10] - Zero) * 10000) + ((Int64)(bytes[offset + 11] - Zero) * 1000) + ((Int64)(bytes[offset + 12] - Zero) * 100) + ((Int64)(bytes[offset + 13] - Zero) * 10) + ((Int64)(bytes[offset + 14] - Zero))) * -1);
                    case 16:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 100000000000000) + ((Int64)(bytes[offset + 2] - Zero) * 10000000000000) + ((Int64)(bytes[offset + 3] - Zero) * 1000000000000) + ((Int64)(bytes[offset + 4] - Zero) * 100000000000) + ((Int64)(bytes[offset + 5] - Zero) * 10000000000) + ((Int64)(bytes[offset + 6] - Zero) * 1000000000) + ((Int64)(bytes[offset + 7] - Zero) * 100000000) + ((Int64)(bytes[offset + 8] - Zero) * 10000000) + ((Int64)(bytes[offset + 9] - Zero) * 1000000) + ((Int64)(bytes[offset + 10] - Zero) * 100000) + ((Int64)(bytes[offset + 11] - Zero) * 10000) + ((Int64)(bytes[offset + 12] - Zero) * 1000) + ((Int64)(bytes[offset + 13] - Zero) * 100) + ((Int64)(bytes[offset + 14] - Zero) * 10) + ((Int64)(bytes[offset + 15] - Zero))) * -1);
                    case 17:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 1000000000000000) + ((Int64)(bytes[offset + 2] - Zero) * 100000000000000) + ((Int64)(bytes[offset + 3] - Zero) * 10000000000000) + ((Int64)(bytes[offset + 4] - Zero) * 1000000000000) + ((Int64)(bytes[offset + 5] - Zero) * 100000000000) + ((Int64)(bytes[offset + 6] - Zero) * 10000000000) + ((Int64)(bytes[offset + 7] - Zero) * 1000000000) + ((Int64)(bytes[offset + 8] - Zero) * 100000000) + ((Int64)(bytes[offset + 9] - Zero) * 10000000) + ((Int64)(bytes[offset + 10] - Zero) * 1000000) + ((Int64)(bytes[offset + 11] - Zero) * 100000) + ((Int64)(bytes[offset + 12] - Zero) * 10000) + ((Int64)(bytes[offset + 13] - Zero) * 1000) + ((Int64)(bytes[offset + 14] - Zero) * 100) + ((Int64)(bytes[offset + 15] - Zero) * 10) + ((Int64)(bytes[offset + 16] - Zero))) * -1);
                    case 18:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 10000000000000000) + ((Int64)(bytes[offset + 2] - Zero) * 1000000000000000) + ((Int64)(bytes[offset + 3] - Zero) * 100000000000000) + ((Int64)(bytes[offset + 4] - Zero) * 10000000000000) + ((Int64)(bytes[offset + 5] - Zero) * 1000000000000) + ((Int64)(bytes[offset + 6] - Zero) * 100000000000) + ((Int64)(bytes[offset + 7] - Zero) * 10000000000) + ((Int64)(bytes[offset + 8] - Zero) * 1000000000) + ((Int64)(bytes[offset + 9] - Zero) * 100000000) + ((Int64)(bytes[offset + 10] - Zero) * 10000000) + ((Int64)(bytes[offset + 11] - Zero) * 1000000) + ((Int64)(bytes[offset + 12] - Zero) * 100000) + ((Int64)(bytes[offset + 13] - Zero) * 10000) + ((Int64)(bytes[offset + 14] - Zero) * 1000) + ((Int64)(bytes[offset + 15] - Zero) * 100) + ((Int64)(bytes[offset + 16] - Zero) * 10) + ((Int64)(bytes[offset + 17] - Zero))) * -1);
                    case 19:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 100000000000000000) + ((Int64)(bytes[offset + 2] - Zero) * 10000000000000000) + ((Int64)(bytes[offset + 3] - Zero) * 1000000000000000) + ((Int64)(bytes[offset + 4] - Zero) * 100000000000000) + ((Int64)(bytes[offset + 5] - Zero) * 10000000000000) + ((Int64)(bytes[offset + 6] - Zero) * 1000000000000) + ((Int64)(bytes[offset + 7] - Zero) * 100000000000) + ((Int64)(bytes[offset + 8] - Zero) * 10000000000) + ((Int64)(bytes[offset + 9] - Zero) * 1000000000) + ((Int64)(bytes[offset + 10] - Zero) * 100000000) + ((Int64)(bytes[offset + 11] - Zero) * 10000000) + ((Int64)(bytes[offset + 12] - Zero) * 1000000) + ((Int64)(bytes[offset + 13] - Zero) * 100000) + ((Int64)(bytes[offset + 14] - Zero) * 10000) + ((Int64)(bytes[offset + 15] - Zero) * 1000) + ((Int64)(bytes[offset + 16] - Zero) * 100) + ((Int64)(bytes[offset + 17] - Zero) * 10) + ((Int64)(bytes[offset + 18] - Zero))) * -1);
                    case 20:
                        return (System.Int64)((((Int64)(bytes[offset + 1] - Zero) * 1000000000000000000) + ((Int64)(bytes[offset + 2] - Zero) * 100000000000000000) + ((Int64)(bytes[offset + 3] - Zero) * 10000000000000000) + ((Int64)(bytes[offset + 4] - Zero) * 1000000000000000) + ((Int64)(bytes[offset + 5] - Zero) * 100000000000000) + ((Int64)(bytes[offset + 6] - Zero) * 10000000000000) + ((Int64)(bytes[offset + 7] - Zero) * 1000000000000) + ((Int64)(bytes[offset + 8] - Zero) * 100000000000) + ((Int64)(bytes[offset + 9] - Zero) * 10000000000) + ((Int64)(bytes[offset + 10] - Zero) * 1000000000) + ((Int64)(bytes[offset + 11] - Zero) * 100000000) + ((Int64)(bytes[offset + 12] - Zero) * 10000000) + ((Int64)(bytes[offset + 13] - Zero) * 1000000) + ((Int64)(bytes[offset + 14] - Zero) * 100000) + ((Int64)(bytes[offset + 15] - Zero) * 10000) + ((Int64)(bytes[offset + 16] - Zero) * 1000) + ((Int64)(bytes[offset + 17] - Zero) * 100) + ((Int64)(bytes[offset + 18] - Zero) * 10) + ((Int64)(bytes[offset + 19] - Zero))) * -1);
                    default:
                        throw new ArgumentException("Int64 out of range count");
                }
            }
        }
        public static UInt64 ToUInt64(byte[] bytes, int offset, int count)
        {
            // Min: 0
            // Max: 18446744073709551615
            // Digits: 20
            {
                switch (count)
                {
                    case 1:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero)));
                    case 2:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 10) + ((UInt64)(bytes[offset + 1] - Zero)));
                    case 3:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 100) + ((UInt64)(bytes[offset + 1] - Zero) * 10) + ((UInt64)(bytes[offset + 2] - Zero)));
                    case 4:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 1000) + ((UInt64)(bytes[offset + 1] - Zero) * 100) + ((UInt64)(bytes[offset + 2] - Zero) * 10) + ((UInt64)(bytes[offset + 3] - Zero)));
                    case 5:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 10000) + ((UInt64)(bytes[offset + 1] - Zero) * 1000) + ((UInt64)(bytes[offset + 2] - Zero) * 100) + ((UInt64)(bytes[offset + 3] - Zero) * 10) + ((UInt64)(bytes[offset + 4] - Zero)));
                    case 6:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 100000) + ((UInt64)(bytes[offset + 1] - Zero) * 10000) + ((UInt64)(bytes[offset + 2] - Zero) * 1000) + ((UInt64)(bytes[offset + 3] - Zero) * 100) + ((UInt64)(bytes[offset + 4] - Zero) * 10) + ((UInt64)(bytes[offset + 5] - Zero)));
                    case 7:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 1000000) + ((UInt64)(bytes[offset + 1] - Zero) * 100000) + ((UInt64)(bytes[offset + 2] - Zero) * 10000) + ((UInt64)(bytes[offset + 3] - Zero) * 1000) + ((UInt64)(bytes[offset + 4] - Zero) * 100) + ((UInt64)(bytes[offset + 5] - Zero) * 10) + ((UInt64)(bytes[offset + 6] - Zero)));
                    case 8:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 10000000) + ((UInt64)(bytes[offset + 1] - Zero) * 1000000) + ((UInt64)(bytes[offset + 2] - Zero) * 100000) + ((UInt64)(bytes[offset + 3] - Zero) * 10000) + ((UInt64)(bytes[offset + 4] - Zero) * 1000) + ((UInt64)(bytes[offset + 5] - Zero) * 100) + ((UInt64)(bytes[offset + 6] - Zero) * 10) + ((UInt64)(bytes[offset + 7] - Zero)));
                    case 9:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 100000000) + ((UInt64)(bytes[offset + 1] - Zero) * 10000000) + ((UInt64)(bytes[offset + 2] - Zero) * 1000000) + ((UInt64)(bytes[offset + 3] - Zero) * 100000) + ((UInt64)(bytes[offset + 4] - Zero) * 10000) + ((UInt64)(bytes[offset + 5] - Zero) * 1000) + ((UInt64)(bytes[offset + 6] - Zero) * 100) + ((UInt64)(bytes[offset + 7] - Zero) * 10) + ((UInt64)(bytes[offset + 8] - Zero)));
                    case 10:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 1000000000) + ((UInt64)(bytes[offset + 1] - Zero) * 100000000) + ((UInt64)(bytes[offset + 2] - Zero) * 10000000) + ((UInt64)(bytes[offset + 3] - Zero) * 1000000) + ((UInt64)(bytes[offset + 4] - Zero) * 100000) + ((UInt64)(bytes[offset + 5] - Zero) * 10000) + ((UInt64)(bytes[offset + 6] - Zero) * 1000) + ((UInt64)(bytes[offset + 7] - Zero) * 100) + ((UInt64)(bytes[offset + 8] - Zero) * 10) + ((UInt64)(bytes[offset + 9] - Zero)));
                    case 11:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 10000000000) + ((UInt64)(bytes[offset + 1] - Zero) * 1000000000) + ((UInt64)(bytes[offset + 2] - Zero) * 100000000) + ((UInt64)(bytes[offset + 3] - Zero) * 10000000) + ((UInt64)(bytes[offset + 4] - Zero) * 1000000) + ((UInt64)(bytes[offset + 5] - Zero) * 100000) + ((UInt64)(bytes[offset + 6] - Zero) * 10000) + ((UInt64)(bytes[offset + 7] - Zero) * 1000) + ((UInt64)(bytes[offset + 8] - Zero) * 100) + ((UInt64)(bytes[offset + 9] - Zero) * 10) + ((UInt64)(bytes[offset + 10] - Zero)));
                    case 12:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 100000000000) + ((UInt64)(bytes[offset + 1] - Zero) * 10000000000) + ((UInt64)(bytes[offset + 2] - Zero) * 1000000000) + ((UInt64)(bytes[offset + 3] - Zero) * 100000000) + ((UInt64)(bytes[offset + 4] - Zero) * 10000000) + ((UInt64)(bytes[offset + 5] - Zero) * 1000000) + ((UInt64)(bytes[offset + 6] - Zero) * 100000) + ((UInt64)(bytes[offset + 7] - Zero) * 10000) + ((UInt64)(bytes[offset + 8] - Zero) * 1000) + ((UInt64)(bytes[offset + 9] - Zero) * 100) + ((UInt64)(bytes[offset + 10] - Zero) * 10) + ((UInt64)(bytes[offset + 11] - Zero)));
                    case 13:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 1000000000000) + ((UInt64)(bytes[offset + 1] - Zero) * 100000000000) + ((UInt64)(bytes[offset + 2] - Zero) * 10000000000) + ((UInt64)(bytes[offset + 3] - Zero) * 1000000000) + ((UInt64)(bytes[offset + 4] - Zero) * 100000000) + ((UInt64)(bytes[offset + 5] - Zero) * 10000000) + ((UInt64)(bytes[offset + 6] - Zero) * 1000000) + ((UInt64)(bytes[offset + 7] - Zero) * 100000) + ((UInt64)(bytes[offset + 8] - Zero) * 10000) + ((UInt64)(bytes[offset + 9] - Zero) * 1000) + ((UInt64)(bytes[offset + 10] - Zero) * 100) + ((UInt64)(bytes[offset + 11] - Zero) * 10) + ((UInt64)(bytes[offset + 12] - Zero)));
                    case 14:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 10000000000000) + ((UInt64)(bytes[offset + 1] - Zero) * 1000000000000) + ((UInt64)(bytes[offset + 2] - Zero) * 100000000000) + ((UInt64)(bytes[offset + 3] - Zero) * 10000000000) + ((UInt64)(bytes[offset + 4] - Zero) * 1000000000) + ((UInt64)(bytes[offset + 5] - Zero) * 100000000) + ((UInt64)(bytes[offset + 6] - Zero) * 10000000) + ((UInt64)(bytes[offset + 7] - Zero) * 1000000) + ((UInt64)(bytes[offset + 8] - Zero) * 100000) + ((UInt64)(bytes[offset + 9] - Zero) * 10000) + ((UInt64)(bytes[offset + 10] - Zero) * 1000) + ((UInt64)(bytes[offset + 11] - Zero) * 100) + ((UInt64)(bytes[offset + 12] - Zero) * 10) + ((UInt64)(bytes[offset + 13] - Zero)));
                    case 15:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 100000000000000) + ((UInt64)(bytes[offset + 1] - Zero) * 10000000000000) + ((UInt64)(bytes[offset + 2] - Zero) * 1000000000000) + ((UInt64)(bytes[offset + 3] - Zero) * 100000000000) + ((UInt64)(bytes[offset + 4] - Zero) * 10000000000) + ((UInt64)(bytes[offset + 5] - Zero) * 1000000000) + ((UInt64)(bytes[offset + 6] - Zero) * 100000000) + ((UInt64)(bytes[offset + 7] - Zero) * 10000000) + ((UInt64)(bytes[offset + 8] - Zero) * 1000000) + ((UInt64)(bytes[offset + 9] - Zero) * 100000) + ((UInt64)(bytes[offset + 10] - Zero) * 10000) + ((UInt64)(bytes[offset + 11] - Zero) * 1000) + ((UInt64)(bytes[offset + 12] - Zero) * 100) + ((UInt64)(bytes[offset + 13] - Zero) * 10) + ((UInt64)(bytes[offset + 14] - Zero)));
                    case 16:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 1000000000000000) + ((UInt64)(bytes[offset + 1] - Zero) * 100000000000000) + ((UInt64)(bytes[offset + 2] - Zero) * 10000000000000) + ((UInt64)(bytes[offset + 3] - Zero) * 1000000000000) + ((UInt64)(bytes[offset + 4] - Zero) * 100000000000) + ((UInt64)(bytes[offset + 5] - Zero) * 10000000000) + ((UInt64)(bytes[offset + 6] - Zero) * 1000000000) + ((UInt64)(bytes[offset + 7] - Zero) * 100000000) + ((UInt64)(bytes[offset + 8] - Zero) * 10000000) + ((UInt64)(bytes[offset + 9] - Zero) * 1000000) + ((UInt64)(bytes[offset + 10] - Zero) * 100000) + ((UInt64)(bytes[offset + 11] - Zero) * 10000) + ((UInt64)(bytes[offset + 12] - Zero) * 1000) + ((UInt64)(bytes[offset + 13] - Zero) * 100) + ((UInt64)(bytes[offset + 14] - Zero) * 10) + ((UInt64)(bytes[offset + 15] - Zero)));
                    case 17:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 10000000000000000) + ((UInt64)(bytes[offset + 1] - Zero) * 1000000000000000) + ((UInt64)(bytes[offset + 2] - Zero) * 100000000000000) + ((UInt64)(bytes[offset + 3] - Zero) * 10000000000000) + ((UInt64)(bytes[offset + 4] - Zero) * 1000000000000) + ((UInt64)(bytes[offset + 5] - Zero) * 100000000000) + ((UInt64)(bytes[offset + 6] - Zero) * 10000000000) + ((UInt64)(bytes[offset + 7] - Zero) * 1000000000) + ((UInt64)(bytes[offset + 8] - Zero) * 100000000) + ((UInt64)(bytes[offset + 9] - Zero) * 10000000) + ((UInt64)(bytes[offset + 10] - Zero) * 1000000) + ((UInt64)(bytes[offset + 11] - Zero) * 100000) + ((UInt64)(bytes[offset + 12] - Zero) * 10000) + ((UInt64)(bytes[offset + 13] - Zero) * 1000) + ((UInt64)(bytes[offset + 14] - Zero) * 100) + ((UInt64)(bytes[offset + 15] - Zero) * 10) + ((UInt64)(bytes[offset + 16] - Zero)));
                    case 18:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 100000000000000000) + ((UInt64)(bytes[offset + 1] - Zero) * 10000000000000000) + ((UInt64)(bytes[offset + 2] - Zero) * 1000000000000000) + ((UInt64)(bytes[offset + 3] - Zero) * 100000000000000) + ((UInt64)(bytes[offset + 4] - Zero) * 10000000000000) + ((UInt64)(bytes[offset + 5] - Zero) * 1000000000000) + ((UInt64)(bytes[offset + 6] - Zero) * 100000000000) + ((UInt64)(bytes[offset + 7] - Zero) * 10000000000) + ((UInt64)(bytes[offset + 8] - Zero) * 1000000000) + ((UInt64)(bytes[offset + 9] - Zero) * 100000000) + ((UInt64)(bytes[offset + 10] - Zero) * 10000000) + ((UInt64)(bytes[offset + 11] - Zero) * 1000000) + ((UInt64)(bytes[offset + 12] - Zero) * 100000) + ((UInt64)(bytes[offset + 13] - Zero) * 10000) + ((UInt64)(bytes[offset + 14] - Zero) * 1000) + ((UInt64)(bytes[offset + 15] - Zero) * 100) + ((UInt64)(bytes[offset + 16] - Zero) * 10) + ((UInt64)(bytes[offset + 17] - Zero)));
                    case 19:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 1000000000000000000) + ((UInt64)(bytes[offset + 1] - Zero) * 100000000000000000) + ((UInt64)(bytes[offset + 2] - Zero) * 10000000000000000) + ((UInt64)(bytes[offset + 3] - Zero) * 1000000000000000) + ((UInt64)(bytes[offset + 4] - Zero) * 100000000000000) + ((UInt64)(bytes[offset + 5] - Zero) * 10000000000000) + ((UInt64)(bytes[offset + 6] - Zero) * 1000000000000) + ((UInt64)(bytes[offset + 7] - Zero) * 100000000000) + ((UInt64)(bytes[offset + 8] - Zero) * 10000000000) + ((UInt64)(bytes[offset + 9] - Zero) * 1000000000) + ((UInt64)(bytes[offset + 10] - Zero) * 100000000) + ((UInt64)(bytes[offset + 11] - Zero) * 10000000) + ((UInt64)(bytes[offset + 12] - Zero) * 1000000) + ((UInt64)(bytes[offset + 13] - Zero) * 100000) + ((UInt64)(bytes[offset + 14] - Zero) * 10000) + ((UInt64)(bytes[offset + 15] - Zero) * 1000) + ((UInt64)(bytes[offset + 16] - Zero) * 100) + ((UInt64)(bytes[offset + 17] - Zero) * 10) + ((UInt64)(bytes[offset + 18] - Zero)));
                    case 20:
                        return (System.UInt64)(((UInt64)(bytes[offset] - Zero) * 10000000000000000000) + ((UInt64)(bytes[offset + 1] - Zero) * 1000000000000000000) + ((UInt64)(bytes[offset + 2] - Zero) * 100000000000000000) + ((UInt64)(bytes[offset + 3] - Zero) * 10000000000000000) + ((UInt64)(bytes[offset + 4] - Zero) * 1000000000000000) + ((UInt64)(bytes[offset + 5] - Zero) * 100000000000000) + ((UInt64)(bytes[offset + 6] - Zero) * 10000000000000) + ((UInt64)(bytes[offset + 7] - Zero) * 1000000000000) + ((UInt64)(bytes[offset + 8] - Zero) * 100000000000) + ((UInt64)(bytes[offset + 9] - Zero) * 10000000000) + ((UInt64)(bytes[offset + 10] - Zero) * 1000000000) + ((UInt64)(bytes[offset + 11] - Zero) * 100000000) + ((UInt64)(bytes[offset + 12] - Zero) * 10000000) + ((UInt64)(bytes[offset + 13] - Zero) * 1000000) + ((UInt64)(bytes[offset + 14] - Zero) * 100000) + ((UInt64)(bytes[offset + 15] - Zero) * 10000) + ((UInt64)(bytes[offset + 16] - Zero) * 1000) + ((UInt64)(bytes[offset + 17] - Zero) * 100) + ((UInt64)(bytes[offset + 18] - Zero) * 10) + ((UInt64)(bytes[offset + 19] - Zero)));
                    default:
                        throw new ArgumentException("UInt64 out of range count");
                }
            }
        }
    }
}
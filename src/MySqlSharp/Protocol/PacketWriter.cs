using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using MySqlSharp.Internal;

namespace MySqlSharp.Protocol
{
    // be careful, this is "struct".

    public struct PacketWriter
    {
        byte[] bytes;
        int offset;

        public int CurrentOffset => offset;

        public ArraySegment<byte> GetBuffer(int sequenceId)
        {
            // Writer header
            var payloadSize = offset - 4;
            BinaryUtil.Write3BytesInt32(ref bytes, 0, payloadSize);
            BinaryUtil.WriteByte(ref bytes, 3, (byte)sequenceId);

            return new ArraySegment<byte>(bytes, 0, offset);
        }

        public static PacketWriter Create()
        {
            return Create(null);
        }

        public static PacketWriter Create(byte[] initialBuffer)
        {
            var writer = new PacketWriter();
            writer.bytes = initialBuffer;
            writer.offset += 4; // header space

            return writer;
        }

        public void EnsureCapacity(int appendLength)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, appendLength);
        }

        public void Skip(int length)
        {
            BinaryUtil.EnsureCapacity(ref bytes, offset, length);
            offset += length;
        }

        public void Seek(int offset)
        {
            this.offset = offset;
        }

        public void WriteInt32(int value)
        {
            offset += BinaryUtil.WriteInt32(ref bytes, offset, value);
        }

        public void WriteByte(byte value)
        {
            offset += BinaryUtil.WriteByte(ref bytes, offset, value);
        }

        /// <summary>
        /// does not ensure capacity.
        /// </summary>
        public void WriteByteUnsafe(byte value)
        {
            bytes[offset] = value;
            offset += 1;
        }

        public void WriteBytes(byte[] value)
        {
            offset += BinaryUtil.WriteBytes(ref bytes, offset, value);
        }

        public void WriteString(string value)
        {
            offset += BinaryUtil.WriteString(ref bytes, offset, value);
        }

        public void WriteString(char[] value, int count)
        {
            offset += BinaryUtil.WriteString(ref bytes, offset, value, count);
        }

        public void WriteNullTerminatedString(string value)
        {
            offset += BinaryUtil.WriteNullTerminatedString(ref bytes, offset, value);
        }
    }
}
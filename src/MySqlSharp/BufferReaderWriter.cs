using System;
using System.Collections.Generic;
using System.Text;
using System.Runtime.CompilerServices;
using MySqlSharp.Internal;

namespace MySqlSharp
{
    public struct BufferWriter
    {
        byte[] bytes;
        int offset;

        readonly int startOffset;

        public int WriteSize => offset - startOffset;
        public ArraySegment<byte> GetBuffer() => new ArraySegment<byte>(bytes, startOffset, WriteSize);

        public BufferWriter(byte[] initialBuffer, int offset)
        {
            this.bytes = initialBuffer;
            this.startOffset = this.offset = offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteInt32(int value)
        {
            offset += BinaryUtil.WriteInt32(ref bytes, offset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteByte(byte value)
        {
            offset += BinaryUtil.WriteByte(ref bytes, offset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteBytes(byte[] value)
        {
            offset += BinaryUtil.WriteBytes(ref bytes, offset, value);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void WriteNullTerminatedString(string value)
        {
            offset += BinaryUtil.WriteNullTerminatedString(ref bytes, offset, value);
        }
    }

    public struct BufferReader
    {
        byte[] bytes;
        int offset;

        readonly int limit;
        readonly int startOffset;

        public int ReadSize => offset - startOffset;
        public ArraySegment<byte> GetBuffer() => new ArraySegment<byte>(bytes, startOffset, ReadSize);
        public int Remaining => (startOffset + limit) - offset;

        public BufferReader(byte[] buffer, int offset, int limit)
        {
            this.bytes = buffer;
            this.limit = limit;
            this.startOffset = this.offset = offset;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void ReadNext(int count)
        {
            offset += count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref byte FetchNext()
        {
            return ref bytes[offset];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref byte ReadByte()
        {
            return ref bytes[offset++];
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public byte[] ReadBytes(int count)
        {
            var v = BinaryUtil.ReadBytes(bytes, offset, count);
            offset += count;
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int ReadInt32()
        {
            var v = BinaryUtil.ReadInt32(bytes, offset);
            offset += 4;
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadNullTerminatedString()
        {
            var v = BinaryUtil.ReadNullTerminatedString(bytes, offset, out var readCount);
            offset += readCount;
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt16()
        {
            var r = BinaryUtil.ReadUInt16(bytes, offset);
            offset += 2;
            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint ReadUInt32()
        {
            var r = BinaryUtil.ReadUInt32(bytes, offset);
            offset += 4;
            return r;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public string ReadString(int count)
        {
            var v = BinaryUtil.ReadString(bytes, offset, count);
            offset += count;
            return v;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public uint Read3BytesUInt32()
        {
            var v = BinaryUtil.Read3BytesUInt32(bytes, offset);
            offset += 3;
            return v;
        }
    }
}
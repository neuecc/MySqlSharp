using System;
using MySqlSharp.Internal;

namespace MySqlSharp.Protocol
{
    // be careful, this is "struct".

    public struct PacketReader
    {
        byte[] bytes;
        int offset;
        int sequenceNo;
        int limit;
        int dataLength;
        int binarySegmentCount;

        public int SequenceNo => sequenceNo;
        public int DataLength => dataLength;
        public int ReadSize => offset;
        public int Remaining => limit - offset;

        public static PacketReader Create(byte[] buffer, int offset, int count)
        {
            var reader = new PacketReader();

            // Read PacketHeader
            var length = BinaryUtil.Read3BytesInt32(buffer, offset);
            offset += 3;
            reader.sequenceNo = BinaryUtil.ReadByte(buffer, offset);
            offset += 1;

            reader.bytes = buffer;
            reader.offset = offset;
            reader.dataLength = length;
            reader.limit = offset + length;
            reader.binarySegmentCount = count;

            // TODO:ReadMore

            return reader;
        }

        // TODO:CreateAsync

        public PacketReader CreateChildReader()
        {
            return PacketReader.Create(bytes, limit, binarySegmentCount);
        }

        void Verify(int readCount)
        {
            if (Remaining < readCount)
            {
                throw new Exception("TODO:Read Sugi");
            }
        }

        public void ReadNext(int count)
        {
            Verify(count);
            offset += count;
        }

        public ref byte FetchNext()
        {
            Verify(1);
            return ref bytes[offset];
        }

        public ref byte ReadByte()
        {
            Verify(1);
            return ref bytes[offset++];
        }

        public byte[] ReadBytes(int count)
        {
            Verify(count);
            var v = BinaryUtil.ReadBytes(bytes, offset, count);
            offset += count;
            return v;
        }

        public ArraySegment<byte> ReadByteSegment(int count)
        {
            Verify(count);
            var v = new ArraySegment<byte>(bytes, offset, count);
            offset += count;
            return v;
        }

        public int ReadInt16()
        {
            Verify(2);
            var v = BinaryUtil.ReadInt16(bytes, offset);
            offset += 2;
            return v;
        }

        public int ReadInt32()
        {
            Verify(4);
            var v = BinaryUtil.ReadInt32(bytes, offset);
            offset += 4;
            return v;
        }

        public long ReadInt64()
        {
            Verify(8);
            var v = BinaryUtil.ReadInt64(bytes, offset);
            offset += 8;
            return v;
        }

        public string ReadNullTerminatedString()
        {
            var v = BinaryUtil.ReadNullTerminatedString(bytes, offset, out var readCount);
            Verify(readCount);
            offset += readCount;
            return v;
        }

        public uint ReadUInt16()
        {
            Verify(2);
            var r = BinaryUtil.ReadUInt16(bytes, offset);
            offset += 2;
            return r;
        }

        public uint ReadUInt32()
        {
            Verify(4);
            var r = BinaryUtil.ReadUInt32(bytes, offset);
            offset += 4;
            return r;
        }

        public ulong ReadUInt64()
        {
            Verify(8);
            var r = BinaryUtil.ReadUInt64(bytes, offset);
            offset += 8;
            return r;
        }

        public string ReadString(int count)
        {
            Verify(count);
            var v = BinaryUtil.ReadString(bytes, offset, count);
            offset += count;
            return v;
        }

        public int Read3BytesInt32()
        {
            Verify(3);
            var v = BinaryUtil.Read3BytesInt32(bytes, offset);
            offset += 3;
            return v;
        }

        // TODO:read more spec
        public long ReadLengthEncodedInteger()
        {
            var encodedLength = bytes[offset++];
            switch (encodedLength)
            {
                case 0xFC:
                    return ReadUInt16();
                case 0xFD:
                    return (long)Read3BytesInt32();
                case 0xFE:
                    return ReadInt64();
                case 0xFF:
                    throw new Exception("TODO?");
                default:
                    return encodedLength;
            }
        }

        public string ReadLengthEncodedString()
        {
            var encodedLength = ReadLengthEncodedInteger();
            return ReadString((int)encodedLength);
        }

        // does not decode string avoid allocation
        public ArraySegment<byte> ReadLengthEncodedStringSegment()
        {
            var encodedLength = (int)ReadLengthEncodedInteger();
            var result = new ArraySegment<byte>(bytes, offset, encodedLength);
            offset += encodedLength;
            return result;
        }

        public bool IsOkPacket()
        {
            return bytes[offset] == OkPacket.Code;
        }

        public bool IsErrorPacket()
        {
            return bytes[offset] == ErrorPacket.Code;
        }

        public bool IsEofPacket()
        {
            return bytes[offset] == EofPacket.Code;
        }
    }
}
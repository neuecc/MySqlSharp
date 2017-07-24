using MySqlSharp.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlSharp.Protocol
{
    // [3:data-length][1:sequence no][x:data]
    // if data-length is 0xFFFFFF, require next packet
    public struct PacketHeader
    {
        byte[] RawBytes;
        public uint DataLength;
        public int SequenceNo;
        public int NextOffset;

        public bool IsRequireReadNextPacket => DataLength == 0xFFFFFF;

        public PacketHeader(byte[] bytes, int offset)
        {
            this.RawBytes = bytes;
            this.DataLength = BinaryUtil.Read3BytesUInt32(bytes, offset);
            this.SequenceNo = BinaryUtil.ReadInt32(bytes, offset + 3);
            this.NextOffset = offset + 4;
        }

        // TODO:Concatenate Next Bytes
    }
}
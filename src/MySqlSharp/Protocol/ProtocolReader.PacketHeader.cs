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
        public uint DataLength;
        public int SequenceNo;

        public bool IsRequireReadNextPacket => DataLength == 0xFFFFFF;

        // TODO:Concatenate Next Bytes
    }

    public static partial class ProtocolReader
    {
        public static PacketHeader ReadPacketHeader(byte[] bytes, int offset, out int readSize)
        {
            readSize = 4;
            return new PacketHeader
            {
                DataLength = BinaryUtil.Read3BytesUInt32(bytes, offset),
                SequenceNo = BinaryUtil.ReadInt32(bytes, offset + 3),
            };
        }
    }
}
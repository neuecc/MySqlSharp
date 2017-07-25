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
        public byte SequenceNo;

        public bool IsRequireReadNextPacket => DataLength == 0xFFFFFF;

        // TODO:Concatenate Next Bytes
    }

    public static partial class ProtocolReader
    {
        public static PacketHeader ReadPacketHeader(ref BufferReader reader)
        {
            return new PacketHeader
            {
                DataLength = reader.Read3BytesUInt32(),
                SequenceNo = reader.ReadByte(),
            };
        }
    }
}
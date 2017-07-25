using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlSharp.Protocol
{
    public class ErrorPacket
    {
        public const byte Signature = 0xFF;

        public int ErrorCode { get; private set; }
        public string State { get; private set; }
        public string Message { get; private set; }

        public static ErrorPacket Parse(ref BufferReader reader)
        {
            var header = ProtocolReader.ReadPacketHeader(ref reader);
            if (reader.ReadByte() != Signature) throw new Exception("TODO");

            var packet = new ErrorPacket();

            packet.ErrorCode = (int)reader.ReadUInt16();
            var nextByte = reader.FetchNext();
            if (nextByte == '#')
            {
                reader.ReadNext(1); // #
                packet.State = reader.ReadString(5);
                packet.Message = reader.ReadString(reader.Remaining);
            }
            else
            {
                packet.Message = reader.ReadString(reader.Remaining);
            }

            return packet;
        }
    }

    public static partial class ProtocolReader
    {
        public static ErrorPacket ReadErrorPacket(ref BufferReader reader)
        {
            return ErrorPacket.Parse(ref reader);
        }
    }
}

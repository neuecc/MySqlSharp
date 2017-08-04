using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MySqlSharp.Protocol
{
    // http://imysql.com/mysql-internal-manual/text-protocol.html

    public enum CommandId : byte
    {
        Query = 3,
        Ping = 14,
        PrepareStatement = 16,
    }

    public static partial class ProtocolWriter
    {
        public static void WritePing(ref PacketWriter writer)
        {
            writer.WriteByte((byte)CommandId.Ping);
        }

        public static void WriteQuery(ref PacketWriter writer, string queryText)
        {
            writer.WriteByte((byte)CommandId.Query);
            writer.WriteString(queryText);
        }

        public static void WritePrepare(ref PacketWriter writer, string queryText)
        {
            writer.WriteByte((byte)CommandId.PrepareStatement);
            writer.WriteString(queryText);
        }
    }
}

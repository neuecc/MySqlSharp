using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace MySqlSharp.Protocol
{
    // http://imysql.com/mysql-internal-manual/text-protocol.html

    public enum CommandId : byte
    {
        Quit = 1,
        Query = 3,
        Statistics = 9,
        Ping = 14,
        PrepareStatement = 22,
        ExecuteStatement = 23,
    }

    [Flags]
    public enum ExecuteStatementFlags : byte
    {
        NoCursor = 0,
        ReadOnly = 1,
        CursorForUpdate = 2,
        ScrollableCursor = 4
    }

    public static partial class ProtocolWriter
    {
        // http://imysql.com/mysql-internal-manual/com-quit.html
        /// <summary>COM_QUIT, tells the server that the client wants to close the connection.</summary>
        public static void WriteQuit(ref PacketWriter writer)
        {
            writer.WriteByte((byte)CommandId.Quit);
        }

        // http://imysql.com/mysql-internal-manual/com-ping.html
        /// <summary>COM_PING, check if the server is alive.</summary>
        public static void WritePing(ref PacketWriter writer)
        {
            writer.WriteByte((byte)CommandId.Ping);
        }

        // http://imysql.com/mysql-internal-manual/com-statistics.html
        /// <summary>COM_STATISTICS, Get a human readable string of internal statistics.</summary>
        public static void WriteStatistics(ref PacketWriter writer)
        {
            writer.WriteByte((byte)CommandId.Statistics);
        }

        // http://imysql.com/mysql-internal-manual/com-query.html
        public static void WriteQuery(ref PacketWriter writer, string queryText)
        {
            writer.WriteByte((byte)CommandId.Query);
            writer.WriteString(queryText);
        }

        public static void WriteQuery(ref PacketWriter writer, char[] queryText, int count)
        {
            writer.WriteByte((byte)CommandId.Query);
            writer.WriteString(queryText, count);
        }

        //http://imysql.com/mysql-internal-manual/com-stmt-prepare.html
        public static void WritePrepareStatement(ref PacketWriter writer, string queryText)
        {
            writer.WriteByte((byte)CommandId.PrepareStatement);
            writer.WriteString(queryText);
        }

        // http://imysql.com/mysql-internal-manual/com-stmt-execute.html
        public static void WriteExecuteStatement(ref PacketWriter writer, int statementId, object[] parameters) // Can I avoid boxing? array allocation?
        {
            /*
                1              [17] COM_STMT_EXECUTE
                4              stmt-id
                1              flags
                4              iteration-count
                if num-params > 0:
                n              NULL-bitmap, length: (num-params+7)/8
                1              new-params-bound-flag
                    if new-params-bound-flag == 1:
                    n              type of each parameter, length: num-params * 2
                    n              value of each parameter
            */

            writer.WriteByte((byte)CommandId.ExecuteStatement);
            writer.WriteInt32(statementId);

            writer.WriteByte((byte)ExecuteStatementFlags.ReadOnly); // flags
            writer.WriteInt32(1); // The iteration-count is always 1


            if (parameters.Length > 0)
            {
                var nullMap = new BitArraySlim(parameters.Length);

                var nullMapLength = (parameters.Length + 7) / 8;
                var nullBitMapOffset = writer.CurrentOffset; // save and after use
                writer.Skip(nullMapLength);

                for (int i = 0; i < parameters.Length; i++)
                {
                    if (parameters[i] == null)
                    {
                        nullMap[i] = true;
                        continue;
                    }

                    nullMap[i] = false;

                    // TODO:write parameter type

                    MySqlParameterWriter.Write(ref writer, parameters[i]);
                }

                var offset = writer.CurrentOffset;
                writer.Seek(nullBitMapOffset); // ready for null-map position
                nullMap.WriteTo(ref writer);

                writer.Seek(offset); // return to end
            }
        }
    }
}

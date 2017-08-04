using MySqlSharp.Data;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlSharp.Protocol
{
    // OK_Packet, ERR_Packet, EOF_Packet
    // https://dev.mysql.com/doc/dev/mysql-server/8.0.0/page_protocol_basic_response_packets.html

    public enum ResponsePacketKind
    {
        OK, Error, EOF
    }

    public abstract class ResponsePacket
    {
        public abstract ResponsePacketKind PacketKind { get; }

        public static ResponsePacket Parse(ref PacketReader reader)
        {
            ref var code = ref reader.FetchNext();

            if (code == ErrorPacket.Code)
            {
                return ErrorPacket.Parse(ref reader);
            }

            // These rules distinguish whether the packet represents OK or EOF:
            // OK: header = 0 and length of packet > 7
            // EOF: header = 0xfe and length of packet< 9
            if (code == EofPacket.Code && reader.DataLength < 9)
            {
                return EofPacket.Parse(ref reader);
            }
            else if (code == OkPacket.Code/* && header.DataLength > 7*/)
            {
                return OkPacket.Parse(ref reader);
            }

            throw new NotImplementedException("TODO");
        }

        public void ThrowIfError()
        {
            if (PacketKind == ResponsePacketKind.Error)
            {
                var error = this as ErrorPacket;
                throw error.ToMySqlException();
            }
        }
    }

    // https://dev.mysql.com/doc/dev/mysql-server/8.0.0/page_protocol_basic_ok_packet.html
    public class OkPacket : ResponsePacket
    {
        public const byte Code = 0x00;

        public override ResponsePacketKind PacketKind => ResponsePacketKind.OK;

        public int AffectedRows { get; private set; }
        public long LastInsertId { get; private set; }
        public ServerStatus StatusFlags { get; private set; }
        public int Warnings { get; private set; }
        public string Info { get; private set; }
        public string SessionStateInfo { get; private set; }

        public static new OkPacket Parse(ref PacketReader reader)
        {
            var packet = new OkPacket();

            reader.ReadByte(); // code
            packet.AffectedRows = (int)reader.ReadLengthEncodedInteger();
            packet.LastInsertId = (long)reader.ReadLengthEncodedInteger();
            packet.StatusFlags = (ServerStatus)reader.ReadUInt16();
            packet.Warnings = (int)reader.ReadUInt16();

            // TODO:read info?

            if ((packet.StatusFlags & ServerStatus.SessionStateChanged) != 0)
            {
                // TODO:read session state changed...
            }

            return packet;
        }
    }

    // https://dev.mysql.com/doc/dev/mysql-server/8.0.0/page_protocol_basic_err_packet.html
    public class ErrorPacket : ResponsePacket
    {
        public const byte Code = 0xFF;

        public override ResponsePacketKind PacketKind => ResponsePacketKind.Error;

        public int ErrorCode { get; private set; }

        /// <summary>SQL state</summary>
        public string State { get; private set; }

        /// <summary>human readable error message</summary>
        public string Message { get; private set; }

        public static new ErrorPacket Parse(ref PacketReader reader)
        {
            var packet = new ErrorPacket();

            reader.ReadByte(); // code
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

        public MySqlException ToMySqlException()
        {
            return new MySqlException($"ErrorCode:{ErrorCode} State:{State} Message:{Message}");
        }
    }

    // https://dev.mysql.com/doc/dev/mysql-server/8.0.0/page_protocol_basic_eof_packet.html
    public class EofPacket : ResponsePacket
    {
        public const byte Code = 0xFE;

        public override ResponsePacketKind PacketKind => ResponsePacketKind.EOF;

        public static new EofPacket Parse(ref PacketReader reader)
        {
            var packet = new EofPacket();

            reader.ReadByte(); // code

            // TODO...
            var warning = reader.ReadUInt16();
            var statusFlags = reader.ReadUInt16();

            return packet;
        }
    }

    public class ResultSet
    {
        public int ColumnCount { get; private set; }

        public static ResultSet Parse(ref PacketReader reader)
        {
            var resultSet = new ResultSet();

            resultSet.ColumnCount = (int)reader.ReadLengthEncodedInteger();

            // column definition
            PacketReader columnReader = reader; // struct copy
            for (int i = 0; i < resultSet.ColumnCount; i++)
            {
                columnReader = columnReader.CreateChildReader();

                var todotodo = ColumnDefinition41.Parse(ref columnReader);
            }

            return resultSet;
        }
    }

    // https://web.archive.org/web/20160604100755/http://dev.mysql.com/doc/internals/en/com-query-response.html#packet-Protocol::ColumnDefinition41
    //lenenc_str catalog
    //lenenc_str schema
    //lenenc_str table
    //lenenc_str org_table
    //lenenc_str name
    //lenenc_str org_name
    //lenenc_int length of fixed-length fields[0c]
    //2              character set
    //4              column length
    //1              type
    //2              flags
    //1              decimals
    //2              filler[00][00]
    //if command was COM_FIELD_LIST {
    //    lenenc_int     length of default- values
    //    string[$len]   default values
    //}

    public class ColumnDefinition41
    {
        ArraySegment<byte> catalog;
        ArraySegment<byte> schema;
        ArraySegment<byte> table;
        ArraySegment<byte> originalTable;
        ArraySegment<byte> column;
        ArraySegment<byte> originalColumn;

        public string Catalog => Encoding.UTF8.GetString(catalog.Array, catalog.Offset, catalog.Count);
        public string Schema => Encoding.UTF8.GetString(schema.Array, schema.Offset, schema.Count);
        public string Table => Encoding.UTF8.GetString(table.Array, table.Offset, table.Count);
        public string OriginalTable => Encoding.UTF8.GetString(originalTable.Array, originalTable.Offset, originalTable.Count);
        public string Column => Encoding.UTF8.GetString(column.Array, column.Offset, column.Count);
        public string OriginalColumn => Encoding.UTF8.GetString(originalColumn.Array, originalColumn.Offset, originalColumn.Count);

        // more, more...

        public static ColumnDefinition41 Parse(ref PacketReader reader)
        {
            var def = new ColumnDefinition41();

            // TODO:Array lifetime?
            def.catalog = reader.ReadLengthEncodedStringSegment(); // always "def"
            def.schema = reader.ReadLengthEncodedStringSegment();
            def.table = reader.ReadLengthEncodedStringSegment();
            def.originalTable = reader.ReadLengthEncodedStringSegment();
            def.column = reader.ReadLengthEncodedStringSegment();
            def.originalColumn = reader.ReadLengthEncodedStringSegment();

            reader.ReadByte(); // 0x

            var cset = (CharacterSet)(byte)reader.ReadUInt16();
            var columnLen = reader.ReadInt32();
            var cType = reader.ReadByte();
            var cFlag = reader.ReadUInt16();
            var decimals = reader.ReadByte(); // 0x00 for intergers and static string, 0x1f for dynamic strings, double, float, 0x00 to 0x51 for decimals

            reader.ReadNext(2); // filter

            // TODO:and others

            return def;
        }

        public override string ToString()
        {
            // TODO:Type
            return Column;
        }
    }


    public static partial class ProtocolReader
    {
        public static ResponsePacket ReadResponsePacket(ref PacketReader reader)
        {
            return ResponsePacket.Parse(ref reader);
        }
    }

    // TODO:should remove this.
    [Flags]
    public enum ServerStatus : ushort
    {
        /// <summary>
        /// A transaction is active.
        /// </summary>
        InTransaction = 1,

        /// <summary>
        /// Auto-commit is enabled
        /// </summary>
        AutoCommit = 2,

        MoreResultsExist = 8,

        NoGoodIndexUsed = 0x10,

        NoIndexUsed = 0x20,

        /// <summary>
        /// Used by Binary Protocol Resultset to signal that COM_STMT_FETCH must be used to fetch the row-data.
        /// </summary>
        CursorExists = 0x40,

        LastRowSent = 0x80,

        DatabaseDropped = 0x100,

        NoBackslashEscapes = 0x200,

        MetadataChanged = 0x400,

        QueryWasSlow = 0x800,

        PsOutParams = 0x1000,

        /// <summary>
        /// In a read-only transaction.
        /// </summary>
        InReadOnlyTransaction = 0x2000,

        /// <summary>
        /// Connection state information has changed.
        /// </summary>
        SessionStateChanged = 0x4000,
    }
}
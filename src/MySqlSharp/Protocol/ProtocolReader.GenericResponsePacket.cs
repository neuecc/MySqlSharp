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

        public ushort ErrorCode { get; private set; }

        /// <summary>SQL state</summary>
        public string State { get; private set; }

        /// <summary>human readable error message</summary>
        public string Message { get; private set; }

        public static new ErrorPacket Parse(ref PacketReader reader)
        {
            var packet = new ErrorPacket();

            reader.ReadByte(); // code
            packet.ErrorCode = reader.ReadUInt16();
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
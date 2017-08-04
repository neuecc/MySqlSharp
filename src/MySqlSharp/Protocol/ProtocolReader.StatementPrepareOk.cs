using MySqlSharp.Internal;
using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlSharp.Protocol
{
    // https://mariadb.com/kb/en/mariadb/com_stmt_prepare/

    /*

int<1> 0x00 COM_STMT_PREPARE_OK header
int<4> statement id
int<2> number of columns
int<2> number of parameters
string<1> -not used-
int<2> number of warnings

 */

    public class StatementPrepareOk
    {
        public const byte Code = 0x00;

        public int StatementId { get; private set; }
        public int ColumnsCount { get; private set; }
        public int ParametersCount { get; private set; }
        public int WarningsCount { get; private set; }


        public static StatementPrepareOk Parse(ref PacketReader reader)
        {
            var prepareOk = new StatementPrepareOk();

            if (reader.ReadByte() != Code) throw new Exception("TODO");

            prepareOk.StatementId = reader.ReadInt16();
            prepareOk.ColumnsCount = reader.ReadInt16();

            var notused = reader.ReadLengthEncodedStringSegment();

            prepareOk.WarningsCount = reader.ReadInt16();

            return prepareOk;
        }
    }

    public static partial class ProtocolReader
    {
        public static StatementPrepareOk ReadStatementPrepareOk(ref PacketReader reader)
        {
            return StatementPrepareOk.Parse(ref reader);
        }
    }
}
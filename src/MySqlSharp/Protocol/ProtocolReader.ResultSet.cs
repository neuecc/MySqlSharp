using MySqlSharp.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MySqlSharp.Protocol
{
    public enum ColumnType
    {
        Decimal = 0,
        Byte = 1,
        Int16 = 2,
        Int24 = 9,
        Int32 = 3,
        Int64 = 8,
        Float = 4,
        Double = 5,
        Timestamp = 7,
        Date = 10,
        Time = 11,
        DateTime = 12,
        Year = 13,
        Newdate = 14,
        VarString = 15,
        Bit = 16,
        JSON = 245,
        NewDecimal = 246,
        Enum = 247,
        Set = 248,
        TinyBlob = 249,
        MediumBlob = 250,
        LongBlob = 251,
        Blob = 252,
        VarChar = 253,
        String = 254,
        Geometry = 255,
        UByte = 501,
        UInt16 = 502,
        UInt24 = 509,
        UInt32 = 503,
        UInt64 = 508,
        Binary = 600,
        VarBinary = 601,
        TinyText = 749,
        MediumText = 750,
        LongText = 751,
        Text = 752,
        Guid = 800
    };

    [Flags]
    public enum ColumnFlags
    {
        NotNull = 1,
        PrimaryKey = 2,
        UniqueKey = 4,
        MultipleKey = 8,
        Blob = 16,
        Unsigned = 32,
        ZeroFill = 64,
        Binary = 128,
        Enum = 256,
        AutoIncrement = 512,
        Timestamp = 1024,
        Set = 2048,
        Number = 32768
    };

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
        internal ArraySegment<byte> catalog;
        internal ArraySegment<byte> schema;
        internal ArraySegment<byte> table;
        internal ArraySegment<byte> originalTable;
        internal ArraySegment<byte> column;
        internal ArraySegment<byte> originalColumn;

        public string Catalog => Encoding.UTF8.GetString(catalog.Array, catalog.Offset, catalog.Count);
        public string Schema => Encoding.UTF8.GetString(schema.Array, schema.Offset, schema.Count);
        public string Table => Encoding.UTF8.GetString(table.Array, table.Offset, table.Count);
        public string OriginalTable => Encoding.UTF8.GetString(originalTable.Array, originalTable.Offset, originalTable.Count);
        public string Column => Encoding.UTF8.GetString(column.Array, column.Offset, column.Count);
        public string OriginalColumn => Encoding.UTF8.GetString(originalColumn.Array, originalColumn.Offset, originalColumn.Count);

        public CharacterSet CharacterSet { get; private set; }
        public int ColumnLength { get; private set; }
        public ColumnType ColumnType { get; private set; }
        public ColumnFlags ColumnFlags { get; private set; }

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

            def.CharacterSet = (CharacterSet)(byte)reader.ReadUInt16();
            def.ColumnLength = reader.ReadInt32();
            def.ColumnType = (ColumnType)reader.ReadByte();
            def.ColumnFlags = (ColumnFlags)reader.ReadUInt16();
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

    // https://web.archive.org/web/20170404085826/https://dev.mysql.com/doc/internals/en/com-query-response.html#packet-ProtocolText::Resultset

    public abstract class ResultSet
    {
        public int ColumnCount { get; private set; }
        public ColumnDefinition41[] ColumnDefinitions { get; private set; }

        protected PacketReader rowReader;
        protected ArraySegment<byte>[] rowData;

        public static ResultSet Parse(ref PacketReader reader, bool isTextResultSet)
        {
            var resultSet = (isTextResultSet)
                ? (ResultSet)new TextResultSet()
                : (ResultSet)new BinaryResultSet();

            resultSet.ColumnCount = (int)reader.ReadLengthEncodedInteger();
            resultSet.ColumnDefinitions = new ColumnDefinition41[resultSet.ColumnCount];

            PacketReader columnReader = reader; // struct copy
            for (int i = 0; i < resultSet.ColumnDefinitions.Length; i++)
            {
                columnReader = columnReader.CreateNextReader();

                var column = ColumnDefinition41.Parse(ref columnReader);
                resultSet.ColumnDefinitions[i] = column;
            }
            resultSet.rowData = new ArraySegment<byte>[resultSet.ColumnCount];

            // if the CLIENT_DEPRECATE_EOF EOF_PACKET
            // RESULTSETROW


            var lastReader = columnReader.CreateNextReader();
            var eof = ProtocolReader.ReadResponsePacket(ref lastReader);

            resultSet.rowReader = lastReader;

            return resultSet;
        }

        // TODO:CreateAsync

        // Move

        public abstract bool Read();
        // TODO:ReadAsync

        public abstract int GetInt32(int ordinal);
        public abstract string GetString(int ordinal);

        public abstract object GetValue(int ordinal);

        public IEnumerable<KeyValuePair<ColumnDefinition41, object>[]> ToEnumerable()
        {
            while (Read())
            {
                var array = new KeyValuePair<ColumnDefinition41, object>[ColumnDefinitions.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    array[i] = new KeyValuePair<ColumnDefinition41, object>(ColumnDefinitions[i], GetValue(i));
                }

                yield return array;
            }
        }
    }

    public sealed class TextResultSet : ResultSet
    {
        public override bool Read()
        {
            rowReader = rowReader.CreateNextReader();
            if (rowReader.IsEofPacket())
            {
                return false;
            }
            else if (rowReader.IsErrorPacket())
            {
                throw ErrorPacket.Parse(ref rowReader).ToMySqlException();
            }

            for (int i = 0; i < rowData.Length; i++)
            {
                rowData[i] = rowReader.ReadLengthEncodedStringSegment();
            }

            return true;
        }

        public override int GetInt32(int ordinal)
        {
            // TODO:Verify can read.
            ref var segment = ref rowData[ordinal];
            return NumberConverter.ToInt32(segment.Array, segment.Offset, segment.Count);
        }

        public override string GetString(int ordinal)
        {
            // TODO:Verify can read.
            ref var segment = ref rowData[ordinal];
            return Encoding.UTF8.GetString(segment.Array, segment.Offset, segment.Count);
        }


        public override object GetValue(int ordinal)
        {
            throw new NotImplementedException();
        }


        public void ReadToEnd()
        {
            while (Read()) { }
        }
    }

    public sealed class BinaryResultSet : ResultSet
    {
        public override bool Read()
        {
            rowReader = rowReader.CreateNextReader();
            if (rowReader.IsEofPacket())
            {
                return false;
            }
            else if (rowReader.IsErrorPacket())
            {
                throw ErrorPacket.Parse(ref rowReader).ToMySqlException();
            }

            var a = rowReader.ReadByte(); // 0x00
            if (a != 0) throw new Exception("Invalid Packet");

            var b = rowReader.ReadByteSegment((ColumnCount + 9) / 8); // TODO:create null-bitmap

            for (int i = 0; i < rowData.Length; i++)
            {
                switch (ColumnDefinitions[i].ColumnType)
                {
                    case ColumnType.Decimal: // TODO?
                        break;
                    case ColumnType.Byte:
                        break;
                    case ColumnType.Int16:
                        break;
                    case ColumnType.Int24:
                        break;
                    case ColumnType.Int32:
                        rowData[i] = rowReader.ReadByteSegment(4);
                        break;
                    case ColumnType.Int64:
                        rowData[i] = rowReader.ReadByteSegment(8);
                        break;
                    case ColumnType.Float:
                        break;
                    case ColumnType.Double:
                        break;
                    case ColumnType.Timestamp:
                        break;
                    case ColumnType.Date:
                        break;
                    case ColumnType.Time:
                        break;
                    case ColumnType.DateTime:
                        break;
                    case ColumnType.Year:
                        break;
                    case ColumnType.Newdate:
                        break;
                    case ColumnType.VarString:
                        break;
                    case ColumnType.Bit:
                        break;
                    case ColumnType.JSON:
                        break;
                    case ColumnType.NewDecimal:
                        break;
                    case ColumnType.Enum:
                        break;
                    case ColumnType.Set:
                        break;
                    case ColumnType.TinyBlob:
                        break;
                    case ColumnType.MediumBlob:
                        break;
                    case ColumnType.LongBlob:
                        break;
                    case ColumnType.Blob:
                        break;
                    case ColumnType.Geometry:
                        break;
                    case ColumnType.UByte:
                        break;
                    case ColumnType.UInt16:
                        break;
                    case ColumnType.UInt24:
                        break;
                    case ColumnType.UInt32:
                        break;
                    case ColumnType.UInt64:
                        break;
                    case ColumnType.Binary:
                        break;
                    case ColumnType.VarBinary:
                        break;
                    default:
                        rowData[i] = rowReader.ReadLengthEncodedStringSegment();
                        break;
                }


            }

            return true;
        }

        public override int GetInt32(int ordinal)
        {
            // TODO:Verify can read.
            ref var segment = ref rowData[ordinal];
            return BinaryUtil.ReadInt32(segment.Array, segment.Offset);
        }

        public override string GetString(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override object GetValue(int ordinal)
        {
            throw new NotImplementedException();
        }
    }

    public static partial class ProtocolReader
    {
        public static TextResultSet ReadTextResultSet(ref PacketReader reader)
        {
            return ResultSet.Parse(ref reader, true) as TextResultSet;
        }

        public static BinaryResultSet ReadBinaryResultSet(ref PacketReader reader)
        {
            return ResultSet.Parse(ref reader, false) as BinaryResultSet;
        }
    }
}

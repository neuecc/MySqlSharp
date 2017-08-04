using MySqlSharp.Internal;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Collections;

namespace MySqlSharp.Protocol
{
    // result set is like cursor, IEnumerable<Row> but designed for performance.

    // https://web.archive.org/web/20170404085826/https://dev.mysql.com/doc/internals/en/com-query-response.html#packet-ProtocolText::Resultset

    // TextResultSet + ColumnDefinition41 + Row

    public sealed class TextResultSet
    {
        public int ColumnCount { get; private set; }
        public ColumnDefinition41[] ColumnDefinitions { get; private set; }

        PacketReader rowReader;
        ArraySegment<byte>[] rowData;

        public static TextResultSet Parse(ref PacketReader reader)
        {
            var resultSet = new TextResultSet();

            resultSet.ColumnCount = (int)reader.ReadLengthEncodedInteger();
            resultSet.ColumnDefinitions = new ColumnDefinition41[resultSet.ColumnCount];

            PacketReader columnReader = reader; // struct copy
            for (int i = 0; i < resultSet.ColumnDefinitions.Length; i++)
            {
                columnReader = columnReader.CreateChildReader();

                var column = ColumnDefinition41.Parse(ref columnReader);
                resultSet.ColumnDefinitions[i] = column;
            }
            resultSet.rowData = new ArraySegment<byte>[resultSet.ColumnCount];

            // if the CLIENT_DEPRECATE_EOF EOF_PACKET
            // RESULTSETROW


            var lastReader = columnReader.CreateChildReader();
            var eof = ProtocolReader.ReadResponsePacket(ref lastReader);

            resultSet.rowReader = lastReader;

            return resultSet;
        }

        // TODO:CreateAsync

        // Move

        public bool MoveNext()
        {
            rowReader = rowReader.CreateChildReader();
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

        ValueTask<bool> MoveNextAsync()
        {
            // rowReader = await rowReader.CreateChildReaderAsync();

            throw new NotImplementedException();
        }

        public int GetInt32(int ordinal)
        {
            // TODO:Verify can read.

            ref var segment = ref rowData[ordinal];
            return NumberConverter.ToInt32(segment.Array, segment.Offset, segment.Count);
        }

        public string GetString(int ordinal)
        {
            // TODO:Verify can read.
            ref var segment = ref rowData[ordinal];

            return Encoding.UTF8.GetString(segment.Array, segment.Offset, segment.Count);
        }

        // boxed(not performant)
        public object GetValue(int ordinal)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<ColumnDefinition41, object>[]> ToEnumerable()
        {
            while (MoveNext())
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

    public static partial class ProtocolReader
    {
        public static TextResultSet ReadTextResultSet(ref PacketReader reader)
        {
            return TextResultSet.Parse(ref reader);
        }
    }
}

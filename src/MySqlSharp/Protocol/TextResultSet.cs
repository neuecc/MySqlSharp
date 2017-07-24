using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace MySqlSharp.Protocol
{
    // result set is like cursor, IEnumerable<Row> but designed for performance.

    // https://web.archive.org/web/20170404085826/https://dev.mysql.com/doc/internals/en/com-query-response.html#packet-ProtocolText::Resultset

    public sealed class TextResultSet
    {
        // public ColumnDefinition{get;}
        // dataoffsets?...?

        // Init(Read Header)

        // Move

        bool MoveNext()
        {
            throw new NotImplementedException();
        }

        ValueTask<bool> MoveNextAsync()
        {
            throw new NotImplementedException();
        }

        // Read Row

        public int GetInt32()
        {
            return 0;
        }

        public TextResultSet()
        {
            var resultSet = this;
            while (resultSet.MoveNext())
            {
                // resultSet.GetInt32();
                // resultSet.GetString(1);
                // ....
            }
        }
    }


}

using MySqlSharp.Protocol;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace MySqlSharp.Data
{
    public class MySqlDataReader : DbDataReader
    {
        readonly TextResultSet resultSet;
        Dictionary<string, int> __ordinalLookup;

        // TODO:test
        public MySqlDataReader(TextResultSet resultSet)
        {
            this.resultSet = resultSet;
        }

        Dictionary<string, int> OrdinalLookup
        {
            get
            {
                if (__ordinalLookup == null)
                {
                    __ordinalLookup = new Dictionary<string, int>(resultSet.ColumnDefinitions.Length);

                    for (int i = 0; i < resultSet.ColumnDefinitions.Length; i++)
                    {
                        __ordinalLookup.Add(resultSet.ColumnDefinitions[i].Column, i);
                    }
                }

                return __ordinalLookup;
            }
        }


        public override object this[int ordinal] => resultSet.GetValue(ordinal);

        public override object this[string name] => resultSet.GetValue(GetOrdinal(name));

        public override int Depth => throw new NotSupportedException();

        public override int FieldCount => resultSet.ColumnCount;

        public override bool HasRows => throw new NotImplementedException();

        public override bool IsClosed => throw new NotImplementedException();

        public override int RecordsAffected => throw new NotImplementedException();

        public override bool GetBoolean(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override byte GetByte(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetBytes(int ordinal, long dataOffset, byte[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override char GetChar(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetChars(int ordinal, long dataOffset, char[] buffer, int bufferOffset, int length)
        {
            throw new NotImplementedException();
        }

        public override string GetDataTypeName(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override DateTime GetDateTime(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override decimal GetDecimal(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override double GetDouble(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override IEnumerator GetEnumerator()
        {
            while (resultSet.Read())
            {
                var xs = new object[resultSet.ColumnCount];
                for (int i = 0; i < xs.Length; i++)
                {
                    xs[i] = resultSet.GetValue(i);
                }
                yield return xs;
            }
        }

        public override Type GetFieldType(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override float GetFloat(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override Guid GetGuid(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override short GetInt16(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override int GetInt32(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override long GetInt64(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override string GetName(int ordinal)
        {
            return resultSet.ColumnDefinitions[ordinal].Column;
        }

        public override int GetOrdinal(string name)
        {
            return OrdinalLookup[name];
        }

        public override string GetString(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override object GetValue(int ordinal)
        {
            return resultSet.GetValue(ordinal);
        }

        public override int GetValues(object[] values)
        {
            throw new NotImplementedException();
        }

        public override bool IsDBNull(int ordinal)
        {
            throw new NotImplementedException();
        }

        public override bool NextResult()
        {
            throw new NotImplementedException();
        }

        public override bool Read()
        {
            return resultSet.Read();
        }
    }
}

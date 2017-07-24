using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace MySqlSharp.Data
{
    public class MySqlCommand : DbCommand
    {
        public override string CommandText { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override int CommandTimeout { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override CommandType CommandType { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override bool DesignTimeVisible { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public override UpdateRowSource UpdatedRowSource { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        protected override DbConnection DbConnection { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        protected override DbParameterCollection DbParameterCollection => throw new NotImplementedException();

        protected override DbTransaction DbTransaction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

        public override int ExecuteNonQuery()
        {
            throw new NotImplementedException();
        }

        public override object ExecuteScalar()
        {
            throw new NotImplementedException();
        }

        public override void Prepare()
        {
            throw new NotImplementedException();
        }

        protected override DbParameter CreateDbParameter()
        {
            throw new NotImplementedException();
        }

        protected override DbDataReader ExecuteDbDataReader(CommandBehavior behavior)
        {
            throw new NotImplementedException();
        }
    }
}

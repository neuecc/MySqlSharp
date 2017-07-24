using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace MySqlSharp.Data
{
    public class MySqlTransaction : DbTransaction
    {
        public override IsolationLevel IsolationLevel => throw new NotImplementedException();

        protected override DbConnection DbConnection => throw new NotImplementedException();

        public override void Commit()
        {
            throw new NotImplementedException();
        }

        public override void Rollback()
        {
            throw new NotImplementedException();
        }
    }
}

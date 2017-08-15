using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Text;

namespace MySqlSharp.Data
{
    public class MySqlTransaction : DbTransaction
    {
        readonly MySqlConnection conn;
        readonly TransactionController controller;

        public override IsolationLevel IsolationLevel => throw new NotImplementedException();

        protected override DbConnection DbConnection => throw new NotImplementedException();

        public MySqlTransaction(MySqlConnection conn, TransactionController controller)
        {
            //TODO:...
        }

        public override void Commit()
        {
            controller.Commit();
        }

        public override void Rollback()
        {
            controller.Rollback();
        }

        protected override void Dispose(bool disposing)
        {
            controller.Dispose();
        }
    }
}

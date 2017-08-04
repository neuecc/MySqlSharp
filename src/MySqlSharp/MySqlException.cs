using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace MySqlSharp
{
    // TODO:MySqlDriverException?
    public class MySqlException : DbException
    {
        public MySqlException()
        {
        }

        public MySqlException(string message) : base(message)
        {
        }

        public MySqlException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

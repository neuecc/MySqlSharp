using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Text;

namespace MySqlSharp.Data
{
    public class MySqlException : DbException
    {
        protected MySqlException()
        {
        }

        protected MySqlException(string message) : base(message)
        {
        }

        protected MySqlException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}

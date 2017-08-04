using System;
using System.Collections.Generic;
using System.Text;

namespace MySqlSharp
{
    // https://dev.mysql.com/doc/connector-net/en/connector-net-connection-options.html

    public class MySqlConnectionOptions
    {
        // General Options

        public string UserId { get; set; }
        public string Server { get; set; }
        public int Port { get; set; }
        public string Password { get; set; }

        public string Database { get; set; }

        // Connection Pooling Options

        // Additional for MySqlSharp

        public MySqlConnectionOptions()
        {
            UserId = "";
            Server = "localhost";
            Port = 3306;
        }
    }
}

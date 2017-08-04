using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp
{
    class Program
    {
        static void Main(string[] args)
        {
            var option = new MySqlSharp.MySqlConnectionOptions
            {
                Server = "",
                Database = "",
                UserId = "",
                Password = "",
            };

            var driver = new MySqlSharp.MySqlDriver(option);

            driver.ConnectAsync().Wait();


            var set = driver.Query("select version() as v, 199 as num union select version(), 399");

            while (set.MoveNext())
            {
                // set.

                var s = set.GetString(0);
                var i = set.GetInt32(1);
            }
        }
    }
}

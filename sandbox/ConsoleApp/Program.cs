using MySqlSharp;
using MySqlSharp.Mapper;
using MySqlSharp.Data;
using MySqlSharp.Protocol;
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
            



            var driver = new MySqlDriver(option);
            driver.Open();

            var reader = driver.Query<long>($"select 1543535353 as aaa, 1234213, 21313");


            
            


            //var nextReader = reader.CreateNextReader();
            //nextReader.rea





            ////var prepare = driver.Prepare("select version() as v, 199 as num union select version(), 399");
            //var prepare = driver.Prepare("select 1");
            //var reader = driver.Execute(prepare.StatementId);


            //while (reader.Read())
            //{
            //    //var a = reader.GetString(0);
            //    var b = reader.GetInt32(0);
            //}




            /*
        var reader = driver.Query("select version() as v, 199 as num union select version(), 399");

        while (reader.MoveNext())
        {
            // set.

            var s = reader.GetString(0);
            var i = reader.GetInt32(1);
        }
        */
        }
    }
}

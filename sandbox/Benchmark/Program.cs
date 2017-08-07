extern alias asyncmysql;

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Diagnosers;
using BenchmarkDotNet.Exporters;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Running;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark
{
    class Program
    {
        static void Main(string[] args)
        {
            var switcher = new BenchmarkSwitcher(new[]
            {
                typeof(SimpleSqlBenchmark)
            });

            args = new string[] { "0" };
#if !DEBUG
            switcher.Run(args);
#else
            var b = new SimpleSqlBenchmark();
            b.Setup();
            b.MySqlSharp();
            b.Cleanup();
#endif
        }
    }

    public class BenchmarkConfig : ManualConfig
    {
        public BenchmarkConfig()
        {
            Add(MarkdownExporter.GitHub);
            Add(MemoryDiagnoser.Default);

            Add(Job.ShortRun.WithTargetCount(1).WithWarmupCount(1));
        }
    }

    [Config(typeof(BenchmarkConfig))]
    public class SimpleSqlBenchmark
    {
        MySqlSharp.MySqlDriver driver;
        MySqlConnection conn;
        asyncmysql::MySql.Data.MySqlClient.MySqlConnection conn2;

        [GlobalSetup]
        public void Setup()
        {
            var option = new MySqlSharp.MySqlConnectionOptions
            {
                Server = "",
                Database = "User",
                UserId = "",
                Password = "",
            };

            driver = new MySqlSharp.MySqlDriver(option);
            driver.ConnectAsync().Wait();

            var connstr1 = ";";
            var connstr2 = ";";

            conn = new MySqlConnection(connstr1);
            conn.Open();

            conn2 = new asyncmysql::MySql.Data.MySqlClient.MySqlConnection(connstr1);
            conn2.Open();

        }

        [GlobalCleanup]
        public void Cleanup()
        {
            driver.Dispose();
            conn.Dispose();
        }

        [Benchmark]
        public void MySqlSharp()
        {
            var set = driver.Query("select Id, MasterId, TItle, MemberNum, GuildRank, MaxGuildRank from Guild where GuildRank = 0 limit 100");
            while (set.Read())
            {
                var a = set.GetInt32(0);
                var b = set.GetInt32(1);
                var c = set.GetInt32(2);
                var d = set.GetInt32(3);
                var e = set.GetInt32(4);
                var f = set.GetInt32(5);
            }
        }

        [Benchmark]
        public void MySqlConnector()
        {
            using (var cmd = conn.CreateCommand())
            {
                cmd.CommandText = "select Id, MasterId, TItle, MemberNum, GuildRank, MaxGuildRank from Guild where GuildRank = 0 limit 100";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var a = reader.GetInt32(0);
                        var b = reader.GetInt32(1);
                        var c = reader.GetInt32(2);
                        var d = reader.GetInt32(3);
                        var e = reader.GetInt32(4);
                        var f = reader.GetInt32(5);
                    }
                }
            }
        }

        [Benchmark]
        public void AsyncMySqlConnector()
        {
            using (var cmd = conn2.CreateCommand())
            {
                cmd.CommandText = "select Id, MasterId, TItle, MemberNum, GuildRank, MaxGuildRank from Guild where GuildRank = 0 limit 100";

                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var a =reader.GetInt32(0);
                        var b =reader.GetInt32(1);
                        var c =reader.GetInt32(2);
                        var d =reader.GetInt32(3);
                        var e =reader.GetInt32(4);
                        var f = reader.GetInt32(5);
                    }
                }
            }
        }
    }

}

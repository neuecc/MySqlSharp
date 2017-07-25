using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace MySqlSharp
{
    public class MySqlDriverPool
    {
        // ConcurrentQueue<MySqlDriver> pool = new ConcurrentQueue<MySqlDriver>();

        readonly MySqlDriver[] pool;
        readonly object getLock = new object();
        readonly object createLock = new object();

        public MySqlDriverPool(int maxPoolSize)
        {
            pool = new MySqlDriver[maxPoolSize];
        }

        public void Rent()
        {



        }


        /*
    public MySqlDriver Rent()
    {
        //if (pool.TryDequeue(out var result))
        //{
        //    return result;
        //}
        //else
        //{
        //    lock (pool)
        //    {
        //        // pool.Count



        //    }
        //}
    }
    */

        public void Return(MySqlDriver driver)
        {
            //pool.Enqueue(driver);
        }


        public void Clean()
        {

        }
    }
}
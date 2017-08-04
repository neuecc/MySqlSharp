using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MySqlSharp
{
    public class MySqlDriverPool
    {
        // ConcurrentQueue<MySqlDriver> pool = new ConcurrentQueue<MySqlDriver>();

        int index;
        readonly Queue<MySqlDriver> pool;
        readonly object getLock = new object();
        readonly object createLock = new object();

        public MySqlDriverPool(int maxPoolSize)
        {
            //pool = new MySqlDriver[maxPoolSize];
        }

        public void Rent()
        {
            lock (getLock)
            {
                if (index == 0)
                {
                    // getLock[index];
                }
            }
        }

        public ValueTask<MySqlDriver> RentAsync()
        {
            //var hoge = new MySqlDriver();

            // pool[0] = new MySqlDriver();


            throw new NotImplementedException();
        }

        public void Return()
        {
        }

        public void Free()
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
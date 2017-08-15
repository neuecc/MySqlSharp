using System;
using System.Collections.Generic;
using System.Data;
using System.Text;

namespace MySqlSharp
{
    // TODO:more MySQL specified Transaction option(chain, release, etc...)
    // https://dev.mysql.com/doc/refman/5.6/ja/commit.html

    public enum MySqlIsolationLevel
    {
        Unspecified,
        RepeatableRead,
        ReadCommitted,
        ReadUncommitted,
        Serializable,
    }

    public class TransactionController : IDisposable
    {
        public MySqlIsolationLevel IsolationLevel { get; }
        bool isFinished;

        readonly MySqlDriver driver;

        public TransactionController(MySqlDriver driver)
        {
            driver.Query("START TRANSACTION");
        }

        internal void StartTransaction()
        {
            // TODO:ReadOnly?
            // TODO:Session?Global?
            string cmd;
            switch (IsolationLevel)
            {
                case MySqlIsolationLevel.ReadCommitted:
                    cmd = "SET SESSION TRANSACTION ISOLATION LEVEL READ COMMITTED;START TRANSACTION;";
                    break;
                case MySqlIsolationLevel.ReadUncommitted:
                    cmd = "SET SESSION TRANSACTION ISOLATION LEVEL READ UNCOMMITTED;START TRANSACTION;";
                    break;
                case MySqlIsolationLevel.RepeatableRead:
                    cmd = "SET SESSION TRANSACTION ISOLATION LEVEL REPEATABLE READ;START TRANSACTION;";
                    break;
                case MySqlIsolationLevel.Serializable:
                    cmd = "SET SESSION TRANSACTION ISOLATION LEVEL SERIALIZABLE;START TRANSACTION;";
                    break;
                case MySqlIsolationLevel.Unspecified:
                    cmd = "START TRANSACTION;";
                    break;
                default:
                    throw new NotSupportedException("TODO msg");
            }

            var resultSet = driver.Query(cmd);
            resultSet.ReadToEnd();
        }

        public void Commit()
        {
            if (isFinished)
            {
                throw new Exception("TODO");
            }
            else
            {
                var resultSet = driver.Query("COMMIT");
                resultSet.ReadToEnd();
                isFinished = true;
            }
        }

        public void Rollback()
        {
            if (isFinished)
            {
                throw new Exception("TODO");
            }
            Dispose();
        }

        // TODO:RollbackAsync

        // Rollback
        public void Dispose()
        {
            if (!isFinished)
            {
                var resultSet = driver.Query("ROLLBACK");
                resultSet.ReadToEnd();
                isFinished = true;
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;

namespace Common.DataAccess.EF
{
    public class MyConfiguration : DbConfiguration
    {
        public MyConfiguration()
        {
            // Entity Framework Handling of Transaction Commit Failures (EF6.1 Onwards)
            // reference: https://msdn.microsoft.com/en-us/library/dn630221(v=vs.113).aspx
            SetTransactionHandler(SqlProviderServices.ProviderInvariantName, () => new CommitFailureHandler());

            // for SQL Azure
            // User initiated transactions not supported
            // reference: https://msdn.microsoft.com/en-us/library/dn307226(v=vs.113).aspx
            SetExecutionStrategy(SqlProviderServices.ProviderInvariantName, () =>
            {
                if (SuspendExecutionStrategy)
                {
                    return new DefaultExecutionStrategy();
                }
                else
                {
                    return new SqlAzureExecutionStrategy();
                }
            });
        }

        public static bool SuspendExecutionStrategy
        {
            get
            {
                return (bool?)CallContext.LogicalGetData("SuspendExecutionStrategy") ?? false;
            }

            set
            {
                CallContext.LogicalSetData("SuspendExecutionStrategy", value);
            }
        }
    }
}

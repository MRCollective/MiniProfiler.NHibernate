using NHibernate;
using NHibernate.AdoNet;
using NHibernate.Engine;

namespace StackExchange.Profiling.NHibernate.Infrastructure
{
    internal class ProfiledSqlClientBatchingBatcherFactory : SqlClientBatchingBatcherFactory
    {
        public override IBatcher CreateBatcher(ConnectionManager connectionManager, IInterceptor interceptor)
        {
            return new ProfiledSqlClientBatchingBatcher(connectionManager, interceptor);
        }
    }
}
using NHibernate;
using NHibernate.AdoNet;
using NHibernate.Engine;

namespace MiniProfiler.NHibernate.NHibernate
{
    public class ProfiledSqlClientBatchingBatcherFactory : SqlClientBatchingBatcherFactory
    {
        public override IBatcher CreateBatcher(ConnectionManager connectionManager, IInterceptor interceptor)
        {
            return new ProfiledSqlClientBatchingBatcher(connectionManager, interceptor);
        }
    }
}
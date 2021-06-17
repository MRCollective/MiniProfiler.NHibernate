using NHibernate;
using NHibernate.AdoNet;
using NHibernate.Engine;

namespace StackExchange.Profiling.NHibernate.Infrastructure
{
#pragma warning disable CS0246 // The type or namespace name 'SqlClientBatchingBatcherFactory' could not be found (are you missing a using directive or an assembly reference?)
    internal class ProfiledSqlClientBatchingBatcherFactory : SqlClientBatchingBatcherFactory
#pragma warning restore CS0246 // The type or namespace name 'SqlClientBatchingBatcherFactory' could not be found (are you missing a using directive or an assembly reference?)
    {
#pragma warning disable CS0115 // 'ProfiledSqlClientBatchingBatcherFactory.CreateBatcher(ConnectionManager, IInterceptor)': no suitable method found to override
        public override IBatcher CreateBatcher(ConnectionManager connectionManager, IInterceptor interceptor)
#pragma warning restore CS0115 // 'ProfiledSqlClientBatchingBatcherFactory.CreateBatcher(ConnectionManager, IInterceptor)': no suitable method found to override
        {
            return new ProfiledSqlClientBatchingBatcher(connectionManager, interceptor);
        }
    }
}
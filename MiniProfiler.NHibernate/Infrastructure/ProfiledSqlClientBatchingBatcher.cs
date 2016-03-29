
using System.Data;
using NHibernate;
using NHibernate.AdoNet;
using StackExchange.Profiling.Data;
using System;
using System.Data.SqlClient;

namespace StackExchange.Profiling.NHibernate.Infrastructure
{
    internal class ProfiledSqlClientBatchingBatcher : StackExchange.Profiling.NHibernate.Infrastructure.SqlClientBatchingBatcher
    {
        public readonly IDbProfiler _profiler;

        public ProfiledSqlClientBatchingBatcher(ConnectionManager connectionManager, IInterceptor interceptor, IDbProfiler profiler) 
            : base(connectionManager, interceptor)
        {
            _profiler = profiler;
        }

        protected override void DoExecuteBatch(IDbCommand ps)
        {
            if (this._profiler == null || !this._profiler.IsActive)
            {
                base.DoExecuteBatch(ps);
            }

            this._profiler.ExecuteStart(ps, SqlExecuteType.NonQuery);
            try
            {
                base.DoExecuteBatch(ps);
            }
            catch (Exception e)
            {
                _profiler.OnError(ps, SqlExecuteType.NonQuery, e);
                throw;
            }
            finally
            {
                _profiler.ExecuteFinish(ps, SqlExecuteType.NonQuery, null);
            }
        }

        protected override SqlCommand GetSqlCommand(IDbCommand batchUpdate)
        {
            // ProfiledSqlDbCommand derives from DbCommand which cannot be casted to SqlCommand as
            // (sealed) SqlCommand derives from DbCommand. To avoid this issue, return the original
            // wrapped SqlCommand instead.
            ProfiledSqlDbCommand command = batchUpdate as ProfiledSqlDbCommand;
            if (command != null)
            {
                return command.SqlCommand;
            }

            return base.GetSqlCommand(batchUpdate);
        }
    }
}
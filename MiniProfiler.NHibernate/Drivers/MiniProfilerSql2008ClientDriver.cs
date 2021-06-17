using NHibernate.AdoNet;
using NHibernate.Driver;
using StackExchange.Profiling.NHibernate.Infrastructure;
using System;
using System.Data.Common;

namespace StackExchange.Profiling.NHibernate.Drivers
{
    public class MiniProfilerSql2008ClientDriver : Sql2008ClientDriver, IEmbeddedBatcherFactoryProvider
    {
        public override DbCommand CreateCommand()
        {
            var command = base.CreateCommand();

            if (MiniProfiler.Current != null)
                command = new ProfiledSqlDbCommand((DbCommand)command, MiniProfiler.Current);

            return command;
        }

        Type IEmbeddedBatcherFactoryProvider.BatcherFactoryClass => typeof(ProfiledSqlClientBatchingBatcherFactory);
    }
}
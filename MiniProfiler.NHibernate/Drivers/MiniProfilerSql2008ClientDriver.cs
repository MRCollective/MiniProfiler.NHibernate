using System;
using System.Data;
using System.Data.Common;
using MiniProfiler.NHibernate.NHibernate;
using NHibernate.AdoNet;
using NHibernate.Driver;

namespace MiniProfiler.NHibernate.Drivers
{
    public class MiniProfilerSql2008ClientDriver : Sql2008ClientDriver, IEmbeddedBatcherFactoryProvider
    {
        public override IDbCommand CreateCommand()
        {
            var command = base.CreateCommand();

            if (StackExchange.Profiling.MiniProfiler.Current != null)
                command = new ProfiledSqlDbCommand((DbCommand)command, StackExchange.Profiling.MiniProfiler.Current);

            return command;
        }

        Type IEmbeddedBatcherFactoryProvider.BatcherFactoryClass
        {
            get { return typeof(ProfiledSqlClientBatchingBatcherFactory); }
        }
    }
}
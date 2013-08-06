using System;
using System.Data;
using System.Data.Common;
using System.Data.OracleClient;
using NHibernate.AdoNet;
using NHibernate.Driver;
using StackExchange.Profiling.NHibernate.Infrastructure;

namespace StackExchange.Profiling.NHibernate.Drivers.Oracle
{
    public class MiniProfilerOracle10ClientDriver : OracleClientDriver, IEmbeddedBatcherFactoryProvider
    {
        public override IDbCommand CreateCommand()
        {
            var command = base.CreateCommand();

            if (MiniProfiler.Current != null)
                command = new ProfiledGenericDbCommand<OracleCommand>((DbCommand)command, MiniProfiler.Current);

            return command;
        }

        Type IEmbeddedBatcherFactoryProvider.BatcherFactoryClass
        {
            get { return typeof(OracleDataClientBatchingBatcherFactory); }
        }
    }
}
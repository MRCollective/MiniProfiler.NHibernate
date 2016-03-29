using System;
using System.Data;
using System.Data.Common;
using NHibernate.AdoNet;
using NHibernate.Driver;
using StackExchange.Profiling.NHibernate.Infrastructure;
using StackExchange.Profiling.Data;
using System.Data.SqlClient;

namespace StackExchange.Profiling.NHibernate.Drivers
{
    public class ProfiledSql2008ClientDriver : Sql2008ClientDriver, IEmbeddedBatcherFactoryProvider
    {
        public override IDbCommand CreateCommand()
        {
            IDbCommand command = base.CreateCommand();
            
            if (MiniProfiler.Current != null)
            {
                command = new ProfiledSqlDbCommand(
                    command as SqlCommand,
                    command.Connection as DbConnection,
                    MiniProfiler.Current
                );
            }

            return command;       
        }

        public override IDbConnection CreateConnection()
        {
            IDbConnection connection = base.CreateConnection();

            if (MiniProfiler.Current != null)
            {
                connection = new ProfiledDbConnection(
                    connection as DbConnection,
                    MiniProfiler.Current
                );
            }

            return connection;
        }


        public Type BatcherFactoryClass
        {
            get { return typeof(ProfiledSqlClientBatchingBatcherFactory); }
        }
    }
}
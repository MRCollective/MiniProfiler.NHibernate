using System;
using System.Data.Common;
using System.Data.SqlClient;
using StackExchange.Profiling.Data;

namespace StackExchange.Profiling.NHibernate.Infrastructure
{
    [Obsolete]
    internal class ProfiledSqlDbCommand : ProfiledDbCommand
    {
        private readonly SqlCommand _sqlCommand;

        public ProfiledSqlDbCommand(DbCommand command, IDbProfiler profiler) : base(command, null, profiler)
        {
            _sqlCommand = (SqlCommand)command;
        }

        public SqlCommand SqlCommand
        {
            get { return _sqlCommand; }
        }
    }
}
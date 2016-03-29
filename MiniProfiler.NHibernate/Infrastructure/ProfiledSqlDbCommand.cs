using System.Data.Common;
using System.Data.SqlClient;
using StackExchange.Profiling.Data;

namespace StackExchange.Profiling.NHibernate.Infrastructure
{
    public class ProfiledSqlDbCommand : ProfiledDbCommand
    {
        public ProfiledSqlDbCommand(SqlCommand command, DbConnection connection, IDbProfiler profiler) 
            : base(command, connection, profiler)
        {

        }

        public SqlCommand SqlCommand
        {
            get
            {
                return (SqlCommand)base.InternalCommand;
            }
        }
    }
}
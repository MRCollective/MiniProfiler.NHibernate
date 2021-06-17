using System.Data.Common;
#pragma warning disable CS0234 // The type or namespace name 'SqlClient' does not exist in the namespace 'System.Data' (are you missing an assembly reference?)
using System.Data.SqlClient;
#pragma warning restore CS0234 // The type or namespace name 'SqlClient' does not exist in the namespace 'System.Data' (are you missing an assembly reference?)
#pragma warning disable CS0234 // The type or namespace name 'Data' does not exist in the namespace 'StackExchange.Profiling' (are you missing an assembly reference?)
using StackExchange.Profiling.Data;
#pragma warning restore CS0234 // The type or namespace name 'Data' does not exist in the namespace 'StackExchange.Profiling' (are you missing an assembly reference?)

namespace StackExchange.Profiling.NHibernate.Infrastructure
{
#pragma warning disable CS0246 // The type or namespace name 'ProfiledDbCommand' could not be found (are you missing a using directive or an assembly reference?)
    public class ProfiledSqlDbCommand : ProfiledDbCommand
#pragma warning restore CS0246 // The type or namespace name 'ProfiledDbCommand' could not be found (are you missing a using directive or an assembly reference?)
    {
#pragma warning disable CS0246 // The type or namespace name 'SqlCommand' could not be found (are you missing a using directive or an assembly reference?)
        private readonly SqlCommand _sqlCommand;
#pragma warning restore CS0246 // The type or namespace name 'SqlCommand' could not be found (are you missing a using directive or an assembly reference?)

#pragma warning disable CS0246 // The type or namespace name 'IDbProfiler' could not be found (are you missing a using directive or an assembly reference?)
        public ProfiledSqlDbCommand(DbCommand command, IDbProfiler profiler) : base(command, null, profiler)
#pragma warning restore CS0246 // The type or namespace name 'IDbProfiler' could not be found (are you missing a using directive or an assembly reference?)
        {
            _sqlCommand = (SqlCommand)command;
        }

#pragma warning disable CS0246 // The type or namespace name 'SqlCommand' could not be found (are you missing a using directive or an assembly reference?)
        public SqlCommand SqlCommand
#pragma warning restore CS0246 // The type or namespace name 'SqlCommand' could not be found (are you missing a using directive or an assembly reference?)
        {
            get { return _sqlCommand; }
        }
    }
}
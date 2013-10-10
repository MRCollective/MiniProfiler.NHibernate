using System.Data.Common;
using StackExchange.Profiling.Data;

namespace StackExchange.Profiling.NHibernate.Infrastructure
{
    internal class ProfiledGenericDbCommand<T> : ProfiledDbCommand 
        where T : DbCommand
    {
        private readonly T _command;

        public ProfiledGenericDbCommand(DbCommand command, IDbProfiler profiler) 
            : base(command, null, profiler)
        {
            _command = (T)command;
        }

        public T Command
        {
            get { return _command; }
        }
    }
}
using System;
using System.Data;
using System.Data.Common;
using NHibernate.AdoNet;
using NHibernate.Driver;
using StackExchange.Profiling.NHibernate.Infrastructure;
using StackExchange.Profiling.Data;

namespace StackExchange.Profiling.NHibernate.Drivers
{
	public class MiniProfilerSQLite20Driver :  SQLite20Driver
	{
		public override IDbCommand CreateCommand()
		{
			var command = base.CreateCommand();

			if (MiniProfiler.Current != null)
				command = new ProfiledDbCommand((DbCommand)command, null, MiniProfiler.Current);

			return command;
		}
			
	}
}

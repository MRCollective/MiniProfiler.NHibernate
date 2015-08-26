﻿using System;
using System.Data;
using System.Data.Common;
using NHibernate.AdoNet;
using NHibernate.Driver;
using StackExchange.Profiling.NHibernate.Infrastructure;

namespace StackExchange.Profiling.NHibernate.Drivers
{
	public class MiniProfilerSQLite20Driver :  SQLite20Driver, IEmbeddedBatcherFactoryProvider
	{
		public override IDbCommand CreateCommand()
		{
			var command = base.CreateCommand();

			if (MiniProfiler.Current != null)
				command = new ProfiledSqlDbCommand((DbCommand)command, MiniProfiler.Current);

			return command;
		}

		Type IEmbeddedBatcherFactoryProvider.BatcherFactoryClass
		{
			get { return typeof(ProfiledSqlClientBatchingBatcherFactory); }
		}
	}
}

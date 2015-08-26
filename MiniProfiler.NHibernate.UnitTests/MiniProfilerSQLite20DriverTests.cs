using System;
using System.Linq;
using NUnit.Framework;
using NHibernate;
using NHibernate.AdoNet;
using NHibernate.AdoNet.Util;
using NHibernate.Exceptions;
using NHibernate.Util;
using NHibernate.Linq;
using StackExchange.Profiling;
using StackExchange.Profiling.NHibernate.Infrastructure;
using Profiler = StackExchange.Profiling.MiniProfiler;
using System.Diagnostics;
using System.Collections.Generic;

namespace MiniProfiler.NHibernate.UnitTests
{
	//TODO: write SQLite batcher for tests 
	[TestFixture ()]
	public class MiniProfilerSQLite20DriverTests
	{

		[Test]
		public void Profiling_does_not_influence_operations()
		{
			Profiler.Settings.ProfilerProvider = new SingletonProfilerProvider ();
			Profiler.Start(StackExchange.Profiling.ProfileLevel.Verbose);
			var provider = ProfiledInMemorySessionFactoryProvider.CreateSessionFactory<Person> ();
			var session = provider.OpenSession();

			var t = session.BeginTransaction ();
			session.Save (new Person{ Id = 1, Name = "John" });
			session.Save (new Person{ Id = 2, Name = "Don" });
			t.Commit ();
			Profiler.Stop ();
			var result = from p in  session.Query<Person> ()
				     	 where p.Id == 2
				         select p;
			Assert.AreEqual ("Don", result.FirstOrDefault().Name);

		}

		[Test]
		public void Profiling_of_non_batched_saves_works()
		{

			IList<object> listener = new List<object>();
			LoggerProvider.SetLoggersFactory (new DelegatingSqlLoggerFactory (listener));
			Profiler.Settings.ProfilerProvider = new SingletonProfilerProvider ();
			Profiler.Start(StackExchange.Profiling.ProfileLevel.Verbose);
			var provider = ProfiledInMemorySessionFactoryProvider.CreateSessionFactory<Person> ();
			var session = provider.OpenSession();
			var t = session.BeginTransaction ();
			session.Save (new Person{ Id = 1, Name = "John" });
			session.Save (new Person{ Id = 2, Name = "Don" });
			t.Commit ();
			Profiler.Stop ();
			var sqlTimings = Profiler.Current.GetSqlTimings ();
			Assert.AreEqual(2,sqlTimings.Count);
			Assert.AreEqual(listener.Count,sqlTimings.Count);
		}
	}
}


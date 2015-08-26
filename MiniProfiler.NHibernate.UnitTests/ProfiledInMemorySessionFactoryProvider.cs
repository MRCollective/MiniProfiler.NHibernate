using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using StackExchange.Profiling.NHibernate.Drivers;

namespace MiniProfiler.NHibernate.UnitTests
{

	public class ProfiledInMemorySessionFactoryProvider
	{
		public ISessionFactory SessionFactory;
		public Configuration Configuration;

		private ProfiledInMemorySessionFactoryProvider() { }

		private class MiniProfilerSQLiteConfiguration:SQLiteConfiguration
		{
			public MiniProfilerSQLiteConfiguration()
			{
				base.Driver<MiniProfilerSQLite20Driver>();
			}
		}

		public static ProfiledInMemorySessionFactoryProvider CreateSessionFactory<T>()
		{
			var provider = new ProfiledInMemorySessionFactoryProvider ();

			provider.SessionFactory = Fluently.Configure()
				.Database(new MiniProfilerSQLiteConfiguration().InMemory().ShowSql())
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<T>())
				//.ExposeConfiguration(cfg => cfg.SetProperty("adonet.batch_size","20"))
				//.ExposeConfiguration(cfg => cfg.SetProperty("generate_statistics", "true"))
				.ExposeConfiguration(cfg => provider.Configuration = cfg)
				.BuildSessionFactory();
			return provider;
		}

		public ISession OpenSession()
		{
			ISession session = SessionFactory.OpenSession();
			var export = new SchemaExport(Configuration);
			export.Execute(true, true, false, session.Connection, null);
			return session;
		}

		public void Dispose()
		{
			if(SessionFactory != null)
				SessionFactory.Dispose();
			SessionFactory = null;
			Configuration = null;
		}
	}
}
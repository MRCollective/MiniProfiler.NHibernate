using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;

namespace MiniProfiler.NHibernate.UnitTests
{
	public class InMemorySessionFactoryProvider
	{
		public ISessionFactory SessionFactory;
		public Configuration Configuration;

		private InMemorySessionFactoryProvider() { }

		public static InMemorySessionFactoryProvider CreateSessionFactory<T>()
		{
			var provider = new InMemorySessionFactoryProvider ();

			provider.SessionFactory = Fluently.Configure()
				.Database(SQLiteConfiguration.Standard.InMemory().ShowSql())
				.Mappings(m => m.FluentMappings.AddFromAssemblyOf<T>())
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
using System;
using System.Linq;
using NUnit.Framework;

using NHibernate;
using NHibernate.AdoNet;
using NHibernate.AdoNet.Util;
using NHibernate.Exceptions;
using NHibernate.Util;
using NHibernate.Linq;

using StackExchange.Profiling.NHibernate.Infrastructure;

namespace MiniProfiler.NHibernate.UnitTests
{

	public class Person
	{
		public virtual int Id{ get; set;}
		public virtual string Name{get;set;}
	}

	public class PersonMap:FluentNHibernate.Mapping.ClassMap<Person>
	{
		public PersonMap()
		{
			this.Id(x=> x.Id).Not.Nullable().Unique();
			this.Map(x=> x.Name);
		}
	}

	[TestFixture ()]
	public class ProfiledSqlClientBatchingBatcherTests
	{
		[Test ()]
		public void Create_nulls_error ()
		{
			Assert.Throws<NullReferenceException> (()=> new ProfiledSqlClientBatchingBatcher (null, null));
		}




		[Test]
		public void DO()
		{
			var provider = InMemorySessionFactoryProvider.CreateSessionFactory<Person> ();
			var session = provider.OpenSession();

			var t = session.BeginTransaction ();
			session.Save (new Person{ Id = 1, Name = "John" });
			session.Save (new Person{ Id = 2, Name = "Don" });
			t.Commit ();

			var result = from p in  session.Query<Person> ()
			             where p.Id == 2
			             select p;
			Assert.AreEqual ("Don", result.FirstOrDefault().Name);
		}
	}
}


using System;

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
}


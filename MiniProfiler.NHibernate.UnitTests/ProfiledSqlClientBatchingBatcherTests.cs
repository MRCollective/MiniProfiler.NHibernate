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
	[TestFixture ()]
	public class ProfiledSqlClientBatchingBatcherTests
	{
		[Test ()]
		public void Create_nulls_error ()
		{
			Assert.Throws<NullReferenceException> (()=> new ProfiledSqlClientBatchingBatcher (null, null));
		}	
	}
}


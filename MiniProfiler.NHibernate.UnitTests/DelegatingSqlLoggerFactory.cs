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
using System.Reflection;
using System.Collections.Generic;

namespace MiniProfiler.NHibernate.UnitTests
{

	/// <summary>
	/// Factory which uses default logging of <see cref="LoggerProvider"/> until logged data directly 
	/// relates to final SQL queries executed.
	/// </summary>
	public class DelegatingSqlLoggerFactory : ILoggerFactory
	{
		private ILoggerFactory _default;
		private ICollection<object> _traceListener;

		public DelegatingSqlLoggerFactory(ICollection<object> traceListener)
		{						
			var instance = typeof(LoggerProvider).InvokeMember("instance",  BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Static, null, null, null);
			var loggerFactory =  typeof(LoggerProvider).InvokeMember("loggerFactory",  BindingFlags.NonPublic | BindingFlags.GetField | BindingFlags.Instance, null, instance, null);
			_default = (ILoggerFactory)loggerFactory;
			_traceListener = traceListener;
		}

		public IInternalLogger LoggerFor (string keyName)
		{
			// parametrized SQL queries executed
			if ("NHibernate.SQL".Equals(keyName,StringComparison.OrdinalIgnoreCase))
			{
				return new SqlLogger(_traceListener);
			}
			return _default.LoggerFor(keyName);
		}
		public IInternalLogger LoggerFor (Type type)
		{
			//TODO: Intercept NHibernate.AdoNet(like AbstractBatcher) to get whole picture of SQL execution including schema/table changes and  batching and other.
			return _default.LoggerFor (type);
		}
	}
}


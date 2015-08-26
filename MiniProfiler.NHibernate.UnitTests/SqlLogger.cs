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
	public class SqlLogger:IInternalLogger
	{
		ICollection<object> _listener;

		public SqlLogger(ICollection<object> listener)
		{
			_listener = listener;
		}

		public void Error (object message)
		{
			
		}
		public void Error (object message, Exception exception)
		{

		}
		public void ErrorFormat (string format, params object[] args)
		{

		}
		public void Fatal (object message)
		{

		}
		public void Fatal (object message, Exception exception)
		{

		}
		public void Debug (object message)
		{
			_listener.Add(message);
		}
		public void Debug (object message, Exception exception)
		{
			
		}
		public void DebugFormat (string format, params object[] args)
		{

		}
		public void Info (object message)
		{

		}
		public void Info (object message, Exception exception)
		{

		}
		public void InfoFormat (string format, params object[] args)
		{

		}
		public void Warn (object message)
		{

		}
		public void Warn (object message, Exception exception)
		{

		}
		public void WarnFormat (string format, params object[] args)
		{

		}
		public bool IsErrorEnabled {
			get {
				return true;
			}
		}
		public bool IsFatalEnabled {
			get {
				return true;
			}
		}
		public bool IsDebugEnabled {
			get {
				return true;
			}
		}
		public bool IsInfoEnabled {
			get {
				return true;
			}
		}
		public bool IsWarnEnabled {
			get {
				return true;
			}
		}

	}

}


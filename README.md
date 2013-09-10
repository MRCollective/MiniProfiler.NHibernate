MiniProfiler.NHibernate
=======================

NHibernate drivers supporting logging to MiniProfiler.

Usage
-----

`Install-Package MiniProfiler.NHibernate`

Configure FluentNHibernate to use the provided driver:

    Fluently.Configure().Database(
      MsSqlConfiguration.MsSql2008.ConnectionString(ConnectionString).Driver<MiniProfiler.NHibernate.MiniProfilerSql2008ClientDriver>()
    );

Or with XML style configuration:

    cfg.SetProperty(Environment.ConnectionDriver, typeof(MiniProfiler.NHibernate.MiniProfilerSql2008ClientDriver).AssemblyQualifiedName)

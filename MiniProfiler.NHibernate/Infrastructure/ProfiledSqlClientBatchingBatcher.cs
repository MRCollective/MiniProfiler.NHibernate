using NHibernate;
using NHibernate.AdoNet;
using NHibernate.AdoNet.Util;
using NHibernate.Exceptions;
using NHibernate.Util;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace StackExchange.Profiling.NHibernate.Infrastructure
{
    internal class ProfiledSqlClientBatchingBatcher : AbstractBatcher
    {
        private int _batchSize;
        private int _totalExpectedRowsAffected;
        private SqlClientSqlCommandSet _currentBatch;
        private StringBuilder _currentBatchCommandsLog;
        private readonly int _defaultTimeout;

        public ProfiledSqlClientBatchingBatcher(ConnectionManager connectionManager, IInterceptor interceptor) : base(connectionManager, interceptor)
        {
            _batchSize = Factory.Settings.AdoBatchSize;
#pragma warning disable CS0618 // 'Environment.Properties' is obsolete: 'This property is not used and will be removed in a future version.'
            _defaultTimeout = PropertiesHelper.GetInt32(global::NHibernate.Cfg.Environment.CommandTimeout, global::NHibernate.Cfg.Environment.Properties, -1);
#pragma warning restore CS0618 // 'Environment.Properties' is obsolete: 'This property is not used and will be removed in a future version.'

            _currentBatch = CreateConfiguredBatch();
            _currentBatchCommandsLog = new StringBuilder().AppendLine("Batch commands:");
        }

        public override int BatchSize
        {
            get => _batchSize;
            set => _batchSize = value;
        }

        protected override Task DoExecuteBatchAsync(DbCommand ps, CancellationToken cancellationToken)
        {
            CheckReaders();
            Prepare(_currentBatch.BatchCommand);
            if (Factory.Settings.SqlStatementLogger.IsDebugEnabled)
            {
                Factory.Settings.SqlStatementLogger.LogBatchCommand(_currentBatchCommandsLog.ToString());
                _currentBatchCommandsLog = new StringBuilder().AppendLine("Batch commands:");
            }

            int rowsAffected;
            try
            {
                rowsAffected = _currentBatch.ExecuteNonQuery();
            }
            catch (DbException e)
            {
                throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, e, "could not execute batch command.");
            }

            Expectations.VerifyOutcomeBatched(_totalExpectedRowsAffected, rowsAffected, ps);

            _currentBatch.Dispose();
            _totalExpectedRowsAffected = 0;
            _currentBatch = CreateConfiguredBatch();

            return Task.CompletedTask;
        }

        public override Task AddToBatchAsync(IExpectation expectation, CancellationToken cancellationToken)
        {
            _totalExpectedRowsAffected += expectation.ExpectedRowCount;
            var batchUpdate = CurrentCommand;
            Driver.AdjustCommand(batchUpdate);
            var sqlStatementLogger = Factory.Settings.SqlStatementLogger;
            if (sqlStatementLogger.IsDebugEnabled)
            {
                var lineWithParameters = sqlStatementLogger.GetCommandLineWithParameters(batchUpdate);
                var formatStyle = sqlStatementLogger.DetermineActualStyle(FormatStyle.Basic);
                lineWithParameters = formatStyle.Formatter.Format(lineWithParameters);
                _currentBatchCommandsLog.Append("command ")
                    .Append(_currentBatch.CountOfCommands)
                    .Append(":")
                    .AppendLine(lineWithParameters);
            }

            if (batchUpdate is ProfiledSqlDbCommand update)
            {
                _currentBatch.Append(update.SqlCommand);
            }
            else
            {
                _currentBatch.Append((SqlCommand)batchUpdate);
            }

            if (_currentBatch.CountOfCommands >= _batchSize)
            {
                ExecuteBatchWithTiming(batchUpdate);
            }

            return Task.CompletedTask;
        }

        protected override int CountOfStatementsInCurrentBatch => _currentBatch.CountOfCommands;

        protected override void DoExecuteBatch(DbCommand ps)
        {
            CheckReaders();
            Prepare(_currentBatch.BatchCommand);
            if (Factory.Settings.SqlStatementLogger.IsDebugEnabled)
            {
                Factory.Settings.SqlStatementLogger.LogBatchCommand(_currentBatchCommandsLog.ToString());
                _currentBatchCommandsLog = new StringBuilder().AppendLine("Batch commands:");
            }

            int rowsAffected;
            try
            {
                rowsAffected = _currentBatch.ExecuteNonQuery();
            }
            catch (DbException e)
            {
                throw ADOExceptionHelper.Convert(Factory.SQLExceptionConverter, e, "could not execute batch command.");
            }

            Expectations.VerifyOutcomeBatched(_totalExpectedRowsAffected, rowsAffected, ps);

            _currentBatch.Dispose();
            _totalExpectedRowsAffected = 0;
            _currentBatch = CreateConfiguredBatch();
        }

        public override void AddToBatch(IExpectation expectation)
        {
            _totalExpectedRowsAffected += expectation.ExpectedRowCount;
            var batchUpdate = CurrentCommand;
            Driver.AdjustCommand(batchUpdate);
            var sqlStatementLogger = Factory.Settings.SqlStatementLogger;
            if (sqlStatementLogger.IsDebugEnabled)
            {
                var lineWithParameters = sqlStatementLogger.GetCommandLineWithParameters(batchUpdate);
                var formatStyle = sqlStatementLogger.DetermineActualStyle(FormatStyle.Basic);
                lineWithParameters = formatStyle.Formatter.Format(lineWithParameters);
                _currentBatchCommandsLog.Append("command ")
                    .Append(_currentBatch.CountOfCommands)
                    .Append(":")
                    .AppendLine(lineWithParameters);
            }

            if (batchUpdate is ProfiledSqlDbCommand update)
            {
                _currentBatch.Append(update.SqlCommand);
            }
            else
            {
                _currentBatch.Append((SqlCommand)batchUpdate);
            }

            if (_currentBatch.CountOfCommands >= _batchSize)
            {
                ExecuteBatchWithTiming(batchUpdate);
            }
        }

        private SqlClientSqlCommandSet CreateConfiguredBatch()
        {
            var result = new SqlClientSqlCommandSet();
            if (_defaultTimeout > 0)
            {
                try
                {
                    result.CommandTimeout = _defaultTimeout;
                }
                catch {}
            }

            return result;
        }
    }
}
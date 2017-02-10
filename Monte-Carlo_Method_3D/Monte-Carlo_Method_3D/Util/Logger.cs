using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Disposables;
using System.Reactive.Subjects;
using System.Text;
using System.Threading.Tasks;
using Monte_Carlo_Method_3D.Util.AssertHelper;

namespace Monte_Carlo_Method_3D.Util
{
    public enum LogLevel
    {
        Debug = 0,
        Info = 1,
        Perf = 2,
        Warn = 3,
        Error = 4
    }

    public sealed class Logger
    {
        public struct LogMessage
        {
            public string LoggerName { get; private set; }
            public int ThreadId { get; private set; }
            public string Message { get; private set; }
            public LogLevel LogLevel { get; private set; }

            public LogMessage(string loggerName, int threadId, string message, LogLevel logLevel)
            {
                LoggerName = loggerName;
                ThreadId = threadId;
                Message = message;
                LogLevel = logLevel;
            }
        }

        private static Subject<LogMessage> m_Messages = new Subject<LogMessage>();

        public static IObservable<LogMessage> Messages => m_Messages;

        public static LogLevel Threshold { get; set; }

        public bool IsDebugEnabled => Threshold <= LogLevel.Debug;

        public bool IsInfoEnabled => Threshold <= LogLevel.Info;

        public bool IsPrefEnabled => Threshold <= LogLevel.Perf;

        public bool IsWarnEnabled => Threshold <= LogLevel.Warn;

        public bool IsErrorEnabled => Threshold <= LogLevel.Error;

        public static Logger New(string name)
        {
            name.AssertNotNullOrWhitespace(nameof(name));

            return new Logger(name);
        }

        public static Logger New(Type type)
        {
            type.AssertNotNull(nameof(type));

            return new Logger(type.Name);
        }

        // Instance members

        private readonly string m_Name;

        private Logger(string name)
        {
            name.AssertNotNullOrWhitespace(nameof(name));

            m_Name = name;
        }

        private void Log(string message, LogLevel level)
        {
            if (Threshold <= level)
            {
                m_Messages.OnNext(new LogMessage(m_Name, Environment.CurrentManagedThreadId, message, level));
            }
        }

        public void LogDebug(string message)
        {
            Log(message, LogLevel.Debug);
        }

        public void LogInfo(string message)
        {
            Log(message, LogLevel.Info);
        }

        public IDisposable LogPerf(string message)
        {
            if (IsPrefEnabled)
            {
                return new PerfMesuare(this, message);
            }
            return Disposable.Empty;
        }

        private void LogPerfResults(TimeSpan results, string message)
        {
            Log($"{message}: {results} ({results.Milliseconds} ms)", LogLevel.Perf);
        }

        public void LogWarn(string message)
        {
            Log(message, LogLevel.Warn);
        }

        public void LogError(string message)
        {
            Log(message, LogLevel.Error);
        }

        private sealed class PerfMesuare : IDisposable
        {
            private readonly Logger m_Owner;
            private readonly string m_Message;
            private readonly Stopwatch m_Stopwatch;

            public PerfMesuare(Logger owner, string message)
            {
                owner.AssertNotNull(nameof(owner));
                message.AssertNotNullOrWhitespace(nameof(message));

                m_Owner = owner;
                m_Message = message;
                m_Stopwatch = Stopwatch.StartNew();
            }

            public void Dispose()
            {
                m_Stopwatch.Stop();
                var result = m_Stopwatch.Elapsed;
                m_Owner.LogPerfResults(result, m_Message);
            }
        }
    }
}

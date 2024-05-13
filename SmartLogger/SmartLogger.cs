using SmartLogger.LogPersistance;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLogger;

public class SmartLogger : ILogAggregator
{

    private ConcurrentBag<LogMessage> _messages = new ConcurrentBag<LogMessage>();

    private Configuration _configuration = new Configuration();

    #region ILogAggregator
    void ILogAggregator.Flush(Severity? severity)
    {
        throw new NotImplementedException();
    }

    Task ILogAggregator.FlushAsync(Severity? severity)
    {
        throw new NotImplementedException();
    }

    void ILogAggregator.LogDebug(string message, int lineNumber, string memberName, string filePath)
    {
        throw new NotImplementedException();
    }

    void ILogAggregator.LogError(string message, int lineNumber, string memberName, string filePath)
    {
        throw new NotImplementedException();
    }

    void ILogAggregator.LogError(Exception exception, int lineNumber, string memberName, string filePath)
    {
        throw new NotImplementedException();
    }

    void ILogAggregator.LogFatal(string message, int lineNumber, string memberName, string filePath)
    {
        throw new NotImplementedException();
    }

    void ILogAggregator.LogFatal(Exception exception, int lineNumber, string memberName, string filePath)
    {
        throw new NotImplementedException();
    }

    void ILogAggregator.LogInformation(string message, int lineNumber, string memberName, string filePath)
    {
        throw new NotImplementedException();
    }

    void ILogAggregator.LogWarning(string message, int lineNumber, string memberName, string filePath)
    {
        throw new NotImplementedException();
    }

    bool ILogAggregator.RegisterObserver(string name, Action<LogMessage> observer)
    {
        throw new NotImplementedException();
    }

    void ILogAggregator.SetLogPersistanceSeverity(Severity severity)
    {
        throw new NotImplementedException();
    }

    bool ILogAggregator.UnregisterObserver(string name)
    {
        throw new NotImplementedException();
    }
    #endregion
}

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

    private Configuration _configuration;

    private int _eventSequenceCounter = 0;

    #region Constructor

    public SmartLogger()
    {
        _configuration  = new Configuration();
    }

    public SmartLogger(Configuration configuration)
    {
        _configuration = configuration;
    }

    #endregion

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
        AddLogMessage(Severity.DEBUG, message, lineNumber, memberName, filePath);
    }

    void ILogAggregator.LogError(string message, int lineNumber, string memberName, string filePath)
    {
        AddLogMessage(Severity.ERROR, message, lineNumber, memberName, filePath);
    }

    void ILogAggregator.LogError(Exception exception, int lineNumber, string memberName, string filePath)
    {
        AddLogMessage(Severity.ERROR, exception, lineNumber, memberName, filePath);
    }

    void ILogAggregator.LogFatal(string message, int lineNumber, string memberName, string filePath)
    {
        AddLogMessage(Severity.FATAL, message, lineNumber, memberName, filePath);
    }

    void ILogAggregator.LogFatal(Exception exception, int lineNumber, string memberName, string filePath)
    {
        AddLogMessage(Severity.FATAL, exception, lineNumber, memberName, filePath);
    }

    void ILogAggregator.LogInformation(string message, int lineNumber, string memberName, string filePath)
    {
        AddLogMessage(Severity.INFORMATION, message, lineNumber, memberName, filePath);
    }

    void ILogAggregator.LogWarning(string message, int lineNumber, string memberName, string filePath)
    {
        AddLogMessage(Severity.WARNING, message, lineNumber, memberName, filePath);
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

    #region Private Methods

    private void AddLogMessage(Severity severity,
                               string message,
                               int lineNumber,
                               string sourcePath,
                               string memberName)
    {
        var sequence = Interlocked.Increment(ref _eventSequenceCounter);
        var logMessage = new LogMessage(sequence,
                                        severity,
                                        DateTime.Now,
                                        message,
                                        null,
                                        lineNumber,
                                        sourcePath,
                                        memberName);
        _messages.Add(logMessage);

    }

    private void AddLogMessage(Severity severity,
                              Exception exception,
                              int lineNumber,
                              string sourcePath,
                              string memberName)
    {
        var sequence = Interlocked.Increment(ref _eventSequenceCounter);
        var logMessage = new LogMessage(sequence,                            
                                        severity,
                                        DateTime.Now,
                                        null,
                                        exception,
                                        lineNumber,
                                        sourcePath,
                                        memberName);
        _messages.Add(logMessage);

    }

    #endregion
}

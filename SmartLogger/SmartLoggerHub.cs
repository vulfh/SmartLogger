using SmartLogger.Core.Exceptions;
using SmartLogger.Core.Infra;
using SmartLogger.Core.LogPersistance;
using System;
using System.Collections.Concurrent;


namespace SmartLogger.Core;

public class SmartLoggerHub : ILogAggregator, IDisposable
{

    private ConcurrentQueue<LogMessageContainer> _messages = new();
    
    private ConcurrentDictionary<string, NotifySubscriberCallback> _flushSubscribers = new();
    
    private ConcurrentQueue<FlushRequest> _flushRequests = new();

    private Configuration _configuration;

    private Mode _mode = Mode.AGGREGATE;

    private int _eventSequenceCounter = 0;

    private Severity _byPassLogSeverityLevel;

    #region Constructor

    public SmartLoggerHub()
    {
        _configuration  = new Configuration();
        SetInitialLogMode();
    }

    public SmartLoggerHub(Configuration configuration)
    {
        _configuration = configuration;
        SetInitialLogMode();
    }

    #endregion

    #region Properties

    public Mode Mode => _mode;
    #endregion

    #region ILogAggregator
    public void Flush(Severity severity = Severity.INFORMATION)
    {
        if (!_messages.IsEmpty)
        {
            AddStopFlushMarker(severity);
            FlushLogMessages();
        }
    }


    private void FlushTillStopMarker(Severity severityLevel)
    {
        while (_messages.TryDequeue(out var message)
              && !message.IsStopFlushMarker)
        {
            NotifySubscribersAccordingSeverityLevel(message.Message, severityLevel);
        }
    }

    public Task FlushAsync(Severity severity = Severity.INFORMATION)
    {
        if (!_messages.IsEmpty)
        {
            AddStopFlushMarker(severity);
            return Task.Factory.StartNew(() => FlushLogMessages());
        }
        else
        {
            return Task.CompletedTask;
        }
    }



    public void LogDebug(string message, int lineNumber, string memberName, string filePath)
    {
        AddLogMessage(Severity.DEBUG, message, lineNumber, memberName, filePath);
    }

    public void LogError(string message, int lineNumber, string memberName, string filePath)
    {
        AddLogMessage(Severity.ERROR, message, lineNumber, memberName, filePath);
    }

    public void LogError(Exception exception, int lineNumber, string memberName, string filePath)
    {
        AddLogMessage(Severity.ERROR, exception, lineNumber, memberName, filePath);
    }

    public void LogFatal(string message, int lineNumber, string memberName, string filePath)
    {
        AddLogMessage(Severity.FATAL, message, lineNumber, memberName, filePath);
    }

    public void LogFatal(Exception exception, int lineNumber, string memberName, string filePath)
    {
        AddLogMessage(Severity.FATAL, exception, lineNumber, memberName, filePath);
    }

    public void LogInformation(string message, int lineNumber, string memberName, string filePath)
    {
        AddLogMessage(Severity.INFORMATION, message, lineNumber, memberName, filePath);
    }

    public void LogWarning(string message, int lineNumber, string memberName, string filePath)
    {
        AddLogMessage(Severity.WARNING, message, lineNumber, memberName, filePath);
    }

    public void RegisterObserver(string name, NotifySubscriberCallback observer)
    {
       _flushSubscribers.AddOrUpdate(name, observer,(name,observer) => observer);
    }

    public bool UnregisterObserver(string name)
    {
        return _flushSubscribers.Remove(name, out _);
    }

    public void StartLogAggregation()
    {
        _mode = Mode.AGGREGATE;
    }
    public void StartByPassLogging(Severity severity)
    {
        _byPassLogSeverityLevel = severity;
        if (_mode == Mode.BYPASS)
            return;

        if (_messages.IsEmpty)
        {
            _mode = Mode.BYPASS;
        }
        else
        {
            throw new InvalidBypassLoggingRequest();
        }
    }
    #endregion

    #region IDosposable

    public void Dispose()
    {
        _messages.Clear();
        _flushSubscribers.Clear();
        _flushSubscribers.Clear();
    }

    #endregion

    #region Private Methods

    private void SetInitialLogMode() 
    {
        if(_configuration.Mode == Mode.AGGREGATE)
        {
            StartLogAggregation();
        }
        else
        {
            StartByPassLogging(_configuration.ByPassSeverityLevel);
        }
    }

    private void FlushLogMessages()
    {
        while (_flushRequests.TryDequeue(out var flushRequest))
        {
            FlushTillStopMarker(flushRequest.Severity);
        }
    }
    private void AddLogMessage(Severity severity,
                               string message,
                               int lineNumber,
                               string sourcePath,
                               string memberName)
    {


        var logMessage = new LogMessage(CurrentSequence(),
                                            severity,
                                            DateTime.Now,
                                            message,
                                            null,
                                            lineNumber,
                                            sourcePath,
                                            memberName);
        PublishLogMessage(logMessage);
       
    }

    private void AddLogMessage(Severity severity,
                              Exception exception,
                              int lineNumber,
                              string sourcePath,
                              string memberName)
    {
        var logMessage = new LogMessage(CurrentSequence(),
                                            severity,
                                            DateTime.Now,
                                            null,
                                            exception,
                                            lineNumber,
                                            sourcePath,
                                            memberName);
        PublishLogMessage(logMessage);

    }

    private void PublishLogMessage(LogMessage message)
    {
        if (_mode == Mode.AGGREGATE)
        {
            _messages.Enqueue(new LogMessageContainer(message));
        }
        else
        {
            NotifySubscribersAccordingSeverityLevel(message,_byPassLogSeverityLevel);
        }
    }

    private void NotifySubscribersAccordingSeverityLevel(LogMessage message,Severity severityLevel)
    {
        if (message.Serverity >= severityLevel)
        {
            NotifySubscribers(message);
        }
    }

    private void AddStopFlushMarker(Severity severity)
    {
        _messages.Enqueue(new LogMessageContainer(null, true));
        _flushRequests.Enqueue(new FlushRequest(severity));
    }
    private int CurrentSequence()
    {
        return Interlocked.Increment(ref _eventSequenceCounter);
    }

    private void NotifySubscribers(LogMessage logMessage) 
    {
        foreach(var subscriber in _flushSubscribers)
        {
            try
            {
                subscriber.Value.Invoke(logMessage);
            }
            finally { }
        }
    }

    #endregion
}

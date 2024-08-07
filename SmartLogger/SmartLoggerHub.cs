﻿using SmartLogger.Core.Infra;
using SmartLogger.Core.LogPersistance;
using System.Collections.Concurrent;


namespace SmartLogger.Core;

public class SmartLoggerHub : ILogAggregator, IDisposable
{
    #region Constants

    private const int AGGREGATE_MODE = 0;
    private const int FLUSH_MODE = 1;

    #endregion

    private ConcurrentQueue<LogMessageContainer> _messages = new();
    private ConcurrentDictionary<string, NotifySubscriberCallback> _flushSubscribers = new();
    private ConcurrentQueue<FlushRequest> _flushRequests = new();

    private Configuration _configuration;

    private int _mode = AGGREGATE_MODE;

    private int _eventSequenceCounter = 0;

    #region Constructor

    public SmartLoggerHub()
    {
        _configuration  = new Configuration();
    }

    public SmartLoggerHub(Configuration configuration)
    {
        _configuration = configuration;
    }

    #endregion

    #region ILogAggregator
    public void Flush(Severity severity = Severity.INFORMATION)
    {
        AddStopFlushMarker(severity);
        FlushLogMessages();
    }


    private void FlushTillStopMarker(Severity severity)
    {
        while (_messages.TryDequeue(out var message)
              && !message.IsStopFlushMarker)
        {
            if (message?.Message?.Serverity >= severity)
            {
                NotifySubscribers(message.Message);
            }
        }
    }

    public Task FlushAsync(Severity severity = Severity.INFORMATION)
    {
        AddStopFlushMarker(severity);
        return Task.Factory.StartNew(() => FlushLogMessages());
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

    private void FlushLogMessages()
    {
        if (Interlocked.CompareExchange(ref _mode, FLUSH_MODE, AGGREGATE_MODE) == AGGREGATE_MODE)
        {
            while (_flushRequests.TryDequeue(out var flushRequest))
            {
                FlushTillStopMarker(flushRequest.Severity);
            }

            Interlocked.CompareExchange(ref _mode, AGGREGATE_MODE, FLUSH_MODE);
        }
    }
    private void AddLogMessage(Severity severity,
                               string message,
                               int lineNumber,
                               string sourcePath,
                               string memberName)
    {
        DoActionOnlyInAggregateMode(() =>
        {
            var logMessage = new LogMessage(CurrentSequence(),
                                            severity,
                                            DateTime.Now,
                                            message,
                                            null,
                                            lineNumber,
                                            sourcePath,
                                            memberName);
            _messages.Enqueue(new LogMessageContainer(logMessage));
        });

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
        _messages.Enqueue(new LogMessageContainer(logMessage));

    }

    private void AddStopFlushMarker(Severity severity)
    {
        _messages.Enqueue(new LogMessageContainer(null, true));
        _flushRequests.Enqueue(new FlushRequest(severity));
    }

    private void DoActionOnlyInAggregateMode(Action action)
    {
        if (Interlocked.CompareExchange(ref _mode, _mode, _mode) == AGGREGATE_MODE)
        {
            action.Invoke();
        }
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

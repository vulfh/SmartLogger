using Moq;
using SmartLogger.Core;
using SmartLogger.Core.LogPersistance;
using System.Collections.Concurrent;
using System.Runtime.InteropServices;
using Xunit;

namespace SmartLoggerTests;

public class SmartLoggerTest
{
    private ILogAggregator objectUnderTest;
    private Mock<FlushLogSubscriberMock> flushLogSubscriberMock;

    public SmartLoggerTest() 
    { 
        objectUnderTest = new SmartLoggerHub();
        flushLogSubscriberMock = new Mock<FlushLogSubscriberMock>();
        
    }

    [Fact]
    public void Expect_SubscriberNotifiedWithError()
    {
        #region Arrange

        objectUnderTest.LogWarning("Warning");
        objectUnderTest.LogDebug("Debug");
        objectUnderTest.LogError("Error");
        objectUnderTest.LogInformation("Information");
        var message = Severity.FATAL;
        ManualResetEvent messageFlushed = new ManualResetEvent(false);
        flushLogSubscriberMock.Setup(mock => mock
                                    .NotifyLogMessage(It.IsAny<LogMessage>()))
                                    .Callback<LogMessage>((logMessage) => { message = logMessage.Serverity;
                                        messageFlushed.Set();
                                    });
        objectUnderTest.RegisterObserver("Test", flushLogSubscriberMock.Object.NotifyLogMessage);

        #endregion

        #region Act
        
        objectUnderTest.Flush(Severity.ERROR);


        #endregion

        #region Assert
        messageFlushed.WaitOne(1000);

        flushLogSubscriberMock.Verify(mock => mock.NotifyLogMessage(It.IsAny<LogMessage>()), Times.Once);
        Assert.That(message, Is.EqualTo(Severity.ERROR));

        #endregion

    }

    [Fact]
    public void Expect_SubscriberNotifiedTwice_WithError_WithWarning()
    {
        #region Arrange

        objectUnderTest.LogWarning("Warning");
        objectUnderTest.LogDebug("Debug");
        objectUnderTest.LogError("Error");
        objectUnderTest.LogInformation("Information");
        var messages = new Dictionary<Severity,int>();
        ManualResetEvent message1Flushed = new ManualResetEvent(false);
        ManualResetEvent message2Flushed = new ManualResetEvent(false);
        flushLogSubscriberMock.Setup(mock => mock
                                    .NotifyLogMessage(It.IsAny<LogMessage>()))
                                    .Callback<LogMessage>((logMessage) => {
                                        messages[logMessage.Serverity] = 1;
                                        if (logMessage.Serverity == Severity.ERROR)
                                        {
                                            message1Flushed.Set();
                                        }
                                        else if (logMessage.Serverity == Severity.WARNING)
                                        {
                                            message2Flushed.Set();
                                        }
                                    });
        objectUnderTest.RegisterObserver("Test", flushLogSubscriberMock.Object.NotifyLogMessage);

        #endregion

        #region Act

        objectUnderTest.Flush(Severity.WARNING);


        #endregion

        #region Assert
        ManualResetEvent.WaitAll(new[] { message1Flushed,message2Flushed },5000);

        flushLogSubscriberMock.Verify(mock => mock.NotifyLogMessage(It.IsAny<LogMessage>()), Times.Exactly(2));
        Assert.That(messages[Severity.WARNING], Is.EqualTo(1));
        Assert.That(messages[Severity.ERROR], Is.EqualTo(1));

        #endregion

    }

    [Fact]
    public async void Expect_SubscriberNotifiedTwice_WithError_WithWarning_And_One_More_Time_After_Second_Flush()
    {
        #region Arrange

        objectUnderTest.LogWarning("Warning");
        objectUnderTest.LogDebug("Debug");
        objectUnderTest.LogError("Error");
        objectUnderTest.LogInformation("Information");
        var messages = new Dictionary<Severity, int>();
        ManualResetEvent message1Flushed = new ManualResetEvent(false);
        ManualResetEvent message2Flushed = new ManualResetEvent(false);
        flushLogSubscriberMock.Setup(mock => mock
                                    .NotifyLogMessage(It.IsAny<LogMessage>()))
                                    .Callback<LogMessage>((logMessage) => {
                                        messages[logMessage.Serverity] = 1;
                                        if (logMessage.Serverity == Severity.ERROR)
                                        {
                                            message1Flushed.Set();
                                        }
                                        else if (logMessage.Serverity == Severity.WARNING)
                                        {
                                            message2Flushed.Set();
                                        }
                                    });
        objectUnderTest.RegisterObserver("Test", flushLogSubscriberMock.Object.NotifyLogMessage);

        #endregion

        #region Act

        var flushTask = objectUnderTest.FlushAsync(Severity.WARNING);

        objectUnderTest.LogWarning("Warning");
        objectUnderTest.LogDebug("Debug");
        objectUnderTest.LogError("Error");
        objectUnderTest.LogInformation("Information");

        await flushTask;



        #endregion

        #region Assert
        ManualResetEvent.WaitAll(new[] { message1Flushed, message2Flushed }, 5000);

        flushLogSubscriberMock.Verify(mock => mock.NotifyLogMessage(It.IsAny<LogMessage>()), Times.Exactly(2));
        Assert.That(messages[Severity.WARNING], Is.EqualTo(1));
        Assert.That(messages[Severity.ERROR], Is.EqualTo(1));

        #endregion

    }
}
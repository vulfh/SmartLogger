using SmartLogger.Core.LogPersistance;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLoggerTests;

public class FlushLogSubscriberMock
{
    public virtual void NotifyLogMessage(LogMessage logMessage){}
}

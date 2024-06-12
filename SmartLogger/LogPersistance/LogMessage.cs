namespace SmartLogger.Core.LogPersistance;

public record LogMessage(int Sequence,                     
                         Severity Serverity,
                         DateTime TimeStamp,
                         string Message,
                         Exception? exception,
                         int Line, 
                         string Source,
                         string Member);


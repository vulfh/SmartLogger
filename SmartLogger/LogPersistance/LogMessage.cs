namespace SmartLogger.LogPersistance;

public record LogMessage(int Sequence,                     
                         Severity serverity,
                         DateTime TimeStamp,
                         string Message,
                         Exception? exception,
                         int Line, 
                         string Source,
                         string Member);


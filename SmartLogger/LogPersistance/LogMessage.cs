namespace SmartLogger.LogPersistance;

public record LogMessage(Severity serverity,
                         DateTime TimeStamp,
                         string Message,
                         Exception exception,
                         int Line, 
                         string Source,
                         string Memeber);


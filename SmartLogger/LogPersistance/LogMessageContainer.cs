namespace SmartLogger.LogPersistance;

public record LogMessageContainer(LogMessage? Message = null,bool IsStopFlushMarker = false);

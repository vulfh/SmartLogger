namespace SmartLogger.Core.LogPersistance;

public record LogMessageContainer(LogMessage? Message = null,bool IsStopFlushMarker = false);

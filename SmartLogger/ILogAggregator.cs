using SmartLogger.Core.Infra;
using System.Runtime.CompilerServices;
namespace SmartLogger.Core;


public interface ILogAggregator
{

    void LogInformation(string message, 
                        [CallerLineNumber] int lineNumber = 0,
                        [CallerMemberName] string memberName = Constants.General.NotAssigned, 
                        [CallerFilePath] string filePath = Constants.General.NotAssigned);
    void LogDebug(string message,
                  [CallerLineNumber] int lineNumber = 0,
                  [CallerMemberName] string memberName = Constants.General.NotAssigned,
                  [CallerFilePath] string filePath = Constants.General.NotAssigned);
    void LogError(string message,
                  [CallerLineNumber] int lineNumber = 0,
                  [CallerMemberName] string memberName = Constants.General.NotAssigned,
                  [CallerFilePath] string filePath = Constants.General.NotAssigned);

    void LogError(Exception exception, 
                  [CallerLineNumber] int lineNumber = 0,
                  [CallerMemberName] string memberName = Constants.General.NotAssigned,
                  [CallerFilePath] string filePath = Constants.General.NotAssigned);
    void LogWarning(string message,
                    [CallerLineNumber] int lineNumber = 0,
                    [CallerMemberName] string memberName = Constants.General.NotAssigned,
                    [CallerFilePath] string filePath = Constants.General.NotAssigned);
    void LogFatal(string message,
                  [CallerLineNumber] int lineNumber = 0,
                  [CallerMemberName] string memberName = Constants.General.NotAssigned,
                  [CallerFilePath] string filePath = Constants.General.NotAssigned);  
    void LogFatal(Exception exception,
                  [CallerLineNumber] int lineNumber = 0,
                  [CallerMemberName] string memberName = Constants.General.NotAssigned,
                  [CallerFilePath] string filePath = Constants.General.NotAssigned);
    Task Flush(Severity severity = Severity.INFORMATION);

    void RegisterAggregatedLogger(string name, NotifyLoggerCallback observer);

    void RegisterImmediateLogger(string name, NotifyLoggerCallback observer);

    bool UnregisterAggregatedLogger(string name);
    bool UnregisterImmediateLogger(string name);

}

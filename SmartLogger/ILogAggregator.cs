using SmartLogger.Core.Infra;
using System.Runtime.CompilerServices;
namespace SmartLogger.Core;


public interface ILogAggregator
{

    Mode Mode { get; }
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
    void Flush(Severity severity = Severity.INFORMATION);
    Task FlushAsync(Severity severity = Severity.INFORMATION);

    void RegisterObserver(string name, NotifySubscriberCallback observer);

    bool UnregisterObserver(string name);

    void StartLogAggregation();
    void StartByPassLogging(Severity severity);
    

}

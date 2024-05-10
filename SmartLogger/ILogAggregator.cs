using SmartLogger.LogPersistance;
using System.Runtime.CompilerServices;
namespace SmartLogger
{

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
        void SetLogPersistanceSeverity(Severity severity);

        void Flush(Severity? severity);
        Task FlushAsync(Severity? severity);

        bool RegisterObserver(string name, Action<LogMessage> observer);

        bool UnregisterObserver(string name);

    }
}

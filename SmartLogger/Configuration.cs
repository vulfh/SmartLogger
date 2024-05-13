using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartLogger;

public class Configuration
{
    /// <summary>
    /// Format of TimeStamp in log message. If empty, timestamp will not be added to the message
    /// </summary>
    public string TimeStampFormat { get; private set; } = Constants.General.DefaultTimeStampFormat;

    /// <summary>
    /// Flag to indicate whether add the source line number, where logging method was called from
    /// </summary>
    public bool SourceLineNumber { get; private set; } = true;

    /// <summary>
    /// Flag to indicate whether add the method name, where logging method was called from
    /// </summary>
    public bool CallingMethodName { get; private set; } = true;

    /// <summary>
    /// Flag to indicate whether add the source file path, where logging method was called from
    /// </summary>
    public bool SourceFilePath { get; private set; } = true;

    /// <summary>
    /// Flag to indicate whether add log timestamp
    /// </summary>
    public bool LogTimeStamp { get; private set; } = true;


}

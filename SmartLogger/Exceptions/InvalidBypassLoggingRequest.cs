
namespace SmartLogger.Core.Exceptions;

public class InvalidBypassLoggingRequest:Exception
{
    public InvalidBypassLoggingRequest():base("Cannot start bypass whille aggregation is in progress"){}
}

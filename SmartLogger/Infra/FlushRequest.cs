namespace SmartLogger.Core.Infra;

internal record FlushRequest
{
    public DateTime TimeStamp { get; private set; } = DateTime.UtcNow;
    public Severity Severity { get; private init; }

    public FlushRequest(Severity severity) {  this.Severity = severity; }
}

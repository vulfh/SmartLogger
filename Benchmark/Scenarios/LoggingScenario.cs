using BenchmarkDotNet.Attributes;
using SmartLogger.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Benchmark.Scenarios;

[MaxIterationCount(10)]
[MinIterationCount(2)]
[ShortRunJob]
[MemoryDiagnoser]
public class LoggingScenario
{
    public ILogAggregator _logAggregator;

    [Params(10,100,1000)]
    public int MaxIterations { get; set; }
    public LoggingScenario()
    {
        _logAggregator = new SmartLoggerHub();
    }

    #region Benchmark

    [Benchmark]
    public void WriteLogs()
    {
        WriteLogs(MaxIterations);
    }



    #endregion

    public  void WriteLogs(int amount)
    {
        int amountOfLogsOfSingleSeverity = amount / 5;
        if(amountOfLogsOfSingleSeverity == 0)
        {
            amountOfLogsOfSingleSeverity = 1;
        }
        WriteLogOfSpecifiedSeverity(amount, Severity.DEBUG);
        WriteLogOfSpecifiedSeverity(amount, Severity.INFORMATION);
        WriteLogOfSpecifiedSeverity(amount, Severity.WARNING);
        WriteLogOfSpecifiedSeverity(amount, Severity.ERROR);
        WriteLogOfSpecifiedSeverity(amount, Severity.FATAL);

    }

    private void WriteLogOfSpecifiedSeverity(int amount, Severity severity)
    {
        for (int i = 0; i < amount; i++)
        {
            switch (severity)
            {
                case Severity.INFORMATION:
                    _logAggregator.LogInformation($"Information message {i}");
                    break;
                case Severity.FATAL:
                    _logAggregator.LogInformation($"Fatal message {i}");
                    break;
                case Severity.WARNING:
                    _logAggregator.LogInformation($"Warning message {i}");
                    break;
                case Severity.ERROR:
                    _logAggregator.LogInformation($"Error message {i}");
                    break;
                case Severity.DEBUG:
                    _logAggregator.LogInformation($"Debug message {i}");
                    break;
            }
        }
    }
    
}

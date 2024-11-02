// See https://aka.ms/new-console-template for more information
using Benchmark.Scenarios;
using BenchmarkDotNet.Running;

Console.WriteLine("Hello, World!");



var summary = BenchmarkRunner.Run<LoggingScenario>();


#if !DEBUG && !ANDROID && !IOS && !MACCATALYST && !WINDOWS
using MauiExcel;

BenchmarkDotNet.Running.BenchmarkRunner.Run([typeof(ExcelBenchmark), typeof(ExcelBenchmarkType), typeof(ExcelBenchmarkTypeLarge)]);
#endif
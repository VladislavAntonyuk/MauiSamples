using BenchmarkDotNet.Attributes;
using MauiExcel.Services;

namespace MauiExcel;

[MemoryDiagnoser]
public class ExcelBenchmark
{
	[Params(10, 1000)]
	public int N;

	public List<List<string>> Data = [];

	[GlobalSetup]
	public void GlobalSetup()
	{
		Data = [];
		for (var i = 0; i < N; i++)
		{
			var rows = new List<string>();
			for (var j = 0; j < N; j++)
			{
				rows.Add($"Item-{i}-{j}");
			}

			Data.Add(rows);
		}
	}

	[Benchmark]
	public void RunOpenXmlService()
	{
		using var stream = new FileStream(Path.GetRandomFileName(), FileMode.Create);
		OpenXmlService.Save(stream, Data);
	}

	[Benchmark]
	public void RunClosedXmlService()
	{
		var stream = new FileStream(Path.GetRandomFileName(), FileMode.Create);
		ClosedXmlService.Save(stream, Data);
	}

	[Benchmark(Baseline = true)]
	public void RunCustomExcelService()
	{
		var stream = new FileStream(Path.GetRandomFileName(), FileMode.Create);
		CustomExcelService.Save(stream, Data);
	}

	[Benchmark]
	public void RunMiniExcelService()
	{
		var stream = new FileStream(Path.GetRandomFileName(), FileMode.Create);
		MiniExcelService.Save(stream, Data);
	}
}
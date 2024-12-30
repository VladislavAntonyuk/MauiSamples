using BenchmarkDotNet.Attributes;
using MauiExcel.Services;

namespace MauiExcel;

[MemoryDiagnoser]
public class ExcelBenchmarkType
{
	[Params(100, 1000, 10000)]
	public int N;

	public List<Data> Data = [];

	[GlobalSetup]
	public void GlobalSetup()
	{
		Data = [];
		for (var i = 0; i < N; i++)
		{
			var data = new Data
			{
				Column1 = $"Item-{i}-1",
				Column2 = $"Item-{i}-2",
				Column3 = $"Item-{i}-3",
				Column4 = $"Item-{i}-4",
				Column5 = $"Item-{i}-5",
				Column6 = $"Item-{i}-6",
				Column7 = $"Item-{i}-7",
				Column8 = $"Item-{i}-8",
				Column9 = $"Item-{i}-9",
				Column10 = $"Item-{i}-10"
			};

			Data.Add(data);
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
		using var stream = new FileStream(Path.GetRandomFileName(), FileMode.Create);
		ClosedXmlService.Save(stream, Data);
	}

	[Benchmark(Baseline = true)]
	public void RunCustomExcelService()
	{
		using var stream = new FileStream(Path.GetRandomFileName(), FileMode.Create);
		CustomExcelService.Save(stream, Data);
	}

	[Benchmark]
	public void RunMiniExcelService()
	{
		using var stream = new FileStream(Path.GetRandomFileName(), FileMode.Create);
		MiniExcelService.Save(stream, Data);
	}
}
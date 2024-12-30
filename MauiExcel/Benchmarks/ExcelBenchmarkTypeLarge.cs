using BenchmarkDotNet.Attributes;
using MauiExcel.Services;

namespace MauiExcel;

[MemoryDiagnoser]
[IterationCount(5)]
public class ExcelBenchmarkTypeLarge
{
	public List<Data> DataType = [];

	public List<List<string>> Data = [];

	[GlobalSetup]
	public void GlobalSetup()
	{
		Data = [];
		for (var i = 0; i < 10_000; i++)
		{
			var rows = new List<string>();
			for (var j = 0; j < 10_000; j++)
			{
				rows.Add($"Item-{i}-{j}");
			}

			Data.Add(rows);
		}

		DataType = [];
		for (var i = 0; i < 1_000_000; i++)
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

			DataType.Add(data);
		}
	}

	[Benchmark]
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

	[Benchmark]
	public void RunCustomExcelServiceType()
	{
		using var stream = new FileStream(Path.GetRandomFileName(), FileMode.Create);
		CustomExcelService.Save(stream, DataType);
	}

	[Benchmark]
	public void RunMiniExcelServiceType()
	{
		using var stream = new FileStream(Path.GetRandomFileName(), FileMode.Create);
		MiniExcelService.Save(stream, DataType);
	}
}
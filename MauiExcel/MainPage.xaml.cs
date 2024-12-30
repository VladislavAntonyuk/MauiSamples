namespace MauiExcel;

using Services;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	private void OnExportClicked(object sender, EventArgs e)
	{
		var data = new List<List<string>>();

		for (var i = 0; i < 10; i++)
		{
			var rows = new List<string>();
			for (var j = 0; j < 10; j++)
			{
				rows.Add($"Item-{i}-{j}");
			}

			data.Add(rows);
		}

		using var closedXmlFileStream = new FileStream("closedXml.xlsx", FileMode.Create);
		ClosedXmlService.Save(closedXmlFileStream, data);

		using var customExcelFileStream = new FileStream("customExcel.xlsx", FileMode.Create);
		CustomExcelService.Save(customExcelFileStream, data);

		using var openExcelFileStream = new FileStream("openXml.xlsx", FileMode.Create);
		OpenXmlService.Save(openExcelFileStream, data);

		using var miniExcelFileStream = new FileStream("miniExcel.xlsx", FileMode.Create);
		MiniExcelService.Save(miniExcelFileStream, data);

		var data2 = new List<Data>();

		for (var i = 0; i < 10; i++)
		{
			var item = new Data
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

			data2.Add(item);
		}

		using var closedXmlFileStream2 = new FileStream("closedXml2.xlsx", FileMode.Create);
		ClosedXmlService.Save(closedXmlFileStream2, data2);

		using var customExcelFileStream2 = new FileStream("customExcel2.xlsx", FileMode.Create);
		CustomExcelService.Save(customExcelFileStream2, data2);

		using var openExcelFileStream2 = new FileStream("openXml2.xlsx", FileMode.Create);
		OpenXmlService.Save(openExcelFileStream2, data2);

		using var miniExcelFileStream2 = new FileStream("miniExcel2.xlsx", FileMode.Create);
		MiniExcelService.Save(miniExcelFileStream2, data2);
	}
}


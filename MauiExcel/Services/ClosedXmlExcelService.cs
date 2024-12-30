using ClosedXML.Excel;

namespace MauiExcel.Services;

internal class ClosedXmlService
{
	public static void Save(Stream stream, IEnumerable<IEnumerable<string>> data)
	{
		using var workbook = new XLWorkbook();
		var worksheet = workbook.Worksheets.Add("Sample Sheet");
		var rowNumber = 0;
		foreach (var row in data)
		{
			rowNumber++;
			var columnNumber = 0;
			foreach (var value in row)
			{
				columnNumber++;
				worksheet.Cell(rowNumber, columnNumber).Value = value;
			}
		}

		workbook.SaveAs(stream);
	}

	public static void Save(Stream stream, IEnumerable<Data> data)
	{
		using var workbook = new XLWorkbook();
		var worksheet = workbook.Worksheets.Add("Sample Sheet");
		var rowNumber = 0;
		foreach (var row in data)
		{
			rowNumber++;
			worksheet.Cell(rowNumber, 1).Value = row.Column1;
			worksheet.Cell(rowNumber, 2).Value = row.Column2;
			worksheet.Cell(rowNumber, 3).Value = row.Column3;
			worksheet.Cell(rowNumber, 4).Value = row.Column4;
			worksheet.Cell(rowNumber, 5).Value = row.Column5;
			worksheet.Cell(rowNumber, 6).Value = row.Column6;
			worksheet.Cell(rowNumber, 7).Value = row.Column7;
			worksheet.Cell(rowNumber, 8).Value = row.Column8;
			worksheet.Cell(rowNumber, 9).Value = row.Column9;
			worksheet.Cell(rowNumber, 10).Value = row.Column10;
		}

		workbook.SaveAs(stream);
	}
}
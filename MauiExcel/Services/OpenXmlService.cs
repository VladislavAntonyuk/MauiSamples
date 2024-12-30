using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml;

namespace MauiExcel.Services;

internal class OpenXmlService
{
	public static void Save(Stream stream, IEnumerable<IEnumerable<string>> rows)
	{
		using var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);
		var wbPart = document.AddWorkbookPart();
		wbPart.Workbook = new Workbook();
		var part = wbPart.AddNewPart<WorksheetPart>();
		var sheetData = new SheetData();
		part.Worksheet = new Worksheet(sheetData);
		wbPart.Workbook.AppendChild(
			new Sheets(
				new Sheet
				{
					Id = wbPart.GetIdOfPart(part),
					SheetId = 1,
					Name = "SheetName",
				}));

		foreach (var value in rows)
		{
			var dataRow = sheetData.AppendChild(new Row());
			foreach (var dataElement in value)
			{
				dataRow.Append(ConstructCell(dataElement, CellValues.String));
			}
		}

		wbPart.Workbook.Save();
	}

	public static void Save(Stream stream, IEnumerable<Data> rows)
	{
		using var document = SpreadsheetDocument.Create(stream, SpreadsheetDocumentType.Workbook);
		var wbPart = document.AddWorkbookPart();
		wbPart.Workbook = new Workbook();
		var part = wbPart.AddNewPart<WorksheetPart>();
		var sheetData = new SheetData();
		part.Worksheet = new Worksheet(sheetData);
		wbPart.Workbook.AppendChild(
			new Sheets(
				new Sheet
				{
					Id = wbPart.GetIdOfPart(part),
					SheetId = 1,
					Name = "SheetName",
				}));

		foreach (var value in rows)
		{
			var dataRow = sheetData.AppendChild(new Row());
			dataRow.Append(ConstructCell(value.Column1, CellValues.String));
			dataRow.Append(ConstructCell(value.Column2, CellValues.String));
			dataRow.Append(ConstructCell(value.Column3, CellValues.String));
			dataRow.Append(ConstructCell(value.Column4, CellValues.String));
			dataRow.Append(ConstructCell(value.Column5, CellValues.String));
			dataRow.Append(ConstructCell(value.Column6, CellValues.String));
			dataRow.Append(ConstructCell(value.Column7, CellValues.String));
			dataRow.Append(ConstructCell(value.Column8, CellValues.String));
			dataRow.Append(ConstructCell(value.Column9, CellValues.String));
			dataRow.Append(ConstructCell(value.Column10, CellValues.String));
		}

		wbPart.Workbook.Save();
	}

	private static DocumentFormat.OpenXml.Spreadsheet.Cell ConstructCell(string value, CellValues dataTypes) => new() { CellValue = new CellValue(value), DataType = new EnumValue<CellValues>(dataTypes) };
}
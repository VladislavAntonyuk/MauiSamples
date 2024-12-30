using System.IO.Compression;
using System.Xml;

namespace MauiExcel.Services;

public static class CustomExcelService
{
	private const string Worksheet = "worksheet";
	private const string WorksheetNameSpace = "http://schemas.openxmlformats.org/spreadsheetml/2006/main";
	private const string SheetData = "sheetData";
	private const string Row = "row";
	private const string Cell = "c";
	private const string CellType = "t";
	private const string CellTypeStr = "str";
	private const string Value = "v";

	private const string ContentTypesEntryXml = """
                                           <Types xmlns="http://schemas.openxmlformats.org/package/2006/content-types">
                                           <Default Extension="xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.sheet.main+xml" />
                                           <Default Extension="rels" ContentType="application/vnd.openxmlformats-package.relationships+xml" />
                                           <Override PartName="/xl/worksheets/sheet1.xml" ContentType="application/vnd.openxmlformats-officedocument.spreadsheetml.worksheet+xml" />
                                           </Types>
                                           """;

	private const string RelsEntryXml = """
                                        <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
                                        <Relationship Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/officeDocument" Target="/xl/workbook.xml" Id="Re9a0e62ee0d84b49" />
                                        </Relationships>
                                        """;

	private const string WorkbookEntryXml = """
                                            <workbook xmlns="http://schemas.openxmlformats.org/spreadsheetml/2006/main">
                                            <sheets><sheet name="Errors" sheetId="1" r:id="SheetId" xmlns:r="http://schemas.openxmlformats.org/officeDocument/2006/relationships" /></sheets>
                                            </workbook>
                                            """;

	private const string WorkbookRelsEntryXml = """
                                                <Relationships xmlns="http://schemas.openxmlformats.org/package/2006/relationships">
                                                <Relationship Type="http://schemas.openxmlformats.org/officeDocument/2006/relationships/worksheet" Target="/xl/worksheets/sheet1.xml" Id="SheetId" />
                                                </Relationships>
                                                """;

	private const string SheetEntryFileName = "xl/worksheets/sheet1.xml";
	private const string ContentTypesEntryFileName = "[Content_Types].xml";
	private const string RelsEntryFileName = "_rels/.rels";
	private const string WorkBookEntryFileName = "xl/workbook.xml";
	private const string WorkbookRelsEntryFileName = "xl/_rels/workbook.xml.rels";

	public static void Save(Stream stream, IEnumerable<IEnumerable<string?>> rows)
	{
		using var zipArchive = new ZipArchive(stream, ZipArchiveMode.Create, true);
		CreateRelatedEntry(zipArchive, ContentTypesEntryFileName, ContentTypesEntryXml);
		CreateRelatedEntry(zipArchive, RelsEntryFileName, RelsEntryXml);
		CreateRelatedEntry(zipArchive, WorkBookEntryFileName, WorkbookEntryXml);
		CreateRelatedEntry(zipArchive, WorkbookRelsEntryFileName, WorkbookRelsEntryXml);

		CreateSheetData(zipArchive, rows);
	}

	private static void CreateSheetData(ZipArchive zipArchive, IEnumerable<IEnumerable<string?>> rows)
	{
		using var sheetStream = zipArchive.CreateEntry(SheetEntryFileName).Open();
		using var writer = XmlWriter.Create(sheetStream);
		writer.WriteStartElement(Worksheet, WorksheetNameSpace);
		writer.WriteStartElement(SheetData);

		foreach (var row in rows)
		{
			WriteRowValues(writer, row);
		}

		writer.WriteEndElement();
		writer.WriteEndElement();
	}

	private static void WriteRowValues(XmlWriter writer, IEnumerable<string?> values)
	{
		writer.WriteStartElement(Row);

		foreach (var value in values)
		{
			WriteCellValue(writer, value);
		}

		writer.WriteEndElement();
	}

	private static void WriteCellValue(XmlWriter writer, string? value)
	{
		writer.WriteStartElement(Cell);
		writer.WriteAttributeString(CellType, CellTypeStr);
		writer.WriteStartElement(Value);

		if (value is not null)
		{
			writer.WriteString(value);
		}

		writer.WriteEndElement();
		writer.WriteEndElement();
	}

	private static void CreateRelatedEntry(ZipArchive zipArchive, string entryName, string xmlData)
	{
		using var xmlStream = zipArchive.CreateEntry(entryName).Open();
		using var writer = XmlWriter.Create(xmlStream);
		writer.WriteRaw(xmlData);
	}

	public static void Save(Stream stream, IEnumerable<Data> rows)
	{
		using var zipArchive = new ZipArchive(stream, ZipArchiveMode.Create, true);
		CreateRelatedEntry(zipArchive, ContentTypesEntryFileName, ContentTypesEntryXml);
		CreateRelatedEntry(zipArchive, RelsEntryFileName, RelsEntryXml);
		CreateRelatedEntry(zipArchive, WorkBookEntryFileName, WorkbookEntryXml);
		CreateRelatedEntry(zipArchive, WorkbookRelsEntryFileName, WorkbookRelsEntryXml);

		CreateSheetData(zipArchive, rows);
	}

	private static void CreateSheetData(ZipArchive zipArchive, IEnumerable<Data> rows)
	{
		using var sheetStream = zipArchive.CreateEntry(SheetEntryFileName).Open();
		using var writer = XmlWriter.Create(sheetStream);
		writer.WriteStartElement(Worksheet, WorksheetNameSpace);
		writer.WriteStartElement(SheetData);

		foreach (var row in rows)
		{
			WriteRowValues(writer, row);
		}

		writer.WriteEndElement();
		writer.WriteEndElement();
	}

	private static void WriteRowValues(XmlWriter writer, Data data)
	{
		writer.WriteStartElement(Row);

		WriteCellValue(writer, data.Column1);
		WriteCellValue(writer, data.Column2);
		WriteCellValue(writer, data.Column3);
		WriteCellValue(writer, data.Column4);
		WriteCellValue(writer, data.Column5);
		WriteCellValue(writer, data.Column6);
		WriteCellValue(writer, data.Column7);
		WriteCellValue(writer, data.Column8);
		WriteCellValue(writer, data.Column9);
		WriteCellValue(writer, data.Column10);

		writer.WriteEndElement();
	}
}
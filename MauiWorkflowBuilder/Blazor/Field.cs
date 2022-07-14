namespace MauiWorkflowBuilder.Blazor;

using System.Xml.Serialization;

[XmlRoot(ElementName = "field")]
public class Field
{
	[XmlAttribute(AttributeName = "name")]
	public string? Name { get; set; }
	[XmlText]
	public string? Text { get; set; }
}
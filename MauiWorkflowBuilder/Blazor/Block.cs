namespace MauiWorkflowBuilder.Blazor;

using System.Xml.Serialization;

[XmlRoot(ElementName = "block")]
public class Block
{
	[XmlAttribute(AttributeName = "type")]
	public string? Type { get; set; }
	[XmlElement(ElementName = "value")]
	public List<Value>? Value { get; set; }
	[XmlElement(ElementName = "field")]
	public Field? Field { get; set; }
}
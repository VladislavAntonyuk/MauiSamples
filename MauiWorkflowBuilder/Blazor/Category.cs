namespace MauiWorkflowBuilder.Blazor;

using System.Xml.Serialization;

[XmlRoot(ElementName = "category")]
public class Category
{
	[XmlElement(ElementName = "block")]
	public List<Block>? Block { get; set; }
	[XmlAttribute(AttributeName = "name")]
	public string? Name { get; set; }
	[XmlAttribute(AttributeName = "colour")]
	public string? Colour { get; set; }
	[XmlAttribute(AttributeName = "custom")]
	public string? Custom { get; set; }
}
namespace MauiWorkflowBuilder.Blazor;

using System.Xml.Serialization;

[XmlRoot(ElementName = "value")]
public class Value
{
	[XmlElement(ElementName = "shadow")]
	public Shadow? Shadow { get; set; }
	[XmlAttribute(AttributeName = "name")]
	public string? Name { get; set; }
	[XmlElement(ElementName = "block")]
	public Block? Block { get; set; }
}
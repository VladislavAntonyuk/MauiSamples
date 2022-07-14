namespace MauiWorkflowBuilder.Blazor;

using System.Xml.Serialization;

[XmlRoot(ElementName = "shadow")]
public class Shadow
{
	[XmlElement(ElementName = "field")]
	public Field? Field { get; set; }
	[XmlAttribute(AttributeName = "type")]
	public string? Type { get; set; }
}
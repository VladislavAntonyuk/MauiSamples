namespace MauiWorkflowBuilder.Blazor;

using System.Xml.Serialization;

[XmlRoot(ElementName = "xml")]
public class Xml
{
	[XmlElement(ElementName = "category")]
	public List<Category>? Category { get; set; }
}
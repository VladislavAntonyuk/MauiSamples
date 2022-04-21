namespace MauiPaint.Serializer;

using System.Text.Json;
using System.Text.Json.Serialization;

public class BrushConverter : JsonConverter<Brush>
{
	public override Brush? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return Color.FromRgba(reader.GetString());
	}

	public override void Write(Utf8JsonWriter writer, Brush value, JsonSerializerOptions options)
	{
		if (value is SolidColorBrush brush)
		{
			writer.WriteStringValue(brush.Color.ToRgbaHex(true));
		}
	}
}
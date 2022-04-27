namespace MauiPaint.Serializer;

using System.Text.Json;
using System.Text.Json.Serialization;

public class ColorConverter : JsonConverter<Color>
{
	public override Color? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		return Color.FromRgba(reader.GetString());
	}

	public override void Write(Utf8JsonWriter writer, Color value, JsonSerializerOptions options)
	{
		writer.WriteStringValue(value.ToRgbaHex(true));
	}
}
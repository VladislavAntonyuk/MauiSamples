namespace MauiPaint.Serializer;

using System.Text.Json;
using System.Threading.Tasks;
using CommunityToolkit.Maui.Core;
using Figures;

public class JsonSerializerService : ISerializerService
{
	readonly JsonSerializerOptions options = new()
	{
		Converters =
		{
			new ColorConverter(),
			new BrushConverter(),
			new InterfaceConverter<IFigure>(),
			new InterfaceConverter<IDrawingLine>(),
		}
	};

	public async Task<Stream> Serialize<T>(T @object, CancellationToken cancellationToken)
	{
		var stream = new MemoryStream();
		await JsonSerializer.SerializeAsync(stream, @object, options, cancellationToken);
		return stream;
	}

	public ValueTask<T?> Deserialize<T>(Stream stream, CancellationToken cancellationToken)
	{
		return JsonSerializer.DeserializeAsync<T>(stream, options, cancellationToken);
	}
}
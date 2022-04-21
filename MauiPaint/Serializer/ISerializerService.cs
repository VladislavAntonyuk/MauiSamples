namespace MauiPaint.Serializer;

public interface ISerializerService
{
	Task<Stream> Serialize<T>(T @object, CancellationToken cancellationToken);
	ValueTask<T?> Deserialize<T>(Stream stream, CancellationToken cancellationToken);
}
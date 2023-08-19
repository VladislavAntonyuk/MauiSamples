#if WINDOWS
namespace MauiPaint;

using CommunityToolkit.Mvvm.Messaging.Messages;

public class DropItemMessage : ValueChangedMessage<Stream>
{
	public DropItemMessage(Stream value) : base(value)
	{
	}
}
#endif
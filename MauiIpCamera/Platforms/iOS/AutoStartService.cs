namespace MauiIpCamera;

public class AutoStartService : IAutoStartService
{
	public Task<bool> IsAutoStartEnabledAsync()
	{
		return Task.FromResult(false);
	}

	public Task<bool> EnableAutoStartAsync()
	{
		return Task.FromResult(false);
	}

	public Task<bool> DisableAutoStartAsync()
	{
		return Task.FromResult(false);
	}
}
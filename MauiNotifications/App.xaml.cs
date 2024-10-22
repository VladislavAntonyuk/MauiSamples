namespace MauiNotifications;

public partial class App
{
	private readonly DeviceInstallationService deviceInstallationService;

	public App(DeviceInstallationService deviceInstallationService)
	{
		this.deviceInstallationService = deviceInstallationService;
		InitializeComponent();
	}

	protected override Window CreateWindow(IActivationState? activationState)
	{
		return new Window(new AppShell());
	}

	protected override async void OnStart()
	{
		base.OnStart();
		await deviceInstallationService.RegisterDevice();
	}
}
namespace MauiNotifications;

public partial class App
{
	private readonly DeviceInstallationService deviceInstallationService;

	public App(DeviceInstallationService deviceInstallationService)
	{
		this.deviceInstallationService = deviceInstallationService;
		InitializeComponent();

		MainPage = new AppShell();
	}

	protected override async void OnStart()
	{
		base.OnStart();
		await deviceInstallationService.RegisterDevice();
	}
}
namespace MauiWorkflowBuilder;

using Blazor;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.JSInterop;

public partial class MainPage : ContentPage, IDisposable
{
	public MainPage()
	{
		InitializeComponent();
		WeakReferenceMessenger.Default.Register<ResultWorkflowMessage>(this, (o, result) =>
		{
			var popup = new Popup()
			{
				Content = new Label()
				{
					Text = result.Result?.ToString(),
					VerticalTextAlignment = TextAlignment.Center,
					HorizontalTextAlignment = TextAlignment.Center,
					FontSize = 20
				}
			};
			this.ShowPopup(popup);
		});
	}

	private void Run(object sender, EventArgs e)
	{
		WeakReferenceMessenger.Default.Send(new RunWorkflowMessage());
	}

	public void Dispose()
	{
		WeakReferenceMessenger.Default.Unregister<ResultWorkflowMessage>(this);
	}
}
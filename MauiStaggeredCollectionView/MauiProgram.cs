namespace MauiStaggeredCollectionView;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureMauiHandlers(c =>
			{
				c.AddHandler<CollectionView, StaggeredStructuredItemsViewHandler>();
			});

		return builder.Build();
	}
}
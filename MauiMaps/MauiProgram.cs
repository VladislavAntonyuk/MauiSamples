namespace MauiMaps;

public static class MauiProgram {
  public static MauiApp CreateMauiApp() {
    var builder = MauiApp.CreateBuilder();
    builder.UseMauiApp<App>().UseMauiMaps();
    builder.ConfigureMauiHandlers(handlers => {
#if IOS || MACCATALYST
      handlers.AddHandler<CustomPin, CustomPinHandler>();
#endif
    });

    return builder.Build();
  }
}

﻿using CommunityToolkit.Maui;

namespace MauiMultiWindow;
public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder.UseMauiApp<App>().UseMauiCommunityToolkit();
        return builder.Build();
    }
}
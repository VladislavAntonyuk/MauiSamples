﻿namespace MauiMultiWindow;

public static partial class WindowExtensions
{
	public static Task<T?> OpenModalWindowAsync<T>(this Window parentWindow, ModalWindow<T> content)
	{
		return Task.FromResult<T?>(default);
	}
}
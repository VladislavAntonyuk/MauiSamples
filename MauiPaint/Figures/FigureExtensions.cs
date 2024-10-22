namespace MauiPaint.Figures;

public static class FigureExtensions
{
	public static async Task<T?> SetParameter<T>(string parameterName)
	{
		var page = Application.Current?.Windows.LastOrDefault()?.Page;
		if (page is null)
		{
			return default;
		}

		T? result = default;
		bool isValid;
		do
		{
			var value = await page.DisplayPromptAsync("Set parameter", parameterName);
			try
			{
				result = (T)Convert.ChangeType(value, typeof(T));
				isValid = true;
			}
			catch
			{
				isValid = false;
			}
		} while (!isValid);

		return result;
	}
}
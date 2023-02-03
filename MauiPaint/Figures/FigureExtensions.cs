namespace MauiPaint.Figures;

public static class FigureExtensions
{
	public static async Task<T?> SetParameter<T>(string parameterName)
	{
		if (Application.Current?.MainPage is null)
		{
			return default;
		}

		T? result = default;
		bool isValid;
		do
		{
			var value = await Application.Current.MainPage.DisplayPromptAsync("Set parameter", parameterName);
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
namespace KanbanBoard;

public class DbPath : IPath
{
	public string GetDatabasePath(string filename)
	{
		return Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), filename);
	}

	public void DeleteFile(string path)
	{
		if (File.Exists(path))
		{
			File.Delete(path);
		}
	}
}
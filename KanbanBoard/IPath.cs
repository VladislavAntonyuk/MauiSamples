namespace KanbanBoard;

public interface IPath
{
	string GetDatabasePath(string filename = "KanbanBoard.db");

	void DeleteFile(string path);
}
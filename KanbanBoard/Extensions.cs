using System.Collections.ObjectModel;

namespace KanbanBoard;

public static class Extensions
{
    public static ObservableCollection<T> ToObservableCollection<T>(this IEnumerable<T> col)
    {
        return new(col);
    }
}

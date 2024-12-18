namespace KanbanBoard.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using KanbanBoardDb;

public partial class CardInfo(KanbanCard kanbanCard) : ObservableObject
{
	[ObservableProperty]
	public partial bool IsDragOver { get; set; }

	public KanbanCard KanbanCard { get; } = kanbanCard;
}
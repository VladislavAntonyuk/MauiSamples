namespace KanbanBoard.Models;

using CommunityToolkit.Mvvm.ComponentModel;
using KanbanBoardDb;

public partial class CardInfo(KanbanCard kanbanCard) : ObservableObject
{
	[ObservableProperty]
	private bool isDragOver;

	public KanbanCard KanbanCard { get; } = kanbanCard;
}
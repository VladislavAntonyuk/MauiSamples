namespace KanbanBoard.Models;

using CommunityToolkit.Mvvm.ComponentModel;

public partial class CardInfo(Card card) : ObservableObject
{
	[ObservableProperty]
	private bool isDragOver;

	public Card Card { get; } = card;
}
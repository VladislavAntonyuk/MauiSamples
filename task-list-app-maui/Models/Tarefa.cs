using System.ComponentModel.DataAnnotations;
using CommunityToolkit.Mvvm.ComponentModel;

namespace task_list_app_maui.Models;

public class Tarefa : ObservableValidator
{
    private int id;
    private string descricao = string.Empty;
    private DateTime data;
    private TimeSpan hora;
    private bool concluida;

    [Key]
    [Required]
    public int Id { get => id; set => SetProperty(ref id, value, true); }

    [Required]
    [MaxLength(60)]
    public string Descricao { get => descricao; set => SetProperty(ref descricao, value, true); }

    [Required]
    public DateTime Data { get => data; set => SetProperty(ref data, value, true); }

    [Required]
    public TimeSpan Hora { get => hora; set => SetProperty(ref hora, value, true); }

    [Required]
    public bool Concluida { get => concluida; set => SetProperty(ref concluida, value, true); }
}

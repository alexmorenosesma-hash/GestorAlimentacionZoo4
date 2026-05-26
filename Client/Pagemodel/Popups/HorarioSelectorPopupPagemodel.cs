using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using System.Collections.ObjectModel;

namespace Client.Pagemodel.Popups;

public partial class HorarioSelectorPopupPagemodel : ObservableObject
{
    private readonly IPopupService _popup;

    public HorarioSelectorPopupPagemodel(IPopupService popup)
    {
        _popup = popup;
    }

    [ObservableProperty]
    private ObservableCollection<HorarioItem> horarios = new();
    public void Inicializar(List<Horario> lista, List<string>? horariosSeleccionados =  null)
    {
        horariosSeleccionados ??= new List<string>();

        Horarios = new ObservableCollection<HorarioItem>(
            lista.Select(h => new HorarioItem
            {
                Id = h.idHorario,
                Nombre = h.hora,
                Seleccionado = horariosSeleccionados.Contains(h.idHorario)
            })
        );
    }

    [RelayCommand]
    private async Task Confirm()
    {
        var seleccionados = Horarios
            .Where(h => h.Seleccionado)
            .Select(h => new Horario
            {
                idHorario = h.Id,
                hora = h.Nombre
            })
            .ToList();

        var resultado = new HorariosSeleccionados
        {
            Horarios = seleccionados
        };

        await _popup.ClosePopupAsync(Shell.Current, resultado);
    }

    [RelayCommand]
    private async Task Cancel()
    {
        await _popup.ClosePopupAsync(Shell.Current);
    }
}

public partial class HorarioItem : ObservableObject
{
    public string Id { get; set; }
    public string Nombre { get; set; }

    [ObservableProperty]
    private bool _seleccionado;
}

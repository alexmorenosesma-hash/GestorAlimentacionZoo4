using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using System.Collections.ObjectModel;

namespace Client.Pagemodel.Popups;

public partial class EnfermedadSelectorPopupPagemodel : ObservableObject
{
    private readonly IPopupService _popup;

    public EnfermedadSelectorPopupPagemodel(IPopupService popup)
    {
        _popup = popup;
    }

    [ObservableProperty]
    private ObservableCollection<EnfermedadItem> enfermedades = new();
    public void Inicializar(List<Enfermedad> lista, List<string>? enfermedadesSeleccionadas = null)
    {
        enfermedadesSeleccionadas ??= new List<string>();

        Enfermedades = new ObservableCollection<EnfermedadItem>(
            lista.Select(e => new EnfermedadItem
            {
                Id = e.idEnfermedad,
                Nombre = e.nombre,
                Seleccionado = enfermedadesSeleccionadas.Contains(e.idEnfermedad)
            })
        );
    }

    [RelayCommand]
    private async Task Confirm()
    {
        var seleccionados = Enfermedades
            .Where(e => e.Seleccionado)
            .Select(e => new Enfermedad

            {
                idEnfermedad = e.Id,
                nombre = e.Nombre
            })
            .ToList();

        var resultado = new EnfermedadSeleccionadas
        {
            Enfermedades = seleccionados
        };

        await _popup.ClosePopupAsync(Shell.Current, resultado);
    }

    [RelayCommand]
    private async Task Cancel()
    {
        await _popup.ClosePopupAsync(Shell.Current);
    }
}

public partial class EnfermedadItem : ObservableObject
{
    public string Id { get; set; }
    public string Nombre { get; set; }

    [ObservableProperty]
    private bool _seleccionado;
}

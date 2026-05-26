using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using System.Collections.ObjectModel;

namespace Client.Pagemodel.Popups;

public partial class AlimentoSelectorPopupPagemodel : ObservableObject
{
    private readonly IPopupService _popup;

    public AlimentoSelectorPopupPagemodel(IPopupService popup)
    {
        _popup = popup;
    }

    [ObservableProperty]
    private ObservableCollection<AlimentoItem> _alimentos = new();

    public void Inicializar(List<Alimento> disponibles, List<AlimentoCantidad>? seleccionados = null)
    {
        seleccionados ??= new List<AlimentoCantidad>();

        Alimentos = new ObservableCollection<AlimentoItem>(
            disponibles.Select(a =>
            {
                var sel = seleccionados.FirstOrDefault(s => s.Id == a.idAlimento);

                return new AlimentoItem
                {
                    Id = a.idAlimento,
                    Nombre = a.nombre,
                    Unidad = a.unidad,
                    Seleccionado = sel != null,
                    Cantidad = sel?.Cantidad ?? 0
                };
            })
        );
    }

    [RelayCommand]
    private async Task Confirm()
    {
        var seleccionados = Alimentos
            .Where(a => a.Seleccionado)
            .Select(a => new AlimentoCantidad
            {
                Id = a.Id,
                Nombre = a.Nombre,
                Cantidad = a.Cantidad
            })
            .ToList();

        await _popup.ClosePopupAsync(Shell.Current, seleccionados);
    }

    // ⭐ Cancelar
    [RelayCommand]
    private async Task Cancel()
    {
        await _popup.ClosePopupAsync(Shell.Current);
    }
}

public partial class AlimentoItem : ObservableObject
{
    public string Id { get; set; }
    public string Nombre { get; set; }
    public string Unidad { get; set; }
    [ObservableProperty]
    private bool _seleccionado;

    [ObservableProperty]
    private int _cantidad;
}

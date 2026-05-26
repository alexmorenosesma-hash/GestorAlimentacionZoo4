using Aplication.Interfaces.Repositories;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Client.Pagemodel.Popups;

public partial class AlimentoModificarPopupPagemodel : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<string> _unidades = new ObservableCollection<string> { "Kg", "g" };

    [ObservableProperty]
    Alimento model = new();

    IPopupService _popup;
    public AlimentoModificarPopupPagemodel(IPopupService popup)
    {
        _popup = popup;
    }

    private bool EsFormularioValido()
    {
        return
            !string.IsNullOrWhiteSpace(Model.nombre) &&
            Model.cantidad > 0 &&
            !string.IsNullOrWhiteSpace(Model.unidad);
    }

    [RelayCommand]
    async Task Save()
    {
        if (!EsFormularioValido())
        {
            await Shell.Current.DisplayAlertAsync("Campos incompletos","Debes rellenar nombre, cantidad y unidad antes de guardar.","OK");
            return;
        }

        await _popup.ClosePopupAsync(Shell.Current, Model);
    }

    [RelayCommand]
    async Task Cancel()
    {
        await _popup.ClosePopupAsync(Shell.Current);
    }
}
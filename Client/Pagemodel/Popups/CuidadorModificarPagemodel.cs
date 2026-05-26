using Aplication.Interfaces.Repositories;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Client.Pagemodel.Popups;

public partial class CuidadorModificarPopupPagemodel : ObservableObject
{

    [ObservableProperty]
    Cuidador model = new();

    IPopupService _popup;
    public CuidadorModificarPopupPagemodel(IPopupService popup)
    {
        _popup = popup;
    }
    private bool EsFormularioValido()
    {
        return !string.IsNullOrWhiteSpace(Model.nombre);
    }

    [RelayCommand]
    async Task Cancel()
    {
        await _popup.ClosePopupAsync(Shell.Current);
    }
    [RelayCommand]
    async Task Save()
    {
        if (!EsFormularioValido())
        {
            await Shell.Current.DisplayAlertAsync("Campos incompletos", "Debes rellenar todos los campos antes de guardar.", "OK");
            return;
        }

        await _popup.ClosePopupAsync(Shell.Current, Model);
    }
}
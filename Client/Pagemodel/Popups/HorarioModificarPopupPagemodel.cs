using Aplication.Interfaces.Repositories;
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Client.Pagemodel.Popups;

public partial class HorarioModificarPopupPagemodel : ObservableObject
{
    [ObservableProperty]
    Horario model = new();

    IPopupService _popup;
    public HorarioModificarPopupPagemodel(IPopupService popup)
    {
        _popup = popup;
    }

    private bool EsFormularioValido()
    {
        return !string.IsNullOrWhiteSpace(Model.hora);
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
            await Shell.Current.DisplayAlertAsync("Campo incompleto", "Debes introducir una hora válida.", "OK");
            return;
        }

        await _popup.ClosePopupAsync(Shell.Current, Model);
    }
}
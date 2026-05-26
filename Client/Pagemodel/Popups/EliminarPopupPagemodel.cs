using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Client.Pagemodel.Popups;

public partial class EliminarPopupPagemodel : ObservableObject
{


    [ObservableProperty]
    string _mensaje;
    IPopupService _popup;
    public EliminarPopupPagemodel(IPopupService popup,string mensaje)
    {
        _popup = popup;
        _mensaje = mensaje;
    }

    [RelayCommand]
    async Task Cancel()
    {
        await _popup.ClosePopupAsync(Shell.Current,new Confirmar { opcion = false });
    }
    [RelayCommand]
    async Task Save()
    {
        await _popup.ClosePopupAsync(Shell.Current, new Confirmar { opcion = true });
    }
}
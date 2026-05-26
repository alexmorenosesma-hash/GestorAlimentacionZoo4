using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Client.Pagemodel.Popups;

public partial class AlimentoPopupPagemodel : ObservableObject
{

    [ObservableProperty]
    ObservableCollection<string> _unidades = new ObservableCollection<string> { "Kg", "g" };

    [ObservableProperty]
    public Alimento _model = new()
    {
        idAlimento = null
    };

    IPopupService _popup;
    public AlimentoPopupPagemodel(IPopupService popup)
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
    async Task Cancel()
    {
        await _popup.ClosePopupAsync(Shell.Current);
    }
    [RelayCommand]
    async Task Save()
    {
        if (!EsFormularioValido())
        {
            await Shell.Current.DisplayAlertAsync("Campos incompletos", "Debes rellenar nombre, cantidad y unidad antes de guardar.", "OK");
            return;
        }

        await _popup.ClosePopupAsync(Shell.Current, Model);
    }
}
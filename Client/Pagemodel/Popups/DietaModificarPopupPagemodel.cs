using Aplication.Interfaces.Repositories;
using Client.Popups;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Client.Pagemodel.Popups;

public partial class DietaModificarPopupPagemodel : ObservableObject
{
    [ObservableProperty]
    ObservableCollection<string> _tiposAlimentacion = new ObservableCollection<string> { "Carnívoro", "Herbívoro", "Omnívoro" };

    [ObservableProperty]
    Dieta model=new();

    IPopupService _popup;
    IServiceProvider _serviceProvider;
    IAlimentoRepository _alimentoRepository;

    public DietaModificarPopupPagemodel(IPopupService popup, IServiceProvider serviceProvider, IAlimentoRepository alimentoRepository    )
    {
        _popup = popup;
        _serviceProvider = serviceProvider;
        _alimentoRepository = alimentoRepository;
    }

    private bool EsFormularioValido()
    {
        return
            !string.IsNullOrWhiteSpace(Model.nombre) &&
            Model.alimentos != null &&
            Model.alimentos.Any(a => a.Cantidad > 0)
            && !string.IsNullOrWhiteSpace(Model.tipoAlimentacion);
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
            await Shell.Current.DisplayAlertAsync("Campos incompletos", "Debes escribir un nombre para la dieta y seleccionar al menos un alimento.", "OK");
            return;
        }

        await _popup.ClosePopupAsync(Shell.Current, Model);
    }

    [RelayCommand]
    async Task SeleccionarAlimentos()
    {
        var disponibles = await _alimentoRepository.ObtenerAlimentos();

        var popup = _serviceProvider.GetRequiredService<AlimentoSelectorPopup>();
        var vm = popup.BindingContext as AlimentoSelectorPopupPagemodel;


        vm?.Inicializar(disponibles, Model.alimentos);


        var result = await Shell.Current.ShowPopupAsync(popup);

        var resultType = result?.GetType();
        var prop = resultType?.GetProperty("Result");
        var seleccionados = prop?.GetValue(result) as List<AlimentoCantidad>;

        if (seleccionados != null)
        {
            Model.alimentos = seleccionados;

            Console.WriteLine(">>> Alimentos seleccionados: " +
                string.Join(", ", seleccionados.Select(s => $"{s.Nombre} x{s.Cantidad}")));
        }
        else
        {
            Console.WriteLine(">>> El popup no devolvió alimentos");
        }
    }
}
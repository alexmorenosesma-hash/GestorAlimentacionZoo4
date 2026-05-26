using Aplication.Interfaces.Repositories;
using Client.Pagemodel.Popups;
using Client.Popups;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using System.Collections.ObjectModel;

namespace Client.Pagemodel.Inventarios;


public partial class EnfermedadPagemodel : ObservableObject
{
    IEnfermedadRepository _repository;
    IPopupService _popup;

    [ObservableProperty]
    ObservableCollection<Enfermedad> _enfermedades = new();

    [ObservableProperty]
    ObservableCollection<Enfermedad> _enfermedadesFiltradas = new();

    [ObservableProperty]
    string _textoBusqueda;

    [ObservableProperty]
    string _criterioOrden;

    public EnfermedadPagemodel(IEnfermedadRepository repository, IPopupService popup)
    {
        _repository = repository;
        _popup = popup;
        cargarEnfermedades();
    }

    [RelayCommand]
    public async Task cargarEnfermedades()
    {
        var enfermedades = await _repository.ObtenerEnfermedades();

        var lista = enfermedades
            .Where(e =>
                e != null &&
                !string.IsNullOrWhiteSpace(e.idEnfermedad) &&
                !string.IsNullOrWhiteSpace(e.nombre)
            )
            .ToList();

        Enfermedades = new ObservableCollection<Enfermedad>(lista);
        EnfermedadesFiltradas = new ObservableCollection<Enfermedad>(lista);
    }

    [RelayCommand]
    public async Task abrirPopupAñadir()
    {
        var result = await _popup.ShowPopupAsync<EnfermedadPopupPagemodel, Enfermedad>(
            Application.Current.MainPage,
            PopupOptions.Empty,
            CancellationToken.None
        );

        if (result.Result is Enfermedad nueva)
        {
            await _repository.CrearEnfermedad(nueva);
            Enfermedades.Add(nueva);
            EnfermedadesFiltradas.Add(nueva);
            await cargarEnfermedades();
        }
    }
    [RelayCommand]
    public async Task abrirPopupModificar(Enfermedad enfermedad)
    {
        var vm = new EnfermedadModificarPopupPagemodel(_popup);

        vm.Model = new Enfermedad
        {
            idEnfermedad = enfermedad.idEnfermedad,
            nombre = enfermedad.nombre,
            sintomas = enfermedad.sintomas  
        };

        var popup = new EnfermedadModificarPopup(vm);

        await Application.Current.MainPage.ShowPopupAsync(
            popup,
            PopupOptions.Empty,
            CancellationToken.None
        );

        var modificada = vm.Model;

        await _repository.EditarEnfermedad(modificada.idEnfermedad, modificada);
        await cargarEnfermedades();
    }

    [RelayCommand]
    public async Task abrirPopupEliminar(Enfermedad enfermedad)
    {
        var mensaje = $"¿Estás seguro de que deseas eliminar la enfermedad {enfermedad.nombre}?";

        var vm = new EliminarPopupPagemodel(_popup, mensaje);
        var popup = new EliminarPopup(vm);


        var result = await Application.Current.MainPage.ShowPopupAsync(popup);
        var resultType = result.GetType();
        var prop = resultType.GetProperty("Result");
        var data = prop?.GetValue(result) as Confirmar;

        var confirmado = data?.opcion ?? false;

        if (confirmado)
        {
            await _repository.EliminarEnfermedad(enfermedad.idEnfermedad);
            await cargarEnfermedades();
        }
    }

    void FiltrarYOrdenar()
    {
        IEnumerable<Enfermedad> lista = Enfermedades;

        if (!string.IsNullOrWhiteSpace(TextoBusqueda))
        {
            lista = lista.Where(e =>
                e.nombre.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                e.sintomas.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase)
            );
        }

        lista = CriterioOrden switch
        {
            "Nombre" => lista.OrderBy(e => e.nombre),
            "Síntomas" => lista.OrderBy(e => e.sintomas),
            _ => lista
        };

        EnfermedadesFiltradas = new ObservableCollection<Enfermedad>(lista);
    }

    partial void OnTextoBusquedaChanged(string value)
    {
        FiltrarYOrdenar();
    }

    partial void OnCriterioOrdenChanged(string value)
    {
        FiltrarYOrdenar();
    }

    [RelayCommand]
    public async Task volver()
    {
        await Shell.Current.GoToAsync("//MenuPage");
    }

}
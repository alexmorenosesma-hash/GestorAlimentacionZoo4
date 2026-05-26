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


public partial class CuidadorPagemodel : ObservableObject
{
    ICuidadorRepository _repository;
    IPopupService _popup;

    [ObservableProperty]
    ObservableCollection<Cuidador> _cuidadores = new();

    [ObservableProperty]
    ObservableCollection<Cuidador> _cuidadoresFiltrados = new();

    [ObservableProperty]
    string _textoBusqueda;

    [ObservableProperty]
    string _criterioOrden;

    public CuidadorPagemodel(ICuidadorRepository repository, IPopupService popup)
    {
        _repository = repository;
        _popup = popup;
        cargarCuidadores();
    }

    [RelayCommand]
    public async Task cargarCuidadores()
    {
        var cuidadores = await _repository.ObtenerCuidadores();

        var lista = cuidadores
            .Where(c =>
                c != null &&
                !string.IsNullOrWhiteSpace(c.idCuidador) &&
                !string.IsNullOrWhiteSpace(c.nombre)
            )
            .ToList();

        Cuidadores = new ObservableCollection<Cuidador>(lista);
        CuidadoresFiltrados = new ObservableCollection<Cuidador>(lista);
    }

    [RelayCommand]
    public async Task abrirPopupAñadir()
    {
        var result = await _popup.ShowPopupAsync<CuidadorPopupPagemodel, Cuidador>(
            Application.Current.MainPage,
            PopupOptions.Empty,
            CancellationToken.None
        );

        if (result.Result is Cuidador nuevo)
        {
            await _repository.CrearCuidador(nuevo);
            Cuidadores.Add(nuevo);
            CuidadoresFiltrados.Add(nuevo);
            await cargarCuidadores();
        }
    }
    [RelayCommand]
    public async Task abrirPopupModificar(Cuidador cuidador)
    {
        var vm = new CuidadorModificarPopupPagemodel(_popup);

        vm.Model = new Cuidador
        {
            idCuidador = cuidador.idCuidador,
            nombre = cuidador.nombre
        };

        var popup = new CuidadorModificarPopup(vm);

        await Application.Current.MainPage.ShowPopupAsync(
            popup,
            PopupOptions.Empty,
            CancellationToken.None
        );

        var modificada = vm.Model;

        await _repository.EditarCuidador(modificada.idCuidador, modificada);
        await cargarCuidadores();
    }

    [RelayCommand]
    public async Task abrirPopupEliminar(Cuidador cuidador)
    {
        var mensaje = $"¿Estás seguro de que deseas eliminar el cuidador {cuidador.nombre}?";

        var vm = new EliminarPopupPagemodel(_popup, mensaje);
        var popup = new EliminarPopup(vm);


        var result = await Application.Current.MainPage.ShowPopupAsync(popup);
        var resultType = result.GetType();
        var prop = resultType.GetProperty("Result");
        var data = prop?.GetValue(result) as Confirmar;

        var confirmado = data?.opcion ?? false;

        if (confirmado)
        {
            await _repository.EliminarCuidador(cuidador.idCuidador);
            await cargarCuidadores();
        }
    }

    void FiltrarYOrdenar()
    {
        IEnumerable<Cuidador> lista = Cuidadores;

        if (!string.IsNullOrWhiteSpace(TextoBusqueda))
        {
            lista = lista.Where(e =>
                e.nombre.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase)
            );
        }

        lista = CriterioOrden switch
        {
            "Nombre" => lista.OrderBy(e => e.nombre),
            _ => lista
        };

        CuidadoresFiltrados = new ObservableCollection<Cuidador>(lista);
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
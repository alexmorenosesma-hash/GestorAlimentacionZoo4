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


public partial class EspeciePagemodel : ObservableObject
{
	IEspecieRepository _repository;
    IPopupService _popup;

	[ObservableProperty]
	ObservableCollection<Especie> _especies=new();

    [ObservableProperty]
    ObservableCollection<Especie> _especiesFiltradas = new();

    [ObservableProperty]
    string _textoBusqueda;

    [ObservableProperty]
    string _criterioOrden;

    public EspeciePagemodel(IEspecieRepository repository,IPopupService popup)
	{
		_repository = repository;
        _popup= popup;
        cargarEspecies();

    }

    [RelayCommand]
    public async Task cargarEspecies()
    {
        var especies = await _repository.ObtenerEspecies();

        var lista = especies
            .Where(e =>
                e != null &&
                !string.IsNullOrWhiteSpace(e.idEspecie) &&
                !string.IsNullOrWhiteSpace(e.nombre)
            )
            .ToList();

        Especies = new ObservableCollection<Especie>(lista);
        EspeciesFiltradas = new ObservableCollection<Especie>(lista);
    }

    [RelayCommand]
    public async Task abrirPopupAñadir()
    {
        var result = await _popup.ShowPopupAsync<EspeciePopupPagemodel, Especie>(
            Application.Current.MainPage,
            PopupOptions.Empty,
            CancellationToken.None
        );

        if (result.Result is Especie nueva)
        {
            await _repository.CrearEspecie(nueva);
            Especies.Add(nueva);
            EspeciesFiltradas.Add(nueva);
            await cargarEspecies();
        }
    }
    [RelayCommand]
    public async Task abrirPopupModificar(Especie especie)
    {
        var vm = new EspecieModificarPopupPagemodel(_popup);

        vm.Model = new Especie
        {
            idEspecie = especie.idEspecie,
            nombre = especie.nombre,
            nombreCientifico = especie.nombreCientifico,
            tipoAlimentacion = especie.tipoAlimentacion,
            habitat=especie.habitat,
            tipoAnimal=especie.tipoAnimal
        };

        var popup = new EspecieModificarPopup(vm);

        await Application.Current.MainPage.ShowPopupAsync(
            popup,
            PopupOptions.Empty,
            CancellationToken.None
        );

        var modificada = vm.Model;

        await _repository.EditarEspecie(modificada.idEspecie, modificada);
        await cargarEspecies();
    }

    [RelayCommand]
    public async Task abrirPopupEliminar(Especie especie)
    {
        var mensaje = $"¿Estás seguro de que deseas eliminar la especie {especie.nombre}?";

        var vm = new EliminarPopupPagemodel(_popup, mensaje);
        var popup = new EliminarPopup(vm);


        var result = await Application.Current.MainPage.ShowPopupAsync(popup);
        var resultType = result.GetType();
        var prop = resultType.GetProperty("Result");
        var data = prop?.GetValue(result) as Confirmar;

        var confirmado = data?.opcion ?? false;

        if (confirmado)
        {
            await _repository.EliminarEspecie(especie.idEspecie);
            await cargarEspecies();
        }
    }

    void FiltrarYOrdenar()
    {
        IEnumerable<Especie> lista = Especies;

        if (!string.IsNullOrWhiteSpace(TextoBusqueda))
        {
            lista = lista.Where(e =>
                e.nombre.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                e.nombreCientifico.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                e.tipoAlimentacion.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                e.tipoAnimal.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                e.habitat.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase)
            );
        }

        lista = CriterioOrden switch
        {
            "Nombre" => lista.OrderBy(e => e.nombre),
            "Nombre científico" => lista.OrderBy(e => e.nombreCientifico),
            "Tipo alimentación" => lista.OrderBy(e => e.tipoAlimentacion),
            _ => lista
        };

        EspeciesFiltradas = new ObservableCollection<Especie>(lista);
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
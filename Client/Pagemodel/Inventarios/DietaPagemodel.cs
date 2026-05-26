using Aplication.Interfaces.Repositories;
using Client.Pagemodel.Popups;
using Client.Popups;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Maui.Services;
using CommunityToolkit.Maui.Views;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using Infraestructure.Repositories;
using System.Collections.ObjectModel;

namespace Client.Pagemodel.Inventarios;

public partial class DietaPagemodel : ObservableObject
{
    IDietaRepository _repository;
    IPopupService _popup;
    IServiceProvider _serviceProvider;
    IAlimentoRepository _alimentoRepository;

    [ObservableProperty]
    ObservableCollection<Dieta> _dietas = new();

    [ObservableProperty]
    ObservableCollection<Dieta> _dietasFiltradas = new();

    [ObservableProperty]
    string _textoBusqueda;

    [ObservableProperty]
    string _criterioOrden;

    public DietaPagemodel(IDietaRepository repository, IPopupService popup, IServiceProvider serviceProvider, IAlimentoRepository alimentoRepository)
    {
        _repository = repository;
        _popup = popup;
        _serviceProvider = serviceProvider;
        _alimentoRepository = alimentoRepository;
        cargarDietas();
    }

    [RelayCommand]
    public async Task cargarDietas()
    {
        var dietas = await _repository.ObtenerDietas();

        var lista = dietas
            .Where(d =>
                d != null &&
                !string.IsNullOrWhiteSpace(d.idDieta) &&
                !string.IsNullOrWhiteSpace(d.nombre)
            )
            .ToList();

        Dietas = new ObservableCollection<Dieta>(lista);
        DietasFiltradas = new ObservableCollection<Dieta>(lista);
    }

    [RelayCommand]
    public async Task abrirPopupAñadir()
    {
        var result = await _popup.ShowPopupAsync<DietaPopupPagemodel, Dieta>(
            Application.Current.MainPage,
            PopupOptions.Empty,
            CancellationToken.None
        );

        if (result.Result is Dieta nueva)
        {
            await _repository.CrearDieta(nueva  );
            Dietas.Add(nueva);
            DietasFiltradas.Add(nueva);
            await cargarDietas();
        }
    }
    [RelayCommand]
    public async Task abrirPopupModificar(Dieta dieta)
    {
        var vm = new DietaModificarPopupPagemodel(_popup,_serviceProvider,_alimentoRepository);

        vm.Model = new Dieta
        {
            idDieta = dieta.idDieta,
            nombre = dieta.nombre,
            alimentos = dieta.alimentos
        };

        var popup = new DietaModificarPopup(vm);

        await Application.Current.MainPage.ShowPopupAsync(
            popup,
            PopupOptions.Empty,
            CancellationToken.None
        );

        var modificada = vm.Model;

        await _repository.EditarDieta(modificada.idDieta, modificada);
        await cargarDietas();
    }

    [RelayCommand]
    public async Task abrirPopupEliminar(Dieta dieta)
    {
        var mensaje = $"¿Estás seguro de que deseas eliminar la dieta {dieta.nombre}?";

        var vm = new EliminarPopupPagemodel(_popup, mensaje);
        var popup = new EliminarPopup(vm);


        var result = await Application.Current.MainPage.ShowPopupAsync(popup);
        var resultType = result.GetType();
        var prop = resultType.GetProperty("Result");
        var data = prop?.GetValue(result) as Confirmar;

        var confirmado = data?.opcion ?? false;

        if (confirmado)
        {
            await _repository.EliminarDieta(dieta.idDieta);
            await cargarDietas();
        }
    }

    void FiltrarYOrdenar()
    {
        IEnumerable<Dieta> lista = Dietas;

        if (!string.IsNullOrWhiteSpace(TextoBusqueda))
        {
            lista = lista.Where(d =>
                d.nombre.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                d.alimentos.Any(a => a.Id.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase))
            );
        }

        lista = CriterioOrden switch
        {
            "Nombre" => lista.OrderBy(d => d.nombre),
            "Cantidad de alimentos" => lista.OrderBy(d => d.alimentos.Count),
            _ => lista
        };

        DietasFiltradas = new ObservableCollection<Dieta>(lista);
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
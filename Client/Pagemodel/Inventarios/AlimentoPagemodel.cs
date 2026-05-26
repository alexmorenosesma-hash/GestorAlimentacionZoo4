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
using System.Collections.ObjectModel;

namespace Client.Pagemodel.Inventarios;

public partial class AlimentoPagemodel : ObservableObject
{
    IAlimentoRepository _repository;
    IPopupService _popup;

    [ObservableProperty]
    ObservableCollection<Alimento> _alimentos = new();

    [ObservableProperty]
    ObservableCollection<Alimento> _alimentosFiltrados = new();

    [ObservableProperty]
    string _textoBusqueda;

    [ObservableProperty]
    string _criterioOrden;

    public AlimentoPagemodel(IAlimentoRepository repository, IPopupService popup)
    {
        _repository = repository;
        _popup = popup;
        cargarAlimentos();
    }

    [RelayCommand]
    public async Task cargarAlimentos()
    {
        var alimentos = await _repository.ObtenerAlimentos();

        var lista = alimentos
            .Where(a =>
                a != null &&
                !string.IsNullOrWhiteSpace(a.idAlimento) &&
                !string.IsNullOrWhiteSpace(a.nombre)
            )
            .ToList();

        Alimentos = new ObservableCollection<Alimento>(lista);
        AlimentosFiltrados = new ObservableCollection<Alimento>(lista);
    }

    [RelayCommand]
    public async Task abrirPopupAñadir()
    {
        var result = await _popup.ShowPopupAsync<AlimentoPopupPagemodel, Alimento>(
            Application.Current.MainPage,
            PopupOptions.Empty,
            CancellationToken.None
        );

        if (result.Result is Alimento nueva)
        {
            await _repository.CrearAlimento(nueva);
            Alimentos.Add(nueva);
            AlimentosFiltrados.Add(nueva);
            await cargarAlimentos();
        }
    }
    [RelayCommand]
    public async Task abrirPopupModificar(Alimento alimento)
    {
        var vm = new AlimentoModificarPopupPagemodel(_popup);

        vm.Model = new Alimento
        {
            idAlimento = alimento.idAlimento,
            nombre = alimento.nombre,
            cantidad = alimento.cantidad,
            unidad = alimento.unidad

        };

        var popup = new AlimentoModificarPopup(vm);

        await Application.Current.MainPage.ShowPopupAsync(
            popup,
            PopupOptions.Empty,
            CancellationToken.None
        );

        var modificada = vm.Model;

        await _repository.EditarAlimento(modificada.idAlimento, modificada);
        await cargarAlimentos();
    }

    [RelayCommand]
    public async Task abrirPopupEliminar(Alimento alimento)
    {
        var mensaje = $"¿Estás seguro de que deseas eliminar la dieta {alimento.nombre}?";

        var vm = new EliminarPopupPagemodel(_popup, mensaje);
        var popup = new EliminarPopup(vm);


        var result = await Application.Current.MainPage.ShowPopupAsync(popup);
        var resultType = result.GetType();
        var prop = resultType.GetProperty("Result");
        var data = prop?.GetValue(result) as Confirmar;

        var confirmado = data?.opcion ?? false;

        if (confirmado)
        {
            await _repository.EliminarAlimento(alimento.idAlimento);
            await cargarAlimentos();
        }
    }

    void FiltrarYOrdenar()
    {
        IEnumerable<Alimento> lista = Alimentos;

        if (!string.IsNullOrWhiteSpace(TextoBusqueda))
        {
            lista = lista.Where(a =>
                a.nombre.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                a.unidad.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase)
            );
        }

        lista = CriterioOrden switch
        {
            "Nombre" => lista.OrderBy(a => a.nombre),
            "Cantidad" => lista.OrderBy(a => a.cantidad),
            "Unidad" => lista.OrderBy(a => a.unidad),
            _ => lista
        };

        AlimentosFiltrados = new ObservableCollection<Alimento>(lista);
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
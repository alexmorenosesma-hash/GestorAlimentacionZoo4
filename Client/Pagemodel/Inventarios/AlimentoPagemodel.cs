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

/**
 * <summary>
 * ViewModel encargado de gestionar la pantalla de inventario de alimentos.
 * Permite cargar, filtrar, ordenar, crear, modificar y eliminar alimentos.
 * Utiliza popups para las operaciones CRUD.
 * ID string generated is "T:Client.Pagemodel.Inventarios.AlimentoPagemodel".
 * </summary>
 */
public partial class AlimentoPagemodel : ObservableObject
{
    IAlimentoRepository _repository;
    IPopupService _popup;


    /**
     * <summary>
     * Colección completa de alimentos obtenidos desde el repositorio.
     * ID string generated is "P:Client.Pagemodel.Inventarios.AlimentoPagemodel.Alimentos".
     * </summary>
     */
    [ObservableProperty]
    ObservableCollection<Alimento> _alimentos = new();
    /**
     * <summary>
     * Colección filtrada según búsqueda y ordenación.
     * ID string generated is "P:Client.Pagemodel.Inventarios.AlimentoPagemodel.AlimentosFiltrados".
     * </summary>
     */
    [ObservableProperty]
    ObservableCollection<Alimento> _alimentosFiltrados = new();
    /**
     * <summary>
     * Texto introducido por el usuario para buscar alimentos.
     * ID string generated is "P:Client.Pagemodel.Inventarios.AlimentoPagemodel.TextoBusqueda".
     * </summary>
     */
    [ObservableProperty]
    string _textoBusqueda;
    /**
     * <summary>
     * Criterio seleccionado para ordenar la lista de alimentos.
     * ID string generated is "P:Client.Pagemodel.Inventarios.AlimentoPagemodel.CriterioOrden".
     * </summary>
     */
    [ObservableProperty]
    string _criterioOrden;

    /**
     * <summary>
     * Constructor del ViewModel.
     * Inicializa dependencias y carga los alimentos desde el repositorio.
     * ID string generated is "M:Client.Pagemodel.Inventarios.AlimentoPagemodel.#ctor(Aplication.Interfaces.Repositories.IAlimentoRepository,CommunityToolkit.Maui.Services.IPopupService)".
     * </summary>
     */
    public AlimentoPagemodel(IAlimentoRepository repository, IPopupService popup)
    {
        _repository = repository;
        _popup = popup;
        cargarAlimentos();
    }


    /**
     * <summary>
     * Carga todos los alimentos desde el repositorio y actualiza las colecciones.
     * ID string generated is "M:Client.Pagemodel.Inventarios.AlimentoPagemodel.cargarAlimentos".
     * </summary>
     */
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


    /**
     * <summary>
     * Abre el popup para añadir un nuevo alimento.
     * Si el usuario confirma, se guarda en Firebase y se recarga la lista.
     * ID string generated is "M:Client.Pagemodel.Inventarios.AlimentoPagemodel.abrirPopupAñadir".
     * </summary>
     */
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
     /**
     * <summary>
     * Abre el popup para modificar un alimento existente.
     * Tras confirmar, actualiza el registro en Firebase.
     * ID string generated is "M:Client.Pagemodel.Inventarios.AlimentoPagemodel.abrirPopupModificar(Domain.Entities.Alimento)".
     * </summary>
     */
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
    /**
     * <summary>
     * Abre un popup de confirmación para eliminar un alimento.
     * Si el usuario acepta, se elimina del repositorio.
     * ID string generated is "M:Client.Pagemodel.Inventarios.AlimentoPagemodel.abrirPopupEliminar(Domain.Entities.Alimento)".
     * </summary>
     */

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
    /**
     * <summary>
     * Aplica el filtro de búsqueda y el criterio de ordenación seleccionados.
     * ID string generated is "M:Client.Pagemodel.Inventarios.AlimentoPagemodel.FiltrarYOrdenar".
     * </summary>
     */
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

    /**
    * <summary>
    * Evento generado automáticamente cuando cambia el texto de búsqueda.
    * ID string generated is "M:Client.Pagemodel.Inventarios.AlimentoPagemodel.OnTextoBusquedaChanged(System.String)".
    * </summary>
    */
    partial void OnTextoBusquedaChanged(string value)
    {
        FiltrarYOrdenar();
    }

    /**
     * <summary>
     * Evento generado automáticamente cuando cambia el criterio de ordenación.
     * ID string generated is "M:Client.Pagemodel.Inventarios.AlimentoPagemodel.OnCriterioOrdenChanged(System.String)".
     * </summary>
     */
    partial void OnCriterioOrdenChanged(string value)
    {
        FiltrarYOrdenar();
    }
        /**
     * <summary>
     * Navega de vuelta al menú principal.
     * ID string generated is "M:Client.Pagemodel.Inventarios.AlimentoPagemodel.volver".
     * </summary>
     */
    [RelayCommand]
    public async Task volver()
    {
        await Shell.Current.GoToAsync("//MenuPage");
    }

}
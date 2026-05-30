using Aplication.Interfaces.Firebase.Realtime;
using Aplication.Interfaces.Repositories;
using Client.Popups;
using CommunityToolkit.Maui;
using CommunityToolkit.Maui.Extensions;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using Infraestructure.Repositories;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Client.Pagemodel.Popups;

public partial class AnimalModificarPopupPagemodel : ObservableObject
{

    [ObservableProperty]
    Animal _model = new();

    [ObservableProperty]
    ObservableCollection<string> _nombresEspecies = new();

    [ObservableProperty]
    ObservableCollection<string> _nombresDietas = new();

    [ObservableProperty]
    ObservableCollection<string> _nombresCuidadores = new();

    [ObservableProperty]
    ObservableCollection<string> _horarios = new();

    [ObservableProperty]
    ObservableCollection<string> _enfermedades = new();

    [ObservableProperty]
    string _especieSeleccionada;

    [ObservableProperty]
    string _dietaSeleccionada;

    [ObservableProperty]
    string _cuidadorSeleccionado;

    IPopupService _popup;
    IEspecieRepository _especieRepository;
    IDietaRepository _dietaRepository;
    IHorarioRepository _horarioRepository;
    IEnfermedadRepository _enfermedadRepository;
    ICuidadorRepository _cuidadorRepository;
    IServiceProvider _serviceProvider;

    public AnimalModificarPopupPagemodel(IPopupService popup, IEspecieRepository especieRepository, IDietaRepository dietaRepository, IHorarioRepository horarioRepository, IEnfermedadRepository enfermedadRepository, ICuidadorRepository cuidadorRepository, IServiceProvider service)
    {
        _popup = popup;
        _especieRepository = especieRepository;
        _dietaRepository = dietaRepository;
        _horarioRepository = horarioRepository;
        _enfermedadRepository = enfermedadRepository;
        _cuidadorRepository = cuidadorRepository;
        _serviceProvider = service;
        InicializarAsync();
    }

    private bool EsFormularioValido()
    {
        return
            !string.IsNullOrWhiteSpace(Model.nombre) &&
            Model.edad > 0 &&
            Model.peso > 0 &&
            Model.especie != null &&
            Model.dieta != null &&
            Model.cuidador != null &&
            Model.horariosAlimentacion != null &&
            Model.horariosAlimentacion.Count > 0;
    }

    public async Task InicializarAsync()
    {
        await CargarNombresEspecies();
        await CargarNombresDietas();
        await CargarNombresCuidadores();
        EspecieSeleccionada = Model.especie;
        DietaSeleccionada = Model.dieta;
        CuidadorSeleccionado = Model.cuidador;
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
            await Shell.Current.DisplayAlertAsync("Campos incompletos","Debes rellenar todos los campos y seleccionar horarios antes de guardar.","OK");
            return;
        }

        await _popup.ClosePopupAsync(Shell.Current, Model);
    }


    public async Task CargarNombresEspecies()
    {
        var nombres = await _especieRepository.ObtenerNombreEspecies();
        NombresEspecies = new ObservableCollection<string>(nombres);
    }

    public async Task CargarNombresDietas()
    {
        var nombres = await _dietaRepository.ObtenerNombreDietas();

        NombresDietas = new ObservableCollection<string>(nombres);
    }

    public async Task CargarNombresCuidadores()
    {
        var nombres = await _cuidadorRepository.ObtenerNombreCuidadores();

        NombresCuidadores = new ObservableCollection<string>(nombres);
    }

    [RelayCommand]
    async Task SeleccionarHorarios()
    {
        var horarios = await _horarioRepository.ObtenerHorarios();

        var popup = _serviceProvider.GetRequiredService<HorarioSelectorPopup>();
        var vm = popup.BindingContext as HorarioSelectorPopupPagemodel;
        vm?.Inicializar(horarios, Model.horariosAlimentacion);
        var result = await Shell.Current.ShowPopupAsync(popup);

        var resultType = result?.GetType();
        var prop = resultType?.GetProperty("Result");
        var seleccionados = prop?.GetValue(result) as HorariosSeleccionados;

        if (seleccionados != null)
        {
            Model.horariosAlimentacion = seleccionados.Horarios
                .Select(h => h.idHorario)
                .ToList();

            Console.WriteLine(">>> Horarios seleccionados: " + string.Join(", ", Model.horariosAlimentacion));
        }
        else
        {
            Console.WriteLine(">>> El popup no devolvió horarios");
        }
    }

        [RelayCommand]
    async Task SeleccionarEnfermedades()
    {
        var enfermedades = await _enfermedadRepository.ObtenerEnfermedades();

        var popup = _serviceProvider.GetRequiredService<EnfermedadSelectorPopup>();
        var vm = popup.BindingContext as EnfermedadSelectorPopupPagemodel;
        vm?.Inicializar(enfermedades, Model.enfermedades);
        var result = await Shell.Current.ShowPopupAsync(popup);

        var resultType = result?.GetType();
        var prop = resultType?.GetProperty("Result");
        var seleccionados = prop?.GetValue(result) as EnfermedadSeleccionadas;

        if (seleccionados != null)
        {
            Model.enfermedades = seleccionados.Enfermedades
                .Select(h => h.idEnfermedad)
                .ToList();

            Console.WriteLine(">>> Enfermedades seleccionadas: " + string.Join(", ", Model.enfermedades));
        }
    }
}
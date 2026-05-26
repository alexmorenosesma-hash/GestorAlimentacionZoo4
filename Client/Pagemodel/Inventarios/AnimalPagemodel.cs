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

public partial class AnimalPagemodel : ObservableObject
{
    IAnimalRepository _repository;
    IPopupService _popup;
    IEspecieRepository _especieRepository;
    IDietaRepository _dietaRepository;
    IHorarioRepository _horarioRepository;
    IServiceProvider _serviceProvider;
    ICuidadorRepository _cuidadorRepository;
    IEnfermedadRepository _enfermedadRepository;

    [ObservableProperty]
    ObservableCollection<Animal> _animales = new();

    [ObservableProperty]
    ObservableCollection<Animal> _animalesFiltrados = new();

    private List<Horario> _horariosSistema = new();

    private List<Enfermedad> _enfermedadesSistema = new();

    [ObservableProperty]
    string _textoBusqueda;

    [ObservableProperty]
    string _criterioOrden;

    public AnimalPagemodel(IAnimalRepository repository, IPopupService popup, IEspecieRepository especieRepository, IDietaRepository dietaRepository, IHorarioRepository horarioRepository, ICuidadorRepository cuidadorRepository, IEnfermedadRepository enfermedadRepository, IServiceProvider serviceProvider)
    {
        _repository = repository;
        _popup = popup;
        _especieRepository = especieRepository;
        _dietaRepository = dietaRepository;
        _horarioRepository = horarioRepository;
        _cuidadorRepository = cuidadorRepository;
        _enfermedadRepository = enfermedadRepository;
        _serviceProvider = serviceProvider;
        cargarHorariosSistema();
        cargaEnfermedadesSistema();
        cargarAnimales();
    }

    [RelayCommand]
    public async Task cargarAnimales()
    {
        var animales = await _repository.ObtenerAnimales();

        var lista = animales
            .Where(a =>
                a != null &&
                !string.IsNullOrWhiteSpace(a.idAnimal) &&
                !string.IsNullOrWhiteSpace(a.nombre)
            )
            .ToList();

        foreach (var animal in lista)
        {
            animal.HorariosTexto = ObtenerTextoHorarios(animal);
            animal.EnfermedadesTexto = ObtenerTextoEnfermedades(animal);
        }

        Animales = new ObservableCollection<Animal>(lista);
        AnimalesFiltrados = new ObservableCollection<Animal>(lista);
    }

    [RelayCommand]
    public async Task abrirPopupAñadir()
    {
        var result = await _popup.ShowPopupAsync<AnimalPopupPagemodel, Animal>(
            Application.Current.MainPage,
            PopupOptions.Empty,
            CancellationToken.None
        );

        if (result.Result is Animal nuevo)
        {
            await _repository.CrearAnimal(nuevo);
            Animales.Add(nuevo);
            AnimalesFiltrados.Add(nuevo);
            await cargarAnimales();
        }
    }
    [RelayCommand]
    public async Task abrirPopupModificar(Animal animal)
    {
        var vm = new AnimalModificarPopupPagemodel(_popup, _especieRepository, _dietaRepository, _horarioRepository, _enfermedadRepository,_cuidadorRepository, _serviceProvider);

        vm.Model = new Animal
        {
            idAnimal = animal.idAnimal,
            nombre = animal.nombre,
            edad = animal.edad,
            peso = animal.peso,
            especie = animal.especie,
            dieta = animal.dieta,
            cuidador = animal.cuidador,
            horariosAlimentacion =animal.horariosAlimentacion,
            enfermedades = animal.enfermedades

        };

        var popup = new AnimalModificarPopup(vm);

        await Application.Current.MainPage.ShowPopupAsync(
            popup,
            PopupOptions.Empty,
            CancellationToken.None
        );

        var modificada = vm.Model;

        await _repository.EditarAnimal(modificada.idAnimal, modificada);
        await cargarAnimales();
    }

    [RelayCommand]
    public async Task abrirPopupEliminar(Animal animal)
    {
        var mensaje = $"¿Estás seguro de que deseas eliminar el animal {animal.nombre}?";

        var vm = new EliminarPopupPagemodel(_popup, mensaje);
        var popup = new EliminarPopup(vm);


        var result = await Application.Current.MainPage.ShowPopupAsync(popup);
        var resultType = result.GetType();
        var prop = resultType.GetProperty("Result");
        var data = prop?.GetValue(result) as Confirmar;

        var confirmado = data?.opcion ?? false;

        if (confirmado)
        {
            await _repository.EliminarAnimal(animal.idAnimal);
            await cargarAnimales();
        }
    }
    public async Task cargarHorariosSistema()
    {
        _horariosSistema = await _horarioRepository.ObtenerHorarios();
    }
    public string ObtenerTextoHorarios(Animal animal)
    {
        if (animal == null)
            return string.Empty;

        if (animal.horariosAlimentacion == null || !animal.horariosAlimentacion.Any())
            return "Sin horarios";

        if (_horariosSistema == null || !_horariosSistema.Any())
            return "Sin horarios";

        var horas = animal.horariosAlimentacion
            .Select(id => _horariosSistema.FirstOrDefault(h => h.idHorario == id)?.hora)
            .Where(h => !string.IsNullOrWhiteSpace(h));

        return horas.Any() ? string.Join(", ", horas) : "Sin horarios";
    }

    public async Task cargaEnfermedadesSistema()
    {
        _enfermedadesSistema = await _enfermedadRepository.ObtenerEnfermedades();
    }
    public string ObtenerTextoEnfermedades(Animal animal)
    {
        if (animal == null)
            return string.Empty;

        if (animal.enfermedades == null || !animal.enfermedades.Any())
            return "Sin enfermedades";

        if (_enfermedadesSistema == null || !_enfermedadesSistema.Any())
            return "Sin enfermedades";

        var enfermedades = animal.enfermedades
            .Select(id => _enfermedadesSistema.FirstOrDefault(e => e.idEnfermedad == id)?.nombre)
            .Where(n => !string.IsNullOrWhiteSpace(n));

        return enfermedades.Any() ? string.Join(", ", enfermedades) : "Sin enfermedades";
    }

    void FiltrarYOrdenar()
    {
        IEnumerable<Animal> lista = Animales;
        if (!string.IsNullOrWhiteSpace(TextoBusqueda))
        {
            lista = lista.Where(a =>
                a.nombre.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                a.especie.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                a.dieta.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase) ||
                a.cuidador.Contains(TextoBusqueda, StringComparison.OrdinalIgnoreCase)
            );
        }

        lista = CriterioOrden switch
        {
            "Nombre" => lista.OrderBy(a => a.nombre),
            "Edad" => lista.OrderBy(a => a.edad),
            "Peso" => lista.OrderBy(a => a.peso),
            "Cuidador" => lista.OrderBy(a => a.cuidador),
            _ => lista
        };

        AnimalesFiltrados = new ObservableCollection<Animal>(lista);
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
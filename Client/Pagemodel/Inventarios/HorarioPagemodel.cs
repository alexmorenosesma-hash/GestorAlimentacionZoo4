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

public partial class HorarioPagemodel : ObservableObject
{
    IHorarioRepository _repository;
    IPopupService _popup;

    [ObservableProperty]
    ObservableCollection<Horario> _horarios = new();

    public HorarioPagemodel(IHorarioRepository repository, IPopupService popup)
    {
        _repository = repository;
        _popup = popup;
        cargarHorario();

    }

    [RelayCommand]
    public async Task cargarHorario()
    {
        var horarios = await _repository.ObtenerHorarios();
        Horarios = new ObservableCollection<Horario>(
            horarios.Where(e =>
                e != null &&
                !string.IsNullOrWhiteSpace(e.idHorario)
            )
        );

    }
    [RelayCommand]
    public async Task abrirPopupAñadir()
    {
        var result = await _popup.ShowPopupAsync<HorarioPopupPagemodel, Horario>(
            Application.Current.MainPage,
            PopupOptions.Empty,
            CancellationToken.None
        );

        if (result.Result is Horario nueva)
        {
            await _repository.CrearHorario(nueva);
            Horarios.Add(nueva);
            await cargarHorario();
        }
    }
    [RelayCommand]
    public async Task abrirPopupModificar(Horario horario)
    {
        var vm = new HorarioModificarPopupPagemodel(_popup);

        vm.Model = new Horario
        {
            idHorario = horario.idHorario,
            hora = horario.hora
        };

        var popup = new HorarioModificarPopup(vm);

        await Application.Current.MainPage.ShowPopupAsync(
            popup,
            PopupOptions.Empty,
            CancellationToken.None
        );

        var modificada = vm.Model;

        await _repository.EditarHorario(modificada.idHorario, modificada);
        await cargarHorario();
    }

    [RelayCommand]
    public async Task abrirPopupEliminar(Horario horario)
    {
        var mensaje = $"¿Estás seguro de que deseas eliminar la hora {horario.hora}?";

        var vm = new EliminarPopupPagemodel(_popup, mensaje);
        var popup = new EliminarPopup(vm);


        var result = await Application.Current.MainPage.ShowPopupAsync(popup);
        var resultType = result.GetType();
        var prop = resultType.GetProperty("Result");
        var data = prop?.GetValue(result) as Confirmar;

        var confirmado = data?.opcion ?? false;

        if (confirmado)
        {
            await _repository.EliminarHorario(horario.idHorario);
            await cargarHorario();
        }
    }

    [RelayCommand]
    public async Task volver()
    {
        await Shell.Current.GoToAsync("//MenuPage");
    }

}
using CommunityToolkit.Maui;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Domain.Entities;
using System.Collections.ObjectModel;
using System.Reactive.Linq;

namespace Client.Pagemodel.Popups;

public partial class EspeciePopupPagemodel : ObservableObject
{
	[ObservableProperty]
	ObservableCollection<string> _tiposAlimentacion = new ObservableCollection<string> { "Carnívoro", "Herbívoro", "Omnívoro" };

    [ObservableProperty]
    ObservableCollection<string> tiposAnimales = new() { "Mamífero", "Ave", "Reptil", "Anfibio", "Pez", "Artrópodo" };

    [ObservableProperty]
    ObservableCollection<string> habitats = new() { "Selva", "Desierto", "Sabana", "Bosque", "Montaña", "Océano", "Río", "Humedal", "Tundra" };

    [ObservableProperty]
	public Especie _model = new()
	{
		idEspecie = null
	};

	IPopupService _popup;
	public EspeciePopupPagemodel(IPopupService popup)
	{
		_popup = popup;
    }

    private bool EsFormularioValido()
    {
        return !string.IsNullOrWhiteSpace(Model.nombre)
            && !string.IsNullOrWhiteSpace(Model.nombreCientifico)
            && !string.IsNullOrWhiteSpace(Model.tipoAlimentacion)
            && !string.IsNullOrWhiteSpace(Model.tipoAnimal)
            && !string.IsNullOrWhiteSpace(Model.habitat);
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
            await Shell.Current.DisplayAlertAsync("Campos incompletos","Debes rellenar todos los campos antes de guardar.","OK");
            return;
        }

        await _popup.ClosePopupAsync(Shell.Current, Model);
    }
}
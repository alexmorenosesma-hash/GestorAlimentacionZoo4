using Aplication.Interfaces.Firebase.Authentication;
using Client.Pagemodel.Login;
using Client.Pages.Inventarios;
using Client.Pages.Login;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace Client.Pagemodel.Menu;

public partial class MenuPagemodel : ObservableObject
{
    private readonly IAuthService _auth;
    public MenuPagemodel()
	{

	}
    [RelayCommand]
	public async Task irEspecies()
	{
        await Shell.Current.GoToAsync("//EspeciePage");
    }
    [RelayCommand]
    public async Task irAnimal()
    {
        await Shell.Current.GoToAsync("//AnimalPage");
    }
    [RelayCommand]
    public async Task irHorario()
    {
        await Shell.Current.GoToAsync("//HorarioPage");
    }
    [RelayCommand]
    public async Task irDieta()
    {
        await Shell.Current.GoToAsync("//DietaPage");
    }
    [RelayCommand]
    public async Task irAlimentos()
    {
        await Shell.Current.GoToAsync("//AlimentoPage");
    }
    [RelayCommand]
    public async Task irEnfermedades()
    {
        await Shell.Current.GoToAsync("//EnfermedadPage");
    }
    [RelayCommand]
    public async Task irCuidadores()
    {
        await Shell.Current.GoToAsync("//CuidadorPage");
    }

    [RelayCommand]
    public async Task cerrarSesion()
    {
        Application.Current.MainPage = new LoginPage(new LoginPagemodel(_auth));
    }
}
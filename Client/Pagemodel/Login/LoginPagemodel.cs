using Aplication.Interfaces.Firebase.Authentication;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using System.Globalization;

namespace Client.Pagemodel.Login;

/*
    Prueba de commit
*/
public partial class LoginPagemodel : ObservableObject
{

    [ObservableProperty]
	string _username;
	[ObservableProperty]
	string _password;
	IAuthService _auth;
    public LoginPagemodel(IAuthService auth)
	{ 
		_auth = auth;
	}
    
    [RelayCommand]
    public async Task recuperarContraseña()
    {
        try
        {
            await _auth.resetEmailPasswordAsync(Username);
            await Application.Current.MainPage.DisplayAlertAsync("Ok", "Se ha enviado la peticion de cambio de contraseña", "Aceptar");
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
    [RelayCommand]
    public async Task iniciarSesion()
    {
        try
        {
            var usuario = await _auth.loginAsync(Username, Password);
            await Application.Current.MainPage.DisplayAlertAsync("Bienvenido", "Se ha iniciado sesion", "Ok");
            Application.Current.Windows[0].Page = new AppShell();
        }
        catch (Exception ex)
        {
            await Application.Current.MainPage.DisplayAlertAsync("Error", ex.Message, "OK");
        }
    }
}
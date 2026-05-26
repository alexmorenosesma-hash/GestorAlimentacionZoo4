using Client.Pagemodel.Login;

namespace Client.Pages.Login;

public partial class LoginPage : ContentPage
{
	public LoginPage(LoginPagemodel vm)
	{
		InitializeComponent();
		BindingContext = vm;
    }
}
using Client.Pages.Login;
using Microsoft.Extensions.DependencyInjection;

namespace Client
{
    public partial class App : Application
    {
        LoginPage _login;
        public App(IServiceProvider service)
        {
            InitializeComponent();
            _login = service.GetRequiredService<LoginPage>();

        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(_login);
        }
    }
}
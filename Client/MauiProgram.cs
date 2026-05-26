using Client.Pagemodel.Inventarios;
using Client.Pagemodel.Login;
using Client.Pagemodel.Menu;
using Client.Pagemodel.Popups;
using Client.Pages.Login;
using Client.Popups;
using CommunityToolkit.Maui;
using Infraestructure;
using MauiIcons.Fluent;
using Microsoft.Extensions.Logging;

namespace Client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .UseFluentMauiIcons()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif
            builder.Services.AddInfraestructure();
            builder.dependencias();
            return builder.Build();
        }
        public static MauiAppBuilder dependencias(this MauiAppBuilder builder)
        {
            builder.Services.AddScoped<LoginPage>();
            builder.Services.AddScoped<LoginPagemodel>();
            builder.Services.AddTransient<MenuPagemodel>();
            builder.Services.AddTransient<EspeciePagemodel>();
            builder.Services.AddTransientPopup<EspeciePopup, EspeciePopupPagemodel>();
            builder.Services.AddTransientPopup<EspecieModificarPopup, EspecieModificarPopupPagemodel>();
            builder.Services.AddTransientPopup<EliminarPopup, EliminarPopupPagemodel>();
            builder.Services.AddTransient<HorarioPagemodel>();
            builder.Services.AddTransientPopup<HorarioPopup, HorarioPopupPagemodel>();
            builder.Services.AddTransientPopup<HorarioModificarPopup, HorarioModificarPopupPagemodel>();
            builder.Services.AddTransient<AlimentoPagemodel>();
            builder.Services.AddTransientPopup<AlimentoPopup, AlimentoPopupPagemodel>();
            builder.Services.AddTransientPopup<AlimentoModificarPopup, AlimentoModificarPopupPagemodel>();
            builder.Services.AddTransientPopup<DietaPopup, DietaPopupPagemodel>();
            builder.Services.AddTransientPopup<DietaModificarPopup, DietaModificarPopupPagemodel>();
            builder.Services.AddTransient<DietaPagemodel>();
            builder.Services.AddTransient<AnimalPagemodel>();
            builder.Services.AddTransientPopup<AnimalPopup, AnimalPopupPagemodel>();
            builder.Services.AddTransientPopup<AnimalModificarPopup, AnimalModificarPopupPagemodel>();
            builder.Services.AddTransientPopup<HorarioSelectorPopup, HorarioSelectorPopupPagemodel>();
            builder.Services.AddTransientPopup<AlimentoSelectorPopup, AlimentoSelectorPopupPagemodel>();
            builder.Services.AddTransient<EnfermedadPagemodel>();
            builder.Services.AddTransientPopup<EnfermedadPopup, EnfermedadPopupPagemodel>();
            builder.Services.AddTransientPopup<EnfermedadModificarPopup, EnfermedadModificarPopupPagemodel>();
            builder.Services.AddTransient<CuidadorPagemodel>();
            builder.Services.AddTransientPopup <CuidadorPopup,CuidadorPopupPagemodel>();
            builder.Services.AddTransientPopup<CuidadorModificarPopup, CuidadorModificarPopupPagemodel>();
            builder.Services.AddTransientPopup<EnfermedadSelectorPopup, EnfermedadSelectorPopupPagemodel>();
            return builder;
            }
        }
}

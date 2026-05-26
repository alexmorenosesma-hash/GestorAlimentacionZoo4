using Aplication.Interfaces.Firebase.Authentication;
using Aplication.Interfaces.Firebase.Realtime;
using Aplication.Interfaces.Repositories;
using Infraestructure.Repositories;
using Infraestructure.Services.Firebase.Auth;
using Infraestructure.Services.Firebase.Realtime;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructure
{
    public static class DependencyInjection
    {
        public static void AddInfraestructure(this IServiceCollection services)
        {
            services.AddScoped<IAuthService,AuthService>();
            services.AddTransient<IRealtimeService, RealtimeService>();
            services.AddTransient<IEspecieRepository, EspecieRepository>();
            services.AddTransient<IHorarioRepository, HorarioRepository>();
            services.AddTransient<IAlimentoRepository, AlimentoRepository>();
            services.AddTransient<IDietaRepository, DietaRepository>();
            services.AddTransient<IAnimalRepository, AnimalRepository>();
            services.AddTransient<IEnfermedadRepository, EnfermedadRepository>();
            services.AddTransient<ICuidadorRepository, CuidadorRepository>();
        }
    }
}

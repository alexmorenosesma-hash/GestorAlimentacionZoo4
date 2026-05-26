using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplication.Interfaces.Repositories
{
    public interface IDietaRepository
    {
        Task<string> CrearDieta(Dieta dieta);
        Task<List<Dieta>> ObtenerDietas();
        Task EditarDieta(string id, Dieta dieta);
        Task EliminarDieta(string id);
        public Task<List<string>> ObtenerNombreDietas();

    }
}

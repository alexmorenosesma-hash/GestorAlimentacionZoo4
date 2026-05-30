using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplication.Interfaces.Repositories
{
    public interface IEnfermedadRepository
    {
        Task<string> CrearEnfermedad(Enfermedad enfermedad);
        Task<List<Enfermedad>> ObtenerEnfermedades();
        Task EditarEnfermedad(string id, Enfermedad enfermedad);
        Task EliminarEnfermedad(string id);
        public Task<List<string>> ObtenerNombreEnfermedades();
        public Task<List<(string Id, Enfermedad Data)>> ObtenerPaginaEnfermedades(string? lastKey, int pageSize);

    }
}

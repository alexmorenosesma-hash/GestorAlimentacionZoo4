using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplication.Interfaces.Repositories
{
    public interface ICuidadorRepository
    {
        Task<string> CrearCuidador(Cuidador cuidador);
        Task<List<Cuidador>> ObtenerCuidadores();
        Task EditarCuidador(string id, Cuidador cuidador);
        Task EliminarCuidador(string id);
        public Task<List<string>> ObtenerNombreCuidadores();
        public Task<List<(string Id, Cuidador Data)>> ObtenerPaginaCuidadores(string? lastKey, int pageSize);

    }
}

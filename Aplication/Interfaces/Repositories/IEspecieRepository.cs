using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplication.Interfaces.Repositories
{
    public interface IEspecieRepository
    {
        Task<string> CrearEspecie(Especie especie);
        Task<List<Especie>> ObtenerEspecies();
        Task EditarEspecie(string id, Especie especie);
        Task EliminarEspecie(string id);
        public Task<List<string>> ObtenerNombreEspecies();

    }
}

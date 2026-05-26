using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplication.Interfaces.Repositories
{
    public interface IAlimentoRepository
    {
        Task<string> CrearAlimento(Alimento alimento);
        Task<List<Alimento>> ObtenerAlimentos();
        Task EditarAlimento(string id, Alimento alimento);
        Task EliminarAlimento(string id);
    }
}

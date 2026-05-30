using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplication.Interfaces.Repositories
{
    public interface IHorarioRepository
    {
        Task<string> CrearHorario(Horario horario);
        Task<List<Horario>> ObtenerHorarios();
        Task EditarHorario(string id, Horario horario);
        Task EliminarHorario(string id);
        public Task<List<(string Id, Horario Data)>> ObtenerPaginaHorarios(string? lastKey, int pageSize);

    }
}

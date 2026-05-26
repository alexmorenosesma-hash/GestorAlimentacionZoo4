using System;
using Aplication.Interfaces.Firebase.Realtime;
using Aplication.Interfaces.Repositories;
using Domain.Entities;
using Infraestructure.Services.Firebase.Realtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructure.Repositories
{
    public class HorarioRepository : IHorarioRepository
    {
        private readonly IRealtimeService _db;

        public HorarioRepository(IRealtimeService db)
        {
            _db = db;
        }

        public async Task<string> CrearHorario(Horario horario)
        {
            var id = await _db.AddAsync("horarios", horario);

            horario.idHorario = id;
            await _db.UpdateAsync("horarios", id, horario);

            return id;
        }

        public async Task<List<Horario>> ObtenerHorarios()
        {
            var result = await _db.GetAllAsync<Horario>("horarios");

            return result.Select(x =>
            {
                x.Data.idHorario = x.Id;
                return x.Data;
            }).ToList();
        }

        public Task EditarHorario(string id, Horario horario)
            => _db.UpdateAsync("horarios", id, horario);


        public Task EliminarHorario(string id)
            => _db.DeleteAsync("horarios", id);
    }
}

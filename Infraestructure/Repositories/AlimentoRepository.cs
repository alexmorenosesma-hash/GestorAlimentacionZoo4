using Aplication.Interfaces.Firebase.Realtime;
using Aplication.Interfaces.Repositories;
using Domain.Entities;
using Infraestructure.Services.Firebase.Realtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructure.Repositories
{
    public class AlimentoRepository : IAlimentoRepository
    {
        private readonly IRealtimeService _db;

        public AlimentoRepository(IRealtimeService db)
        {
            _db = db;
        }

        public async Task<string> CrearAlimento(Alimento alimento)
        {
            var id = await _db.AddAsync("alimentos", alimento);

            alimento.idAlimento = id;
            await _db.UpdateAsync("alimentos", id, alimento);

            return id;
        }

        public async Task<List<Alimento>> ObtenerAlimentos()
        {
            var result = await _db.GetAllAsync<Alimento>("alimentos");

            return result.Select(x =>
            {
                x.Data.idAlimento = x.Id;
                return x.Data;
            }).ToList();
        }

        public Task EditarAlimento(string id, Alimento alimento)
            => _db.UpdateAsync("alimentos", id, alimento);


        public Task EliminarAlimento(string id)
            => _db.DeleteAsync("alimentos", id);

        public async Task<List<(string Id, Alimento Data)>> ObtenerPaginaAlimentos(string? lastKey, int pageSize)
        {
            return await _db.GetPagedAsync<Alimento>("alimentos", lastKey, pageSize);
        }
    }
}

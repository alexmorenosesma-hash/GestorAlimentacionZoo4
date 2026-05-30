using Aplication.Interfaces.Firebase.Realtime;
using Aplication.Interfaces.Repositories;
using Domain.Entities;
using Infraestructure.Services.Firebase.Realtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructure.Repositories
{
    public class CuidadorRepository : ICuidadorRepository
    {
        private readonly IRealtimeService _db;

        public CuidadorRepository(IRealtimeService db)
        {
            _db = db;
        }

        public async Task<string> CrearCuidador(Cuidador cuidador)
        {
            var id = await _db.AddAsync("cuidadores", cuidador);

            cuidador.idCuidador = id;
            await _db.UpdateAsync("cuidadores", id, cuidador);

            return id;
        }

        public async Task<List<Cuidador>> ObtenerCuidadores()
        {
            var result = await _db.GetAllAsync<Cuidador>("cuidadores");

            return result.Select(x =>
            {
                x.Data.idCuidador = x.Id;
                return x.Data;
            }).ToList();
        }

        public Task EditarCuidador(string id, Cuidador cuidador)
            => _db.UpdateAsync("cuidadores", id, cuidador);


        public Task EliminarCuidador(string id)
            => _db.DeleteAsync("cuidadores", id);

        public async Task<List<string>> ObtenerNombreCuidadores()
        {
            var cuidadores = await ObtenerCuidadores();
            return cuidadores
            .Where(c => !string.IsNullOrWhiteSpace(c.nombre))
            .Select(c => c.nombre)
            .ToList();
        }

        public async Task<List<(string Id, Cuidador Data)>> ObtenerPaginaCuidadores(string? lastKey, int pageSize)
        {
            return await _db.GetPagedAsync<Cuidador>("cuidadores", lastKey, pageSize);
        }

    }
}

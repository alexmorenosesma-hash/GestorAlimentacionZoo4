using Aplication.Interfaces.Firebase.Realtime;
using Aplication.Interfaces.Repositories;
using Domain.Entities;
using Infraestructure.Services.Firebase.Realtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructure.Repositories
{
    public class DietaRepository : IDietaRepository
    {
        private readonly IRealtimeService _db;

        public DietaRepository(IRealtimeService db)
        {
            _db = db;
        }

        public async Task<string> CrearDieta(Dieta dieta)
        {
            var id = await _db.AddAsync("dietas", dieta);

            dieta.idDieta = id;
            await _db.UpdateAsync("dietas", id, dieta);

            return id;
        }

        public async Task<List<Dieta>> ObtenerDietas()
        {
            var result = await _db.GetAllAsync<Dieta>("dietas");

            return result.Select(x =>
            {
                x.Data.idDieta = x.Id;
                return x.Data;
            }).ToList();
        }

        public Task EditarDieta(string id, Dieta dieta)
            => _db.UpdateAsync("dietas", id, dieta);


        public Task EliminarDieta(string id)
            => _db.DeleteAsync("dietas", id);

        public async Task<List<string>> ObtenerNombreDietas()
        {
            var dietas = await ObtenerDietas();
            return dietas
            .Where(d => !string.IsNullOrWhiteSpace(d.nombre))
            .Select(d => d.nombre)
            .ToList();
        }
    }
}

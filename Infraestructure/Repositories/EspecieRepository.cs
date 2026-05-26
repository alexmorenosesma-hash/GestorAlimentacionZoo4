using Aplication.Interfaces.Firebase.Realtime;
using Aplication.Interfaces.Repositories;
using Domain.Entities;
using Infraestructure.Services.Firebase.Realtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructure.Repositories
{
    public class EspecieRepository:IEspecieRepository
    {
        private readonly IRealtimeService _db;

        public EspecieRepository(IRealtimeService db)
        {
            _db = db;
        }

        public async Task<string> CrearEspecie(Especie especie)
        {
            var id = await _db.AddAsync("especies", especie);

            especie.idEspecie = id;
            await _db.UpdateAsync("especies", id, especie);

            return id;
        }

        public async Task<List<Especie>> ObtenerEspecies()
        {
            var result = await _db.GetAllAsync<Especie>("especies");

            return result.Select(x =>
            {
                x.Data.idEspecie = x.Id;
                return x.Data;
            }).ToList();
        }

        public Task EditarEspecie(string id, Especie especie)
            => _db.UpdateAsync("especies", id, especie);


        public Task EliminarEspecie(string id)
            => _db.DeleteAsync("especies", id);

        public async Task<List<string>> ObtenerNombreEspecies()
        {
            var especies = await ObtenerEspecies();
            return especies
            .Where(e => !string.IsNullOrWhiteSpace(e.nombre))
            .Select(e => e.nombre)
            .ToList();
        }
    }

}

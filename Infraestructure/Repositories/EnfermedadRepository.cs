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
    public class EnfermedadRepository : IEnfermedadRepository
    {
        private readonly IRealtimeService _db;

        public EnfermedadRepository(IRealtimeService db)
        {
            _db = db;
        }

        public async Task<string> CrearEnfermedad(Enfermedad enfermedad)
        {
            var id = await _db.AddAsync("enfermedades", enfermedad);

            enfermedad.idEnfermedad = id;
            await _db.UpdateAsync("enfermedades", id, enfermedad);

            return id;
        }

        public async Task<List<Enfermedad>> ObtenerEnfermedades()
        {
            var result = await _db.GetAllAsync<Enfermedad>("enfermedades");

            return result.Select(x =>
            {
                x.Data.idEnfermedad = x.Id;
                return x.Data;
            }).ToList();
        }

        public Task EditarEnfermedad(string id, Enfermedad enfermedad)
            => _db.UpdateAsync("enfermedades", id, enfermedad);


        public Task EliminarEnfermedad(string id)
            => _db.DeleteAsync("enfermedades", id);

        public async Task<List<string>> ObtenerNombreEnfermedades()
        {
            var enfermedades = await ObtenerEnfermedades();
            return enfermedades
            .Where(e => !string.IsNullOrWhiteSpace(e.nombre))
            .Select(e => e.nombre)
            .ToList();
        }

        public async Task<List<(string Id, Enfermedad Data)>> ObtenerPaginaEnfermedades(string? lastKey, int pageSize)
        {
            return await _db.GetPagedAsync<Enfermedad>("enfermedades", lastKey, pageSize);
        }
    }
}

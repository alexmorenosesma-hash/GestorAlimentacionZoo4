using Aplication.Interfaces.Firebase.Realtime;
using Aplication.Interfaces.Repositories;
using Domain.Entities;
using Infraestructure.Services.Firebase.Realtime;
using System;
using System.Collections.Generic;
using System.Text;

namespace Infraestructure.Repositories
{
    public class AnimalRepository : IAnimalRepository
    {
        private readonly IRealtimeService _db;

        public AnimalRepository(IRealtimeService db)
        {
            _db = db;
        }

        public async Task<string> CrearAnimal(Animal animal)
        {
            var id = await _db.AddAsync("animales", animal);

            animal.idAnimal = id;
            await _db.UpdateAsync("animales", id, animal);

            return id;
        }

        public async Task<List<Animal>> ObtenerAnimales()
        {
            var result = await _db.GetAllAsync<Animal>("animales");

            return result.Select(x =>
            {
                x.Data.idAnimal = x.Id;
                return x.Data;
            }).ToList();
        }

        public Task EditarAnimal(string id, Animal animal)
            => _db.UpdateAsync("animales", id, animal);


        public Task EliminarAnimal(string id)
            => _db.DeleteAsync("animales", id);

        public async Task<List<(string Id, Animal Data)>> ObtenerPaginaAnimales(string? lastKey,int pageSize)
        {
            return await _db.GetPagedAsync<Animal>("animales", lastKey, pageSize);
        }
    }
}

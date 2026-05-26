using Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Aplication.Interfaces.Repositories
{
    public interface IAnimalRepository
    {
        Task<string> CrearAnimal(Animal animal);
        Task<List<Animal>> ObtenerAnimales();
        Task EditarAnimal(string id, Animal animal);
        Task EliminarAnimal(string id);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Especie
    {
        public string idEspecie { get; set; }
        public string nombre { get; set; }
        public string nombreCientifico { get; set; }
        public string tipoAlimentacion { get; set; }
        public string tipoAnimal { get; set; }  
        public string habitat { get; set; }

        public Especie()
        {
        }

        public Especie(string nombre,string tipoAlimentacion)
        {
            this.nombre = nombre;
            this.tipoAlimentacion = tipoAlimentacion;
        }
        public Especie(string nombre, string nombreCientifico, string tipoAlimentacion, string tipoAnimal, string habitat)
        {
            this.nombre = nombre;
            this.nombreCientifico = nombreCientifico;
            this.tipoAlimentacion = tipoAlimentacion;
            this.tipoAnimal = tipoAnimal;
            this.habitat = habitat;
        }

    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Enfermedad
    {
        public string idEnfermedad;
        public string nombre { get; set; }
        public string sintomas { get; set;}

        public Enfermedad()
        {
        }
        public Enfermedad(string nombre, string sintomas)
        {
            this.nombre = nombre;
            this.sintomas = sintomas;
        }
    }
}

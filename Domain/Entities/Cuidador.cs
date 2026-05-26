using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Cuidador
    {
        public string idCuidador;
        public string nombre { get; set; }

        public Cuidador()
        {
        }

        public Cuidador(string nombre)
        {
            this.nombre = nombre;
        }
    }
}

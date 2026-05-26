using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Alimento
    {
        public string idAlimento { get; set; }
        public string nombre { get; set; }
        public double cantidad { get; set; }
        public string unidad { get; set; }
        public Alimento()
        {
        }
        public Alimento(string nombre, double cantidad,string unidad)
        {
            this.nombre = nombre;
            this.cantidad = cantidad;
            this.unidad = unidad;
        }
    }
}

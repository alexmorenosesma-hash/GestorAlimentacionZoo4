using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Dieta
    {
        public string idDieta { get; set; }
        public string nombre { get; set; }
        public string tipoAlimentacion { get; set; }
        public List<AlimentoCantidad> alimentos { get; set; } = new();
        public Dieta()
        {
        }
        public Dieta(string nombre, string tipoAlimentacion, List<AlimentoCantidad> alimentos)
        {
            this.nombre = nombre;
            this.tipoAlimentacion = tipoAlimentacion;
            this.alimentos = alimentos;
        }
    }
}

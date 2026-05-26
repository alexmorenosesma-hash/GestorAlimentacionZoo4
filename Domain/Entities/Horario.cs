using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Horario
    {
        public string idHorario { get; set; }
        public string hora{get;set;}
        public Horario()
        {
        }
        public Horario(string hora) {
            this.hora = hora;
        }
    }
}

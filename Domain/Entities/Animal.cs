using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Entities
{
    public class Animal
    {
        public string idAnimal { get; set; }
        public string nombre { get; set; }
        public int edad { get; set; }
        public int peso { get; set; }
        public string especie { get; set; }
        public string dieta {get;set; }
        public string cuidador { get; set; }
        public List<string> horariosAlimentacion{get;set; }
        public List<string> enfermedades { get;set; }
        public string HorariosTexto { get; set; } = "Sin horarios";
        public string EnfermedadesTexto { get; set; } = "Sin enfermedades";

        public Animal()
        {
        }
        public Animal(string idAnimal, string nombre, int edad, int peso, string especie, string dieta, string cuidador, List<string> horariosAlimentacion, List<string> enfermedades, string horariosTexto, string enfermedadesTexto)
        {
            this.idAnimal = idAnimal;
            this.nombre = nombre;
            this.edad = edad;
            this.peso = peso;
            this.especie = especie;
            this.dieta = dieta;
            this.cuidador = cuidador;
            this.horariosAlimentacion = horariosAlimentacion;
            this.enfermedades = enfermedades;
            HorariosTexto = horariosTexto;
            EnfermedadesTexto = enfermedadesTexto;
        }

        public Animal(int edad, int peso, string especie, string dieta, List<string> horariosAlimentacion, List<string> enfermedades, string cuidador)
        {
            this.edad = edad;
            this.peso = peso;
            this.especie = especie;
            this.dieta = dieta;
            this.horariosAlimentacion = horariosAlimentacion;
            this.enfermedades = enfermedades;
            this.cuidador = cuidador;
        }
    }
}

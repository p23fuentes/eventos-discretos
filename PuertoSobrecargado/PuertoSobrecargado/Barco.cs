using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuertoSobrecargado
{
    public enum TipoBarco { Pequeño, Mediano, Grande };

    class Barco
    {
        public double llegadaAlPuerto;
        public double atendido;
        public double salidaDelMuelle;
        public double llegadaAlMuelle;
        public int nombre;
        public double finCarga;
        
        public TipoBarco tipo { get; set; }
        Random random=new Random();
        public Barco(double arribo, int nombre)
        {
            double r = random.NextDouble();
            if (r <= 0.25) tipo=TipoBarco.Pequeño;
            else if (r <= 0.5) tipo=TipoBarco.Mediano;
            else tipo=TipoBarco.Grande;

            llegadaAlPuerto = arribo;
            this.nombre = nombre;
        }

        public double TiempoEsperaEnMuelle() { return salidaDelMuelle - finCarga; }
        public double TiempoEsperaEnPuerto() { return atendido - llegadaAlPuerto; }
       
    }
}

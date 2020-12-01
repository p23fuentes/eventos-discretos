using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuertoSobrecargado
{
    class Simulacion
    {
        Queue<Barco> barcosEnPuerto;
       public List<Barco> barcosAtendidos;
        List<Barco> barcosEnMuelle;
        List<Tuple<double, string>> eventos;

        Remolcador remolcador;
        Barco barco;
        double tiempo;
        double arribo;
        double duracion;
        int cantidadMuelles;
        int contNombres;
        Random random;
        Distribuciones Distribuciones = new Distribuciones();


        public List<Tuple<double, string>> IniciaSimulacion()
        {
            while (tiempo < duracion)
            {
                contNombres++;
                arribo += ArriboBarcos();
                barco = new Barco(arribo, contNombres);
                barcosEnPuerto.Enqueue(barco);
                eventos.Add(new Tuple<double, string>(arribo, "El Tanquero " + barco.nombre.ToString() + " arribo al Puerto"));


                if (remolcador.posicion == Posicion.Puerto)
                {
                    if (barcosEnMuelle.Count != 0) //Hay barcos en el muelle 
                    {
                        if (barcosEnPuerto.Count != 0 && barcosEnPuerto.Peek().llegadaAlPuerto <= tiempo) AtiendePuerto();
                        else
                        {
                            //Viaja al muelle solo 
                            eventos.Add(new Tuple<double, string>(tiempo, "Remolcador viaja hacia el Muelle sin Tanquero"));
                            tiempo += Distribuciones.Exponencial(15); 
                            eventos.Add(new Tuple<double, string>(tiempo, "Remolcador llega al Muelle sin Tanquero"));

                            remolcador.posicion = Posicion.Muelle;
                        }
                    }
                    else
                    {
                        if (barcosEnPuerto.Count != 0)
                        {
                            tiempo = Math.Max(tiempo, barcosEnPuerto.Peek().llegadaAlPuerto);
                            AtiendePuerto();
                        }

                    }

                }
                else
                {
                    if (barcosEnPuerto.Count != 0)
                    {
                        if (!MuelleLibre()) //no hay muelle libre
                        {
                            tiempo = Math.Max(tiempo, barcosEnMuelle[0].finCarga);
                            AtiendeMuelle();
                        }
                        else
                        {
                            if (barcosEnMuelle[0].finCarga <= barcosEnPuerto.Peek().llegadaAlPuerto)
                            {
                                tiempo = Math.Max(tiempo, barcosEnMuelle[0].finCarga);
                                AtiendeMuelle();
                            }
                            else
                            {
                                //Viaja al puerto solo 
                                tiempo = Math.Max(tiempo, barcosEnPuerto.Peek().llegadaAlPuerto);
                                eventos.Add(new Tuple<double, string>(tiempo, "Remolcador viaja hacia el Puerto sin Tanquero"));
                                tiempo += Distribuciones.Exponencial(15);
                                eventos.Add(new Tuple<double, string>(tiempo, "Remolcador llega al Puerto sin Tanquero"));

                                remolcador.posicion = Posicion.Puerto;
                                AtiendePuerto();
                            }

                        }

                    }

                }

            }

            return eventos;
        }

        public Simulacion(double duracion,int cantMuelles=3)
        {
            random = new Random();
            barcosEnPuerto = new Queue<Barco>();
            barcosAtendidos = new List<Barco>();
            barcosEnMuelle = new List<Barco>();

            eventos = new List<Tuple<double, string>>();
            
            cantidadMuelles = cantMuelles;
            remolcador = new Remolcador();
            tiempo = 0;
            arribo = 0;
            contNombres = 0;
            this.duracion = duracion;
        }

        private bool MuelleLibre()
        {
            return !(barcosEnMuelle.Count == cantidadMuelles); 
        }
        private void AtiendePuerto() 
        {
                barco = barcosEnPuerto.Dequeue();
                barco.atendido = tiempo;

                eventos.Add(new Tuple<double, string>(tiempo, "Remolcador sale hacia el Muelle con Tanquero "+barco.nombre.ToString()));
                tiempo+=LlevarBarcoAlMuelle();
                eventos.Add(new Tuple<double, string>(tiempo, "Remolcador llega al Muelle con Tanquero " + barco.nombre.ToString()));

                barco.llegadaAlMuelle = tiempo;
                remolcador.posicion = Posicion.Muelle;
                InsertaMuelle(barco);            
        }
        private void AtiendeMuelle()
        {
            barco = barcosEnMuelle[0];
            barcosEnMuelle.RemoveAt(0);
            barco.salidaDelMuelle = tiempo;

            eventos.Add(new Tuple<double, string>(tiempo, "Remolcador sale hacia el Puerto con Tanquero " + barco.nombre.ToString()));
            tiempo += SacarBarcoDelPuerto();

            if (tiempo <= duracion)  barcosAtendidos.Add(barco); //Puede que se acabe el tiempo estando en el mar
            eventos.Add(new Tuple<double, string>(tiempo, "Remolcador llega al Puerto con Tanquero " + barco.nombre.ToString()));
            
            remolcador.posicion = Posicion.Puerto;
        }

        private void InsertaMuelle(Barco barco) //Reorganiza la lista de las terminaciones de carga
        {

            this.barco=barco;
            barco.finCarga = barco.llegadaAlMuelle + TiempoCarga(barco);

            eventos.Add(new Tuple<double, string>(barco.finCarga, "El Tanquero " + barco.nombre.ToString() + " termina su carga"));

            for (int i = 0; i < barcosEnMuelle.Count; i++)
            {
                if (barcosEnMuelle[i].finCarga > barco.finCarga) { barcosEnMuelle.Insert(i, barco); return; }
            }

            barcosEnMuelle.Add(barco);

        }
        

        double ArriboBarcos()
        {
            return Distribuciones.Exponencial(8) * 60;
        }

        protected double LlevarBarcoAlMuelle()
        {
            return Distribuciones.Exponencial(2) * 60;
        }

        protected double SacarBarcoDelPuerto()
        {
            return Distribuciones.Exponencial(1) * 60;
        }

        private double TiempoCarga(Barco b) // genera el tiempo de carga de acuerdo a los tamaños de los barcos.
        {
            double d = 0;
            switch (b.tipo)
            {
                case TipoBarco.Pequeño:
                    d = Distribuciones.Normal(9, 1);
                    break;
                case TipoBarco.Mediano:
                    d = Distribuciones.Normal(12, 2);
                    break;
                case TipoBarco.Grande:
                    d = Distribuciones.Normal(18, 3);
                    break;
                default:
                    break;
            }

            return d * 60;
        }
    }
}

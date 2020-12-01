using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Threading.Tasks;

namespace PuertoSobrecargado
{
    class Program
    {
        static List<Tuple<double, string>> eventos;
        static Simulacion sim;
        static int tiempo;
        static double acumulado = 0;
        static StreamWriter text;
        static int vez=0;
        static int cantVez = 0;
        static string aux;
        static int totalBarcos = 0;
        static double espera = 0;

        static void Main(string[] args)
        {
            Console.WriteLine("Introdusca el tiempo para la Simulacion (en horas): ");
            tiempo =int.Parse(Console.ReadLine())*60;
            Console.WriteLine("Indique la cantidad de Simulaciones a realizar: ");
            cantVez = int.Parse(Console.ReadLine());

            while (vez < cantVez)
            {
                vez++;
                string name = "Simulacion " + vez + ".txt";
                text = new StreamWriter(name);

                sim = new Simulacion(tiempo);
                eventos = sim.IniciaSimulacion();
                eventos.Sort();

                aux = "\n Comienza Simulacion";
                Console.WriteLine(aux + " " + vez + "\n");
                text.WriteLine(aux +"\n");

                foreach (var e in eventos)
                {
                    if (e.Item1 <= tiempo)
                    {
                        aux = e.Item1 + "  :  " + e.Item2;
                        Console.WriteLine(aux);
                        text.WriteLine(aux);
                    }
                }
                

                aux="\n Termina Simulacion";
                Console.WriteLine(aux+"\n");
                text.WriteLine(aux+"\n");

                aux = "Tiempo de Simulacion: " + tiempo + " minutos";
                Console.WriteLine(aux);
                text.WriteLine(aux);


                aux = "***********************************";
                Console.WriteLine(aux);
                text.WriteLine(aux);

                foreach (var item in sim.barcosAtendidos)
                {
                    double temp1 = item.TiempoEsperaEnPuerto();
                    double temp2 = item.TiempoEsperaEnMuelle();
             
                    aux = "Espera del Tanquero " + item.nombre + " en el Puerto: " + temp1 + " minutos";
                    Console.WriteLine(aux);
                    text.WriteLine(aux);

                    aux = "Espera del Tanquero " + item.nombre + " en el Muelle: " + temp2 + " minutos";
                    Console.WriteLine(aux);
                    text.WriteLine(aux);

                    aux = "Espera del Tanquero " + item.nombre + " Total: " + temp1 + temp2 + " minutos";
                    Console.WriteLine(aux);
                    text.WriteLine(aux);

                    acumulado += temp1 + temp2;
                }

                aux ="--------------------------------------------";
                Console.WriteLine(aux);
                text.WriteLine(aux);

                aux = "Total de Tanqueros atendidos: " + sim.barcosAtendidos.Count;
                Console.WriteLine(aux);
                text.WriteLine(aux);
               

                if (sim.barcosAtendidos.Count != 0)
                {
                    aux = "Promedio de Espera de los Tanqueros";
                    Console.WriteLine(aux);
                    text.WriteLine(aux);

                    aux = acumulado / sim.barcosAtendidos.Count + " minutos";
                    Console.WriteLine(aux);
                    text.WriteLine(aux);
                }

           text.Close();
           espera += acumulado / sim.barcosAtendidos.Count;
           acumulado = 0;
           totalBarcos += sim.barcosAtendidos.Count;

           Console.WriteLine("***********************************");
                
            }

            Console.WriteLine("Media de barcos atendidos "+ totalBarcos/cantVez);
            Console.WriteLine("Media de espera de barcos " + espera / cantVez);
          Console.ReadLine();
        }

    }
}

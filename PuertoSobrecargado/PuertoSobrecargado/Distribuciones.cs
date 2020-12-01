using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuertoSobrecargado
{
    class Distribuciones
    {
        Random random=new Random();
        public double Exponencial(double lambda)
        {
            double r = random.NextDouble();
            return (-1 / lambda) * (Math.Log(r));
        }

        public double Normal(double mu, double sigma)
        {
            double a = random.NextDouble();
            double b = random.NextDouble();
            double c = Math.Sqrt(-2 * Math.Log(a)) * Math.Cos(2 * Math.PI * b);
            return Math.Sqrt(sigma) * c + mu;
        }
    }
}

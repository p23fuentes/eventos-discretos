using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PuertoSobrecargado
{
    public enum Posicion { Puerto, Muelle };
    class Remolcador
    {
        public Posicion posicion;
        public Remolcador()
        {
            posicion = Posicion.Puerto;
        }

    }
}

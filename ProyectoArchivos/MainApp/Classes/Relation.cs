using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoArchivos.MainApp.Classes
{
    class Relation
    {
        public string tablaOrigen { get; set; }
        public string attrOrigen { get; set; }
        public string tablaDestino { get; set; }

        public Relation(List<string> l)
        {
            tablaOrigen = l[0];
            attrOrigen = l[1];
            tablaDestino = l[2];
        }

        public Relation(string to, string ao, string td)
        {
            tablaOrigen = to;
            attrOrigen = ao;
            tablaDestino = td;
        }
    }
}

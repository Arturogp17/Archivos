using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoArchivos.MainApp.Classes
{
    public class Register
    {
        public long dir { get; set; }
        public object val { get; set; }
        public long nextDir { get; set; }
        public long address { get; set; }
        public int id { get; set; }
        public List<long> addr { get; set; }
        public List<long> addrNext { get; set; }
        public List<long> bloque { get; set; }
        public List<List<Register>> values { get; set; }
        public long next { get; set; }
    }
}

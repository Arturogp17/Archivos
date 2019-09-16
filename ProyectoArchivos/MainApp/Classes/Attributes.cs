using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoArchivos.Models
{
    public class Attributes
    {
        public string id { get; set; }
        public string name { get; set; }
        public char dataType { get; set; }
        public int length { get; set; }
        public long address { get; set; }
        public int indexType { get; set; }
        public long indexAddress { get; set; }
        public long nextAttributeAddress { get; set; }
    }
}

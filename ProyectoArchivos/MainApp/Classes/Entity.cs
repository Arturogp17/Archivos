using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProyectoArchivos.Models
{
    public class Entity
    {
        public string id { get; set; }
        public string name { get; set; }
        public long address { get; set; }
        public long attributeAddress { get; set; }
        public long dataAddress { get; set; }
        public long nextEntityAddress { get; set; }
        public List<Attributes> attributes = new List<Attributes>();
    }
}

﻿using System;
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
        public List<Register> bloque { get; set; }
    }
}
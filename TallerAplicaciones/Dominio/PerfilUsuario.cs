﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public abstract class PerfilUsuario
    {

        public int PerfilUsuarioID { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Email { get; set; }
        public string Login { get; set; }

        public bool Activo { get; set; }
        
    }
}

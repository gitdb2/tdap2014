using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public abstract class Usuario
    {
        public int UsuarioID { get; set; }

        public string Nombre { get; set; }
        public string Apellido { get; set; }
        public string Login { get; set; }
        public string Contrasena { get; set; }
        public string Email { get; set; }

    }
}

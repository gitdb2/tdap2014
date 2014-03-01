using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;


namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public  class Usuario
    {
    
        public int UsuarioID { get; set; }

        //public string Nombre { get; set; }
        //public string Apellido { get; set; }
        public string Login { get; set; }
        public string Contrasena { get; set; }
        //public string Email { get; set; }

        public Usuario() { }

    }
}

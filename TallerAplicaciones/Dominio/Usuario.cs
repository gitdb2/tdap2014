using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel.DataAnnotations;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public class Usuario
    {
        public int UsuarioID { get; set; }

        public string Login { get; set; }

        public bool Activo { get; set; }
    }
}

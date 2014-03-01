using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace uy.edu.ort.taller.aplicaciones.negocio
{
    public class Persistencia : DbContext
    {

        public Persistencia()
            : base("DefaultConnection")
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }
        
        public DbSet<PerfilUsuario> PerfilesUsuario { get; set; }

    }
}

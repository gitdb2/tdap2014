using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace Business
{
    public class Persistencia : DbContext
    {

        public Persistencia()
            : base("DefaultConnection")
        {

        }

        public DbSet<Usuario> Usuarios { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.dominio;
using System.Data.Entity;

namespace uy.edu.ort.taller.aplicaciones.negocio
{
    public class Persistencia : DbContext
    {

            public Persistencia() : base("DefaultConnection")
            {
            }

            public DbSet<Usuario> Usuarios { get; set; }
    }
}

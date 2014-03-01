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

        public DbSet<PerfilUsuario> PerfilesUsuario { get; set; }

        public void InitializeDatabase(Persistencia context)
        {
            if (context.Database.Exists() && !context.Database.CompatibleWithModel(false))
            {
                context.Database.Delete();
            }

            if (!context.Database.Exists())
            {
                context.Database.Create();
                context.Database.ExecuteSqlCommand("ALTER TABLE \"Usuario\" ADD CONSTRAINT UniqueLogin UNIQUE (login)");
            }
        }


    }
}

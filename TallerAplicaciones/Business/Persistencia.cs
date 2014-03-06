using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using uy.edu.ort.taller.aplicaciones.dominio;
using System.Data.Entity.ModelConfiguration.Conventions;

namespace uy.edu.ort.taller.aplicaciones.negocio
{
    public class Persistencia : DbContext
    {

        public Persistencia()
            : base("DefaultConnection")
        {

        }

        public DbSet<PerfilUsuario> PerfilesUsuario { get; set; }

        public DbSet<EmpresaDistribuidora> Empresas { get; set; }

        public DbSet<Producto> Productos { get; set; }

        public DbSet<Atributo> Atributos { get; set; }

        public DbSet<ValorPredefinido> ValoresPredefinidos { get; set; }

        public DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
        }
       
    }

    public class PersistenciaInitializer : IDatabaseInitializer<Persistencia>
    {
        public void InitializeDatabase(Persistencia context)
        {
            if (context.Database.Exists() && !context.Database.CompatibleWithModel(false))
            {
                context.Database.Delete();
            }
            if (!context.Database.Exists())
            {
                context.Database.Create();
            }
        }
    }


}

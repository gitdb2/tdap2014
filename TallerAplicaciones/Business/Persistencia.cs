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
        static Persistencia()
        {
            Database.SetInitializer<Persistencia>(new PersistenciaInitializer());
        }

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

        public DbSet<Pedido> Pedidos { get; set; }

        public DbSet<Archivo> Archivos { get; set; }

        public DbSet<CantidadProductoPedido> CantidadProductosPedido { get; set; }

        public DbSet<ValorAtributo> ValoresAtributos { get; set; }

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

            context.Database.ExecuteSqlCommand("ALTER TABLE Producto ADD UNIQUE (Codigo) ");

            try
            {
                context.Database.ExecuteSqlCommand("CREATE TABLE [dbo].[Log] ("
                                                   + "[Id] [int] IDENTITY (1, 1) NOT NULL,"
                                                   + "[Date] [datetime] NOT NULL,"
                                                   + "[Login] [varchar] (255) NOT NULL,"
                                                   + "[Thread] [varchar] (255) NOT NULL,"
                                                   + "[Level] [varchar] (50) NOT NULL,"
                                                   + "[Logger] [varchar] (255) NOT NULL,"
                                                   + "[Message] [varchar] (4000) NOT NULL,"
                                                   + "[Exception] [varchar] (2000) NULL)");
            }
            catch (Exception oculta)
            {
                //si esto revienta que pasa?
            }

        }
    }


}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.dominio.Exceptions;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace uy.edu.ort.taller.aplicaciones.negocio
{
    public class ManejadorProducto : IProducto
    {
        #region singleton
        private static ManejadorProducto instance = new ManejadorProducto();

        private ManejadorProducto() { }

        public static ManejadorProducto GetInstance()
        {
            return instance;
        }
        #endregion

        public void AltaProducto(Producto producto)
        {
            try
            {
                using (var db = new Persistencia())
                {
                    db.Productos.Add(producto);
                    db.SaveChanges();
                }

            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                if (e.InnerException.InnerException.Message.Contains("UNIQUE KEY"))
                {
                    throw new ValorDuplicadoException("El codigo ya existe", e);
                }

                throw;
            }

        }

        public void BajaProducto(int idProducto)
        {
            using (var db = new Persistencia())
            {
                var producto = db.Productos.SingleOrDefault(p => p.ProductoID == idProducto);
                if (producto != null)
                {
                    producto.Activo = false;
                    db.SaveChanges();
                }

            }
        }


        public void ModificarProducto(Producto producto)
        {
            try
            {
                using (var db = new Persistencia())
                {
                    var productoDb = db.Productos.SingleOrDefault(p => p.ProductoID == producto.ProductoID);
                    if (productoDb != null)
                    {
                        productoDb.Activo = producto.Activo;
                        productoDb.Codigo = producto.Codigo;
                        productoDb.Descripcion = producto.Descripcion;
                        productoDb.Nombre = producto.Nombre;

                        db.SaveChanges();
                    }
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                if (e.InnerException.InnerException.Message.Contains("UNIQUE KEY"))
                {
                    throw new ValorDuplicadoException("El codigo ya existe", e);
                }

                throw;
            }
        }

        public List<Producto> ListarProductos()
        {
            using (var db = new Persistencia())
            {
                return db.Productos.ToList();
            }
        }

        public void AgregarImagenProducto(int idProducto, Foto imagen)
        {
            using (var db = new Persistencia())
            {
                var productoDb = db.Productos.Include("Archivos").SingleOrDefault(p => p.ProductoID == idProducto);
                if (productoDb != null)
                {
                    productoDb.Archivos.Add(imagen);

                    db.SaveChanges();
                }

            }
        }

        public void AgregarVideoProducto(int idProducto, Video video)
        {
            using (var db = new Persistencia())
            {
                var productoDb = db.Productos.Include("Archivos").SingleOrDefault(p => p.ProductoID == idProducto);
                if (productoDb != null)
                {
                    if (productoDb.Archivos == null)
                    {
                        productoDb.Archivos = new List<Archivo>();
                    }
                    productoDb.Archivos.Add(video);

                    db.SaveChanges();
                }

            }
        }

        public List<Foto> GetFotosProducto(int idProducto)
        {
            var ret = new List<Foto>();
            using (var db = new Persistencia())
            {
                var productoDb = db.Productos.Include("Archivos").SingleOrDefault(p => p.ProductoID == idProducto);
                if (productoDb != null)
                {
                    if (productoDb.Archivos.OfType<Video>().Any())
                    {
                        return productoDb.Archivos.OfType<Foto>().ToList();
                    }
                }
            }
            return ret;
        }

        public List<Video> GetVideosProducto(int idProducto)
        {
            var ret = new List<Video>();
            using (var db = new Persistencia())
            {
                var productoDb = db.Productos.Include("Archivos").SingleOrDefault(p => p.ProductoID == idProducto);
                if (productoDb != null)
                {
                    if (productoDb.Archivos.OfType<Video>().Any())
                    {
                        return productoDb.Archivos.OfType<Video>().ToList();
                    }

                }
            }
            return ret;
        }

        public void AsignarFotos(int idProducto, List<string> fotoDirs)
        {
            if (fotoDirs.Count == 0) return;
            var archivos = fotoDirs.Select(item => new Foto
            {
                Activo = true,
                Nombre = item,
                URL = item
            }).ToList();
            AsignarArchivo(idProducto, archivos);
        }

        public void AsignarVideos(int idProducto, List<string> videoDirs)
        {
            if (videoDirs.Count == 0) return;
            var archivos = videoDirs.Select(item => new Video
            {
                Activo = true,
                Nombre = item,
                URL = item
            }).ToList();
            AsignarArchivo(idProducto, archivos);
        }




        private void AsignarArchivo(int idProducto, IEnumerable<Archivo> archivos)
        {
            using (var db = new Persistencia())
            {
                var productoDb = db.Productos.Include("Archivos").SingleOrDefault(p => p.ProductoID == idProducto);
                if (productoDb != null && archivos.Any())
                {
                    if (productoDb.Archivos == null)
                    {
                        productoDb.Archivos = new List<Archivo>();
                    }
                    foreach (var archivo in archivos)
                    {
                        productoDb.Archivos.Add(archivo);

                    }
                    db.SaveChanges();
                }
            }
        }


        public void AltaProducto(Producto producto, List<string> fotoDirs, List<string> videoDirs)
        {

            try
            {
                using (var db = new Persistencia())
                {
                     producto.Archivos = new List<Archivo>();

                    if (videoDirs.Count > 0)
                    {
                        foreach (var item in fotoDirs)
                        {
                            producto.Archivos.Add(new Video
                            {
                                Activo = true,
                                Nombre = item,
                                URL = item
                            });
                        }
                    }

                    if (fotoDirs.Count > 0)
                    {
                        foreach (var item in fotoDirs)
                        {
                            producto.Archivos.Add(new Foto
                            {
                                Activo = true,
                                Nombre = item,
                                URL = item
                            });
                        }
                    }

                    db.Productos.Add(producto);
                    db.SaveChanges();
                }

            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                if (e.InnerException.InnerException.Message.Contains("UNIQUE KEY"))
                {
                    throw new ValorDuplicadoException("El codigo ya existe", e);
                }

                throw;
            }
      
        }
    }
}

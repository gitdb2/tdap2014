using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.dominio.DTO;
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

        public bool BajaProducto(int idProducto)
        {
            using (var db = new Persistencia())
            {
                var producto = db.Productos.SingleOrDefault(p => p.ProductoID == idProducto);
                if (producto != null)
                {
                    producto.Activo = false;
                    return db.SaveChanges() > 0;
                }
                else
                {
                    throw new Exception("No se pudo encontrar ese peroducto");
                }

            }
            return false;
        }

        public List<Producto> ListarProductos()
        {
            using (var db = new Persistencia())
            {
                return db.Productos.Include("Archivos").ToList();
            }
        }

        public List<ProductoDTO> ListarProductosDTO()
        {
            var resultado = new List<ProductoDTO>();
            using (var db = new Persistencia())
            {
                var aDevolver = db.Productos.Where(p => p.Activo).ToList();
                if (aDevolver.Any())
                {
                    foreach (var producto in aDevolver)
                    {
                        resultado.Add(new ProductoDTO()
                        {
                            ProductoId = producto.ProductoID,
                            Codigo = producto.Codigo,
                            Nombre = producto.Nombre,
                            Descripcion = producto.Descripcion
                        });
                    }
                }
            }
            return resultado;
        }

        //TODO
        public List<ValorAtributoDTO> ListarAtributosProductoDTO(int idProducto)
        {
            //dummy porque no hay valores de atributo aun
            var resultado = new List<ValorAtributoDTO>();

            ValorAtributoDTO va1 = new ValorAtributoDTO() { Nombre = "atributo1" };
            ValorAtributoDTO va2 = new ValorAtributoDTO() { Nombre = "atributo2" };
            ValorAtributoDTO va3 = new ValorAtributoDTO() { Nombre = "atributo2" };

            List<ValorDTO> v1 = new List<ValorDTO>();
            v1.Add(new ValorDTO(){ValorString = "aaaaa"});
            v1.Add(new ValorDTO() { ValorString = "bbbbb" });

            List<ValorDTO> v2 = new List<ValorDTO>();
            v2.Add(new ValorDTO() { ValorString = "ccccc" });
            v2.Add(new ValorDTO() { ValorString = "ddddd" });

            List<ValorDTO> v3 = new List<ValorDTO>();
            v3.Add(new ValorDTO() { ValorString = "eeeee" });
            v3.Add(new ValorDTO() { ValorString = "fffff" });

            va1.Valores = v1;
            va2.Valores = v2;
            va3.Valores = v3;

            resultado.Add(va1);
            resultado.Add(va2);
            resultado.Add(va3);

            return resultado;
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
                Url = item
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
                Url = item
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

        public Producto GetProducto(int idProducto)
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
                  
                }
                return productoDb;
            }
        }

        public void Modificar(Producto productoUpdate, List<int> filesToDelete)
        {
            try
            {
                using (var db = new Persistencia())
                {

                    bool ok = true;
                    string message = "";
                    string key = "";

                    var productoDb = db.Productos.Include("Archivos").SingleOrDefault(p => p.ProductoID == productoUpdate.ProductoID);
                    if (productoDb != null)
                    {
                        productoDb.Activo = productoUpdate.Activo;
                        productoDb.Descripcion = productoUpdate.Descripcion;
                        productoDb.Nombre = productoUpdate.Nombre;
                        productoDb.Codigo = productoUpdate.Codigo;

                        if (productoDb.Archivos == null)
                        {
                            productoDb.Archivos  = new List<Archivo>();
                        }

                        if (productoDb.Archivos.Any() && filesToDelete!=null && filesToDelete.Any())
                        {
                           productoDb.Archivos.RemoveAll(a => filesToDelete.Contains(a.ArchivoID));

                           var archivosABorrar   =  db.Archivos
                                                  .Where(a => filesToDelete.Contains(a.ArchivoID))
                                                  .ToList();

                           foreach (var arch in archivosABorrar)
                           {
                               db.Archivos.Remove(arch);
                           }

                           
                        }

                        if (productoUpdate.Archivos != null && productoUpdate.Archivos.Any())
                        {
                            foreach (var archivo in productoUpdate.Archivos)
                            {
                                productoDb.Archivos.Add(archivo);
                            }
                        }

                        if (ok)
                        {
                            db.SaveChanges();
                        }
                        else
                        {
                            throw new CustomException(message){Key = key};
                        }
                    }
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                if (e.InnerException.InnerException.Message.Contains("UNIQUE KEY"))
                {
                    throw new ValorDuplicadoException("El codigo >"+productoUpdate.Codigo+"< ya existe", e);
                }

                throw;
            }
        }

    }
}

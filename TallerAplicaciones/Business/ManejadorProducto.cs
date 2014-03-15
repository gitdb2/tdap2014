using System;
using System.Collections.Generic;
using System.Data.Entity;
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

        public void AltaProducto(Producto producto, List<int> idAtributoSimple, List<string> valorAtributoSimple, List<string> valorAtributoCombo, List<string> valorAtributoMulti)
        {
            try
            {
                //producto.ValoresSeleccionados = ParserAtributosModel(idAtributoSimple, valorAtributoSimple, valorAtributoCombo, valorAtributoMulti);

                using (var db = new Persistencia())
                {
                    ///////////////////////////////////////////////////////////
                    List<ValorAtributo> aRetornar = new List<ValorAtributo>();

                    int i = 0;
                    if ((idAtributoSimple != null) && (idAtributoSimple.Any()))
                    {
                        foreach (var idAtrib in idAtributoSimple)
                        {
                            //AtributoSimple atributo = (AtributoSimple)iAtributo.GetAtributo(idAtrib);
                            AtributoSimple atributo = db.Atributos.OfType<AtributoSimple>().SingleOrDefault(a => a.AtributoID == idAtrib);

                            var valorAtributo = new ValorAtributoSimple()
                            {
                                Valor = valorAtributoSimple[i],
                                Atributo = atributo
                            };
                            aRetornar.Add(valorAtributo);
                            i++;
                        }
                    }
                    if ((valorAtributoCombo != null) && (valorAtributoCombo.Any()))
                    {
                        foreach (var idYValor in valorAtributoCombo)
                        {
                            string[] idAtributo = idYValor.Split(new char[] { '|' });
                            int idAtrib = Convert.ToInt16(idAtributo[0]);
                            int idValor = Convert.ToInt16(idAtributo[1]);

                            //AtributoCombo atributo = (AtributoCombo)iAtributo.GetAtributo(idAtrib);
                            AtributoCombo atributo =
                                db.Atributos.OfType<AtributoCombo>().SingleOrDefault(a => a.AtributoID == idAtrib);

                            //ValorPredefinido valorPredefinido = iAtributo.GetValorPredefinido(idValor);
                            ValorPredefinido valorPredefinido =
                                db.ValoresPredefinidos.SingleOrDefault(a => a.ValorPredefinidoID == idValor);

                            var listaValorPredefinido = new List<ValorPredefinido>();
                            listaValorPredefinido.Add(valorPredefinido);
                            var ValorAtributo = new ValorAtributoCombo()
                            {
                                Atributo = atributo,
                                Valores = listaValorPredefinido
                            };
                            aRetornar.Add(ValorAtributo);
                        }
                    }
                    if ((valorAtributoMulti != null) && (valorAtributoMulti.Any()))
                    {
                        foreach (var idYValor in valorAtributoMulti)
                        {
                            string[] idAtributo = idYValor.Split(new char[] { '|' });
                            int idAtrib = Convert.ToInt16(idAtributo[0]);
                            int idValor = Convert.ToInt16(idAtributo[1]);
                            //AtributoCombo atributo = (AtributoCombo)iAtributo.GetAtributo(idAtrib);
                            AtributoCombo atributo =
                                db.Atributos.OfType<AtributoCombo>().SingleOrDefault(a => a.AtributoID == idAtrib);

                            //ValorPredefinido valorPredefinido = iAtributo.GetValorPredefinido(idValor);
                            ValorPredefinido valorPredefinido =
                                db.ValoresPredefinidos.SingleOrDefault(a => a.ValorPredefinidoID == idValor);
                            var listaValorPredefinido = new List<ValorPredefinido>();
                            listaValorPredefinido.Add(valorPredefinido);
                            var ValorAtributo = new ValorAtributoCombo()
                            {
                                Atributo = atributo,
                                Valores = listaValorPredefinido
                            };
                            aRetornar.Add(ValorAtributo);
                        }
                    }
                    producto.ValoresSeleccionados = aRetornar;
                    ///////////////////////////////////////////////////////////


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
            var resultado = new List<ValorAtributoDTO>();
            using (var db = new Persistencia())
            {
                Producto producto = db.Productos
                    .Include(p0 => p0.ValoresSeleccionados)
                    .SingleOrDefault(p1 => p1.ProductoID == idProducto);

                if (producto != null)
                {
                    resultado.AddRange(ObtenerValoresSimplesDTO(producto));
                    resultado.AddRange(ObtenerValoresComboDTO(producto));
                }
            }
            return resultado;
        }

        private List<ValorAtributoDTO> ObtenerValoresSimplesDTO(Producto producto)
        {
            List<ValorAtributoDTO> resultado = new List<ValorAtributoDTO>();
            List<ValorAtributoSimple> valoresSimples = ObtenerValoresSimples(producto);
            if (valoresSimples.Any())
            {
                foreach (var valorAtributoSimple in valoresSimples)
                {
                    ValorAtributoDTO vdto = new ValorAtributoDTO();
                    vdto.Nombre = valorAtributoSimple.Atributo.Nombre;
                    vdto.Valores = new List<ValorDTO>();
                    vdto.Valores.Add(new ValorDTO() { ValorString = valorAtributoSimple.Valor });
                    resultado.Add(vdto);
                }
            }
            return resultado;
        }

        private List<ValorAtributoSimple> ObtenerValoresSimples(Producto producto)
        {
            List<ValorAtributoSimple> resultado = new List<ValorAtributoSimple>();
            List<int> ids = ObtenerIdsAtributoSimple(producto);
            using (var db = new Persistencia())
            {
                return db.ValoresAtributos
                    .OfType<ValorAtributoSimple>()
                    .Where(v0 => ids.Contains(v0.ValorAtributoID))
                    .Include(v1 => v1.Atributo)
                    .ToList();
            }
        }

        private List<int> ObtenerIdsAtributoSimple(Producto producto)
        {
            var resultado = new List<int>();
            foreach (var valor in producto.ValoresSeleccionados.OfType<ValorAtributoSimple>())
            {
                resultado.Add(valor.ValorAtributoID);
            }
            return resultado;
        }

        private List<ValorAtributoDTO> ObtenerValoresComboDTO(Producto producto)
        {
            List<ValorAtributoDTO> resultado = new List<ValorAtributoDTO>();
            Dictionary<Atributo, List<ValorPredefinido>> mapaCombos = ObtenerMapaValoresCombo(producto);
            if (mapaCombos.Any())
            {
                foreach (KeyValuePair<Atributo, List<ValorPredefinido>> combo in mapaCombos)
                {
                    ValorAtributoDTO vdto = new ValorAtributoDTO();
                    vdto.Nombre = combo.Key.Nombre;
                    vdto.Valores = new List<ValorDTO>();
                    if (combo.Value.Any())
                    {
                        foreach (var valorPredefinido in combo.Value)
                        {
                            vdto.Valores.Add(new ValorDTO()
                            {
                                ValorString = valorPredefinido.Valor
                            });
                        }
                    }
                    resultado.Add(vdto);
                }
            }
            return resultado;
        }

        private Dictionary<Atributo, List<ValorPredefinido>> ObtenerMapaValoresCombo(Producto producto)
        {
            Dictionary<Atributo, List<ValorPredefinido>> resultado = new Dictionary<Atributo, List<ValorPredefinido>>();
            List<ValorAtributoCombo> listaCombo = new List<ValorAtributoCombo>();
            List<int> ids = ObtenerIdsAtributoCombo(producto);
            using (var db = new Persistencia())
            {
                listaCombo = db.ValoresAtributos
                    .OfType<ValorAtributoCombo>()
                    .Where(v0 => ids.Contains(v0.ValorAtributoID))
                    .Include(v1 => v1.Valores)
                    .Include(v2 => v2.Atributo)
                    .ToList();

                foreach (var valorAtributoCombo in listaCombo)
                {
                    if (resultado.ContainsKey(valorAtributoCombo.Atributo))
                    {
                        resultado[valorAtributoCombo.Atributo].AddRange(valorAtributoCombo.Valores);
                    }
                    else
                    {
                        var listaComboMapa = new List<ValorPredefinido>();
                        listaComboMapa.AddRange(valorAtributoCombo.Valores);
                        resultado.Add(valorAtributoCombo.Atributo, listaComboMapa);
                    }
                }
            }
            return resultado;
        }

        private List<int> ObtenerIdsAtributoCombo(Producto producto)
        {
            var resultado = new List<int>();
            foreach (var valor in producto.ValoresSeleccionados.OfType<ValorAtributoCombo>())
            {
                resultado.Add(valor.ValorAtributoID);
            }
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
                    if (productoDb.Archivos.OfType<Foto>().Any())
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
                            productoDb.Archivos = new List<Archivo>();
                        }

                        if (productoDb.Archivos.Any() && filesToDelete != null && filesToDelete.Any())
                        {
                            productoDb.Archivos.RemoveAll(a => filesToDelete.Contains(a.ArchivoID));

                            var archivosABorrar = db.Archivos
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
                            throw new CustomException(message) { Key = key };
                        }
                    }
                }
            }
            catch (System.Data.Entity.Infrastructure.DbUpdateException e)
            {
                if (e.InnerException.InnerException.Message.Contains("UNIQUE KEY"))
                {
                    throw new ValorDuplicadoException("El codigo >" + productoUpdate.Codigo + "< ya existe", e);
                }

                throw;
            }
        }

        public List<ArchivoDTO> ListarImagenesProductoDTO(int idProducto)
        {
            var resultado = new List<ArchivoDTO>();
            var imagenes = GetFotosProducto(idProducto);
            if (imagenes != null && imagenes.Any())
            {
                foreach (var imagen in imagenes)
                {
                    resultado.Add(new ArchivoDTO()
                    {
                        ArchivoId = imagen.ArchivoID,
                        Nombre = imagen.Nombre,
                        Url = imagen.Url
                    });
                }
            }
            return resultado;
        }

        public List<ArchivoDTO> ListarVideosProductoDTO(int idProducto)
        {
            var resultado = new List<ArchivoDTO>();
            var videos = GetVideosProducto(idProducto);
            if (videos != null && videos.Any())
            {
                foreach (var video in videos)
                {
                    resultado.Add(new ArchivoDTO()
                    {
                        ArchivoId = video.ArchivoID,
                        Nombre = video.Nombre,
                        Url = video.Url
                    });
                }
            }
            return resultado;
        }

        /// <summary>
        /// maxResultados indica la cantidad de items que devuelve el metodo
        /// si maxResultados == 0 se devuelven todos
        /// </summary>
        /// <param name="maxResultados"></param>
        /// <returns></returns>
        public List<CantidadProductoPedido> ReporteProductos(int maxResultados)
        {
            var resultado = new List<CantidadProductoPedido>();
            using (var db = new Persistencia())
            {
                if (maxResultados <= 0)
                    maxResultados = db.CantidadProductosPedido.Count();

                resultado = db.CantidadProductosPedido
                    .Include("Producto")
                    .Where(x => x.Activo)
                    .OrderByDescending(y => y.Cantidad)
                    .Take(maxResultados)
                    .ToList();
            }
            return resultado;
        }

    }
}

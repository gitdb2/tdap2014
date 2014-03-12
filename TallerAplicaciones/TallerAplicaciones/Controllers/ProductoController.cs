using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallerAplicaciones.Filters;
using TallerAplicaciones.Models;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.dominio.Exceptions;
using uy.edu.ort.taller.aplicaciones.negocio;

namespace TallerAplicaciones.Controllers
{
    public class ProductoController : Controller
    {
        //
        // GET: /Producto/
        [CustomAuthorize(Roles = "EjecutivoDeCuenta")]
        public ActionResult Index()
        {
            return View();
        }

        //
        // GET: /Producto/Details/5

        //        public ActionResult Details(int id)
        //        {
        //            return View();
        //        }


        [AllowAnonymous]
        public ActionResult List()
        {
            var model = new ProductoListModel
            {
                Productos = ManejadorProducto.GetInstance().ListarProductos()
            };

            return View(model);
        }



        //
        // GET: /Producto/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        // POST: /Producto/Create
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Create(ProductoConArchivosSubmitModel model)
        {

            if (!ModelState.IsValid) return View(model);

        
            var videoList = new List<Archivo>();
            var fotoList = new List<Archivo>();
  

            try
            {
                var producto = new Producto()
                {
                    Codigo = model.Codigo,
                    Descripcion = model.Descripcion,
                    Nombre = model.Nombre,
                    Activo = true,
                    Archivos = new List<Archivo>()
                };

                fotoList = GetArchivosAndSaveFiles(model, producto, false);
                videoList = GetArchivosAndSaveFiles(model, producto, true);


                 ManejadorProducto.GetInstance().AltaProducto(producto);


                return RedirectToAction("List");
            }
            catch (ValorDuplicadoException ex)
            {
                ModelState.AddModelError("Codigo", ex.Message);

                foreach (var file in videoList)
                {
                    System.IO.File.Delete(file.PathFileSystem);
                }
                foreach (var file in fotoList)
                {
                    System.IO.File.Delete(file.PathFileSystem);
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        
        //
        // GET: /Producto/Edit/5

        public ActionResult Edit(int idProducto)
        {

            return View(GetProductoConArchivosSubmitModelFromDB(idProducto));
        }

        private ProductoConArchivosSubmitModel GetProductoConArchivosSubmitModelFromDB(int idProducto)
        {
            var producto = ManejadorProducto.GetInstance().GetProducto(idProducto);
            if (producto == null) throw new Exception("El producto id " + idProducto + " no existe");


            return new ProductoConArchivosSubmitModel
            {
                Activo = producto.Activo,
                Producto = producto,
                Descripcion = producto.Descripcion,
                Nombre = producto.Nombre,
                Codigo = producto.Codigo,
                ProductoID = producto.ProductoID
            }; 
        }


        //
        // POST: /Producto/Edit/5

        [HttpPost]
        public ActionResult Edit(ProductoConArchivosSubmitModel model)
        {

            if (!ModelState.IsValid) return View(model);

          

            var filesToDelete = (model.DeleteFiles != null && model.DeleteFiles.Any())
                ? model.DeleteFiles
                : new List<int>();



            var videoList = new List<Archivo>();
            var fotoList = new List<Archivo>();


            try
            {
                var producto = new Producto()
                {
                    Codigo = model.Codigo,
                    Descripcion = model.Descripcion,
                    Nombre = model.Nombre,
                    Activo = model.Activo,
                    ProductoID = model.ProductoID,
                    Archivos = new List<Archivo>()
                };

                fotoList = GetArchivosAndSaveFiles(model, producto, false);
                videoList = GetArchivosAndSaveFiles(model, producto, true);

                
                ManejadorProducto.GetInstance().Modificar(producto, filesToDelete);


                return RedirectToAction("List");
            }
            catch (ValorDuplicadoException ex)
            {

               

                ModelState.AddModelError("Codigo", ex.Message);

                foreach (var file in videoList)
                {
                    System.IO.File.Delete(file.PathFileSystem);
                }
                foreach (var file in fotoList)
                {
                    System.IO.File.Delete(file.PathFileSystem);
                }
            }
            catch (CustomException ex)
            {
                ModelState.AddModelError(ex.Key, ex.Message);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }

            // If we got this far, something failed, redisplay form
            ///Este es el modelo que se devuelve en caso que la operacion de modificacion de error
            var errorModel = GetProductoConArchivosSubmitModelFromDB(model.ProductoID);
            errorModel.Activo = model.Activo;
            errorModel.Descripcion = model.Descripcion;
            errorModel.Codigo = model.Codigo;
            errorModel.Nombre = model.Nombre;
            errorModel.ProductoID = model.ProductoID;
            return View(errorModel);


        }

        //
        // GET: /Producto/Delete/5

        public ActionResult Delete(int idProducto)
        {
            return View(new DeleteProductModel { IdProducto = idProducto });
        }

        //
        // POST: /Producto/Delete/5

        [HttpPost]
        public ActionResult Delete(int idProducto, DeleteProductModel model)
            //int id, FormCollection collection)
        {

            //var model = new DeleteProductModel {id = id};

            try
            {
                if (ManejadorProducto.GetInstance().BajaProducto(model.IdProducto))
                {
                    return RedirectToAction("List");
                }
                ModelState.AddModelError("idProducto", "El producto No fue  modificado");

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("idProducto", ex.Message);
            }

            return View(model);
        }


        /// <summary>
        /// Genera y salva los archivos de fotos y video y los pone en el producto
        /// </summary>
        /// <param name="model"></param>
        /// <param name="producto"></param>
        /// <param name="isVideo"></param>
        /// <returns></returns>
        protected List<Archivo> GetArchivosAndSaveFiles(ProductoConArchivosSubmitModel model, Producto producto, bool isVideo)
        {
            String defaultFolder = "Images";
            string tipo = isVideo ? "Videos" : "Fotos";
            var fileList = new List<Archivo>();
            string basePath = Server.MapPath("~/" + defaultFolder);
            basePath = Path.Combine(basePath, tipo);
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            var uploadeFiles = isVideo ? model.Videos : model.Fotos;

            if (uploadeFiles == null) return fileList;

            foreach (var file in uploadeFiles)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        var extension = Path.GetExtension(file.FileName);
                        var fsName = Guid.NewGuid().ToString() + extension;
                        var path = Path.Combine(basePath, fsName);


                        Archivo archivo = null;
                        if (isVideo)
                        {
                            archivo = new Video();
                        }
                        else
                        {
                          archivo = new Foto();  
                        }
                        
                        archivo.Url = "/" + defaultFolder+"/"+tipo+"/" + fsName;
                        archivo.PathFileSystem = path;
                        archivo.Nombre = fileName;
                        archivo.Activo = true;
                   
                        producto.Archivos.Add(archivo);
                        fileList.Add(archivo);
                        file.SaveAs(path);
                    }
                }
            }
            return fileList;
        }

    }


}

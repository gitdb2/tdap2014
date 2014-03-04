using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
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

        
            var videoList = new List<Video>();
            var fotoList = new List<Foto>();
  

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


              
                {//FOTOS
                    var basePath = Server.MapPath("~/Uploads");
                    basePath = Path.Combine(basePath, "Fotos");
                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }

                    foreach (var file in model.Fotos)
                    {
                        if (file != null && file.ContentLength > 0)
                        {
                            var fileName = Path.GetFileName(file.FileName);
                            if (fileName != null)
                            {
                                var extension = Path.GetExtension(file.FileName);
                                var fsName = Guid.NewGuid().ToString() + extension;
                                var path = Path.Combine(basePath, fsName);
                                var archivo = new Foto
                                {
                                    Url = "/Uploads/Fotos/" + fsName,
                                    PathFileSystem = path,
                                    Nombre = fileName,
                                    Activo = true
                                };
                                producto.Archivos.Add(archivo);
                                fotoList.Add(archivo);
                                file.SaveAs(path);
                            }

                        }
                    }
                }

                
                {//Videos
                   
                    var basePath = Server.MapPath("~/Uploads");
                    basePath = Path.Combine(basePath, "Videos");
                    if (!Directory.Exists(basePath))
                    {
                        Directory.CreateDirectory(basePath);
                    }
                    foreach (var file in model.Videos)
                    {
                        if (file!=null && file.ContentLength > 0)
                        {
                           
                            var fileName = Path.GetFileName(file.FileName);
                            if (fileName != null)
                            {
                                var extension = Path.GetExtension(file.FileName);
                                var fsName = Guid.NewGuid().ToString() + "." + extension;
                                var path = Path.Combine(basePath, fsName);

                                var archivo = new Video
                                {
                                    Url = "/Uploads/Videos/" + fsName,
                                    PathFileSystem = path,
                                    Nombre = fileName,
                                    Activo = true
                                };
                                producto.Archivos.Add(archivo);
                                videoList.Add(archivo);
                                file.SaveAs(path);
                            }
                            
                        }
                    }
                }

                

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

        public ActionResult Edit(int id)
        {
            var producto = ManejadorProducto.GetInstance().GetProducto(id);
            return View(new ProductoConArchivosSubmitModel
            {
                Activo = producto.Activo,
                Producto = producto,
                Descripcion = producto.Descripcion,
                Nombre = producto.Nombre,
                Codigo = producto.Codigo,
                ProductoID = producto.ProductoID
            });
        }

        //
        // POST: /Producto/Edit/5

        [HttpPost]
        public ActionResult Edit(ProductoConArchivosSubmitModel model)
        {

            if (!ModelState.IsValid) return View(model);

            try
            {
                

                return RedirectToAction("List");
            }
            catch (ValorDuplicadoException ex)
            {
                ModelState.AddModelError("Codigo", ex.Message);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", e.Message);
            }
            return View(model);
        }

        //
        // GET: /Producto/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Producto/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }
    }
}

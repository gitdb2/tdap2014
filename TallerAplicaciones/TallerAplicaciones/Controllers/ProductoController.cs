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

        //
        // POST: /Producto/Create
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Create(ProductoModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var producto = new Producto()
                {
                    Codigo = model.Codigo,
                    Descripcion = model.Descripcion,
                    Nombre = model.Nombre,
                    Activo = true
                };
                ManejadorProducto.GetInstance().AltaProducto(producto);

                return RedirectToAction("Create");
            }
            catch (ValorDuplicadoException ex)
            {
                ModelState.AddModelError("Codigo", ex.Message);
            }
            catch (Exception e)
            {

                ModelState.AddModelError("", e.Message);
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult CreateConArchivos()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult CreateConArchivos(ProductoConArchivosSubmitModel model)
        {

            if (!ModelState.IsValid) return View(model);

            try
            {
                var producto = new Producto()
                {
                    Codigo = model.Codigo,
                    Descripcion = model.Descripcion,
                    Nombre = model.Nombre,
                    
                    Activo = true
                };
              

               
                var fotoDirs = new List<string>(); 
                {///Fotos
                   
                    
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
                            var path = Path.Combine(basePath, fileName);
                            file.SaveAs(path);
                            fotoDirs.Add(path);
                        }
                    }
                    //if (fotoDirs.Any())
                    //{
                      
                    //    ManejadorProducto.GetInstance().AsignarFotos(producto.ProductoID, fotoDirs);
                    //}
                }

                var videoDirs = new List<string>();    
                {///Videos
                   
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
                            var path = Path.Combine(basePath, fileName);
                            file.SaveAs(path);
                            videoDirs.Add(path);
                        }
                    }
                    //if (videoDirs.Any())
                    //{
                    //    ManejadorProducto.GetInstance().AsignarVideos(producto.ProductoID, videoDirs);
                    //}
                }

                ManejadorProducto.GetInstance().AltaProducto(producto, fotoDirs, videoDirs);


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

            // If we got this far, something failed, redisplay form
            return View();
        }

























        //
        // GET: /Producto/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Producto/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
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

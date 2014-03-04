using System;
using System.Collections.Generic;
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

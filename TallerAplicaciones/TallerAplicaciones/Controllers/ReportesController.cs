using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallerAplicaciones.Models;
using uy.edu.ort.taller.aplicaciones.negocio;

namespace TallerAplicaciones.Controllers
{
    public class ReportesController : Controller
    {

        [AllowAnonymous]
        public ActionResult Productos()
        {
            var model = new ReporteProductosModel
            {
                Productos = ManejadorProducto.GetInstance().ReporteProductos(0)
            };
            return View(model);
        }

        [AllowAnonymous]
        public ActionResult TopProductos()
        {
            var model = new ReporteTopProductosModel
            {
                TopProductos = ManejadorProducto.GetInstance().ReporteProductos(5)
            };
            return View(model);
        }

    }
}

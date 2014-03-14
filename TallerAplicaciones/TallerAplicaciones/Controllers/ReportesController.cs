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
        //
        // GET: /Reportes/

        [AllowAnonymous]
        public ActionResult Productos()
        {
            var model = new ReporteProductosModel
            {
                Productos = ManejadorProducto.GetInstance().ReporteProductos()
            };
            return View(model);
        }

    }
}

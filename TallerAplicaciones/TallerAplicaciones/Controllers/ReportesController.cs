using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallerAplicaciones.Models;
using uy.edu.ort.taller.aplicaciones.dominio;
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


        [AllowAnonymous]
        public ActionResult Logs()
        {
            var now = DateTime.Now;
            var from = now.AddMonths(-1) + new TimeSpan(0, 0, 0, 0);
            var to = now + new TimeSpan(12, 59, 59);

            var model = new ReporteLogsModel
            {
                FechaDesde = from,
                FechaHasta = to,
                Logs = ManejadorReporte.GetInstance().GetLogs(from, to)
            };
            return View(model);
        }



        [AllowAnonymous]
        [HttpPost]
        public ActionResult Logs(ReporteLogsModel model)
        {

            if (ModelState.IsValid)
            {
                if (model.FechaDesde > model.FechaHasta)
                {
                    var tmp = model.FechaDesde;
                    model.FechaDesde = model.FechaHasta;
                    model.FechaHasta = tmp;
                }


                model.FechaDesde = model.FechaDesde + new TimeSpan(0, 0, 0, 0);
                model.FechaHasta = model.FechaHasta + new TimeSpan(12, 59, 59);


                model.Logs = ManejadorReporte.GetInstance().GetLogs(model.FechaDesde, model.FechaHasta);
            }
            else
            {

                model.Logs = new List<LogInfo>();

            }


            return View(model);
        }



    }
}

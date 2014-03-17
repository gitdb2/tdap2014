using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallerAplicaciones.Filters;
using TallerAplicaciones.Models;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.dominio.Constants;
using uy.edu.ort.taller.aplicaciones.negocio;

namespace TallerAplicaciones.Controllers
{
    [CustomAuthorize]
    public class ReportesController : Controller
    {

        [CustomAuthorize(Roles = "EjecutivoDeCuenta")]
        public ActionResult Productos()
        {
            var model = new ReporteProductosModel
            {
                Productos = ManejadorProducto.GetInstance().ReporteProductos(0)
            };
            return View(model);
        }

        [CustomAuthorize(Roles = "EjecutivoDeCuenta")]
        public ActionResult TopProductos()
        {
            var model = new ReporteTopProductosModel
            {
                TopProductos = ManejadorProducto.GetInstance().ReporteProductos(5)
            };
            return View(model);
        }


        [CustomAuthorize(Roles = "Administrador")]
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



        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
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

                var now = DateTime.Now;
                var from = now.AddMonths(-1) + new TimeSpan(0, 0, 0, 0);
                var to = now + new TimeSpan(12, 59, 59);

                model.FechaDesde = from;
                model.FechaHasta = to;

                model.Logs = new List<LogInfo>();

            }


            return View(model);
        }


        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Pedidos()
        {

            var now = DateTime.Now;
            var fromDate = now.AddMonths(-1) + new TimeSpan(0, 0, 0, 0);
            var toDate = now + new TimeSpan(12, 59, 59);

            var model = new ReportePedidoModel
            {
                FechaDesde = fromDate,
                FechaHasta = toDate,
                DistribuidorId = -1,
                EjecutivoId = -1,
                Pedidos = new List<Pedido>()
            };



            var distribuidores = ManejadorPerfilUsuario.GetInstance().GetDistribuidores();
            var listaDist = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Selecctione un Distribuidor",
                    Value = "-1",
                    Selected = true
                }
            };
            if (distribuidores != null && distribuidores.Any())
            {
                foreach (var dist in distribuidores)
                {

                    var item = new SelectListItem()
                    {
                        Value = dist.PerfilUsuarioID.ToString(),
                        Text =
                            dist.Usuario.Login.ToString() + "(" + dist.Nombre + " " + dist.Apellido + " - " +
                            dist.Empresa.Nombre + ")"

                    };
                    listaDist.Add(item);
                }
            }
            ViewBag.Distribuidores = listaDist;

            var ejecutivos = ManejadorPerfilUsuario.GetInstance().GetEjecutivos();
            var listaEjec = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Selecctione un Ejecutivo",
                    Value = "-1",
                    Selected = true
                }
            };
            if (ejecutivos != null && ejecutivos.Any())
            {
                foreach (var ejec in ejecutivos)
                {
                    listaEjec.Add(new SelectListItem
                    {
                        Value = ejec.PerfilUsuarioID.ToString(),
                        Text =
                            ejec.Usuario.Login.ToString() + "(" + ejec.Nombre + " " + ejec.Apellido + ")"

                    });
                }
            }
            ViewBag.Ejecutivos = listaEjec;
            return View(model);
        }


        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Pedidos(ReportePedidoModel model)
        {
            //si los ids de distribuidor o ejecutivo son -1 entonces se asume que son filtros
            if (ModelState.IsValid)
            {
                if (model.FechaDesde > model.FechaHasta)
                {
                    var tmp = model.FechaDesde;
                    model.FechaDesde = model.FechaHasta;
                    model.FechaHasta = tmp;
                }

                model.Pedidos = ManejadorReporte.GetInstance()
                    .GetPedidos(model.FechaDesde, model.FechaHasta, model.DistribuidorId, model.EjecutivoId);
            }
            else
            {
                var now = DateTime.Now;
                var from = now.AddMonths(-1) + new TimeSpan(0, 0, 0, 0);
                var to = now + new TimeSpan(12, 59, 59);

                model.FechaDesde = from;
                model.FechaHasta = to;
                model.Pedidos = new List<Pedido>();
            }

            var distribuidores = ManejadorPerfilUsuario.GetInstance().GetDistribuidores();
            var listaDist = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Selecctione un Distribuidor",
                    Value = "-1",
                    Selected = model.DistribuidorId == -1
                }
            };
            if (distribuidores != null && distribuidores.Any())
            {
                foreach (var dist in distribuidores)
                {

                    var item = new SelectListItem()
                    {
                        Value = dist.PerfilUsuarioID.ToString(),
                        Text =
                            dist.Usuario.Login.ToString() + "(" + dist.Nombre + " " + dist.Apellido + " - " +
                            dist.Empresa.Nombre + ")",
                        Selected = model.DistribuidorId == dist.PerfilUsuarioID

                    };
                    listaDist.Add(item);
                }
            }
            ViewBag.Distribuidores = listaDist;

            var ejecutivos = ManejadorPerfilUsuario.GetInstance().GetEjecutivos();
            var listaEjec = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Selecctione un Ejecutivo",
                    Value = "-1",
                    Selected = model.EjecutivoId == -1
                }
            };
            if (ejecutivos != null && ejecutivos.Any())
            {
                foreach (var ejec in ejecutivos)
                {
                    listaEjec.Add(new SelectListItem
                    {
                        Value = ejec.PerfilUsuarioID.ToString(),
                        Text =
                            ejec.Usuario.Login.ToString() + "(" + ejec.Nombre + " " + ejec.Apellido + ")",
                        Selected = model.EjecutivoId == ejec.PerfilUsuarioID

                    });
                }
            }
            ViewBag.Ejecutivos = listaEjec;


            return View(model);
        }

        //-------------------------------------------------------------------
        [CustomAuthorize(Roles = "EjecutivoDeCuenta")]
        public ActionResult PedidosEjecutivo()
        {

            var now = DateTime.Now;
            var fromDate = now.AddMonths(-1) + new TimeSpan(0, 0, 0, 0);
            var toDate = now + new TimeSpan(12, 59, 59);

            var ejecutivo = (PerfilUsuario)Session[Constants.SESSION_PERFIL];

            var orderBy = ManejadorReporte.Orderby.Fecha;
            var ordenDir = ManejadorReporte.OrdenDir.Desc;

            var model = new ReporteEjecutivoPedidoModel()
            {
                FechaDesde = fromDate,
                FechaHasta = toDate,
                DistribuidorId = -1,
                EjecutivoId = ejecutivo.PerfilUsuarioID,
                OrdenBy = (int)orderBy,
                OrdenDir = (int)ordenDir,

                Pedidos = new List<Pedido>()
            };
            model.Pedidos = ManejadorReporte.GetInstance()
                 .GetPedidos(model.FechaDesde, model.FechaHasta,
                              model.DistribuidorId, model.EjecutivoId,
                              orderBy, ordenDir);


            var distribuidores = ManejadorPerfilUsuario.GetInstance().GetDistribuidoresConEmpresasDeEjecutivo(ejecutivo.PerfilUsuarioID);
            var listaDist = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Todos los Distribuidores",
                    Value = "-1",
                    Selected = true
                }
            };
            if (distribuidores != null && distribuidores.Any())
            {
                foreach (var dist in distribuidores)
                {

                    var item = new SelectListItem()
                    {
                        Value = dist.PerfilUsuarioID.ToString(),
                        Text =
                            dist.Usuario.Login.ToString() + "(" + dist.Nombre + " " + dist.Apellido + " - " +
                            dist.Empresa.Nombre + ")"

                    };
                    listaDist.Add(item);
                }
            }
            ViewBag.Distribuidores = listaDist;

            return View(model);
        }

        [CustomAuthorize(Roles = "EjecutivoDeCuenta")]
        [HttpPost]
        public ActionResult PedidosEjecutivo(ReporteEjecutivoPedidoModel model)
        {

            var ejecutivo = (PerfilUsuario)Session[Constants.SESSION_PERFIL];

            model.EjecutivoId = ejecutivo.PerfilUsuarioID;
            //si los ids de distribuidor o ejecutivo son -1 entonces se asume que son filtros
            if (ModelState.IsValid)
            {
                if (model.FechaDesde > model.FechaHasta)
                {
                    var tmp = model.FechaDesde;
                    model.FechaDesde = model.FechaHasta;
                    model.FechaHasta = tmp;
                }

                var orderBy = (ManejadorReporte.Orderby)model.OrdenBy;
                var orderDir = (ManejadorReporte.OrdenDir)model.OrdenDir;

                model.Pedidos = ManejadorReporte.GetInstance()
                    .GetPedidos(model.FechaDesde, model.FechaHasta,
                                 model.DistribuidorId, model.EjecutivoId,
                                 orderBy, orderDir);
            }
            else
            {
                var now = DateTime.Now;
                var from = now.AddMonths(-1) + new TimeSpan(0, 0, 0, 0);
                var to = now + new TimeSpan(12, 59, 59);

                model.FechaDesde = from;
                model.FechaHasta = to;
                model.Pedidos = new List<Pedido>();
            }

            var distribuidores = ManejadorPerfilUsuario.GetInstance().GetDistribuidoresConEmpresasDeEjecutivo(ejecutivo.PerfilUsuarioID);

            var listaDist = new List<SelectListItem>
            {
                new SelectListItem
                {
                    Text = "Todos los Distribuidores",
                    Value = "-1",
                    Selected = model.DistribuidorId == -1
                }
            };
            if (distribuidores != null && distribuidores.Any())
            {
                foreach (var dist in distribuidores)
                {

                    var item = new SelectListItem()
                    {
                        Value = dist.PerfilUsuarioID.ToString(),
                        Text =
                            dist.Usuario.Login.ToString() + "(" + dist.Nombre + " " + dist.Apellido + " - " +
                            dist.Empresa.Nombre + ")",
                        Selected = model.DistribuidorId == dist.PerfilUsuarioID

                    };
                    listaDist.Add(item);
                }
            }
            ViewBag.Distribuidores = listaDist;


            return View(model);
        }
    }
}

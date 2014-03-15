using System;
using System.Collections.Generic;
using System.Data;
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
    [CustomAuthorize]
    public class PedidoController : Controller
    {

        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //
        // GET: /Pedido/
        public ActionResult Index()
        {
            return View();
        }

        [AllowAnonymous]
        public ActionResult List()
        {
            var model = new PedidoListModel
            {
                Pedidos = ManejadorPedido.GetInstance().ListarPedidos()
            };

            return View(model);
        }

        //
        // GET: /Pedido/Create
        [AllowAnonymous]
        public ActionResult Create()
        {
            var eject = (EjecutivoDeCuenta)Session["Perfil"];

            var distribuidores = ManejadorPerfilUsuario.GetInstance().GetDistribuidoresConEmpresasDeEjecutivo(eject.PerfilUsuarioID);
            var productosDisponibles = ManejadorProducto.GetInstance().ListarProductos();
            var model = new PedidoCreateModel()
            {
                EjecutivoDeCuenta = eject,
                EjecutivoId = eject.PerfilUsuarioID,
                DistribuidoresDisponibles = distribuidores,
                ProductosDisponibles = productosDisponibles,
                Fecha = DateTime.Now,
                Aprobado = false,
                Activo = true
            };

            return View(model);
        }

        // POST: /Pedido/Create
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Create(PedidoCreatePOSTModel model)
        {

            if (!ModelState.IsValid)
            {
                return View(TransformarAErrorModelo(model));
            }

            try
            {
                var pedido = new Pedido()
                {
                    Activo = true,
                    Aprobado = model.Aprobado,
                    Descripcion = model.Descripcion,
                    Fecha = model.Fecha
                };

                try
                {
                    ManejadorPedido.GetInstance()
                        .Alta(pedido, model.DistribuidorID, model.EjecutivoId, model.Productos, model.Cantidades);
                }
                catch (EnvioMailException envioMailException)
                {
                    log.Error(envioMailException);
                }

                return RedirectToAction("List");
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
            return View(TransformarAErrorModelo(model));
        }

        private PedidoCreateModel TransformarAErrorModelo(PedidoCreatePOSTModel modelIn)
        {
            var model = new PedidoCreateModel()
            {
                PedidoID = modelIn.PedidoID,
                Aprobado = modelIn.Aprobado,
                Descripcion = modelIn.Descripcion,
                Fecha = modelIn.Fecha,
                Activo = modelIn.Activo,
                EjecutivoId = modelIn.EjecutivoId,
                DistribuidorID = modelIn.DistribuidorID,
                Productos = modelIn.Productos,
                Cantidades = modelIn.Cantidades
            };

            model.ProductosDisponibles = new List<Producto>();
            model.EjecutivoDeCuenta = (EjecutivoDeCuenta)Session["Perfil"];

            model.DistribuidoresDisponibles = ManejadorPerfilUsuario.GetInstance()
                .GetDistribuidoresConEmpresasDeEjecutivo(model.EjecutivoDeCuenta.PerfilUsuarioID);
            model.ProductosDisponibles = ManejadorProducto.GetInstance().ListarProductos();
            return model;
        }

        private PedidoEditModel GetPedidoModelFromDB(int idPedido)
        {

            var pedido = ManejadorPedido.GetInstance().GetPedido(idPedido);
            if (pedido == null) throw new CustomException("El pedido id " + idPedido + " no existe")
            {
                Key = "PedidoID"
            };
            var ret = new PedidoEditModel()
            {
                Activo = pedido.Activo,
                Aprobado = pedido.Aprobado,
                Descripcion = pedido.Descripcion,
                Fecha = pedido.Fecha,
                PedidoID = pedido.PedidoID,
                DistribuidorID = pedido.Distribuidor.PerfilUsuarioID,
                EjecutivoId = pedido.Ejecutivo.PerfilUsuarioID,
                EjecutivoDeCuenta = pedido.Ejecutivo,
                Pedido = pedido,
            };

            LoadDistribuidoresYProductos(ret);
            return ret;
        }

        protected void LoadDistribuidoresYProductos(PedidoEditModel model)
        {
            model.DistribuidoresDisponibles = ManejadorPerfilUsuario.GetInstance()
              .GetDistribuidoresConEmpresasDeEjecutivo(model.EjecutivoId);
            model.ProductosDisponibles = ManejadorProducto.GetInstance().ListarProductos();
        }

        public class BaseJson
        {
            public bool Ok { get; set; }
            public string Message { get; set; }
        }

        public class ModificarCantidadPedidoJson:BaseJson
        {
            public int IdPedido { get; set; }
            public int IdCantidadProductoPedido { get; set; }
            public int Cantidad { get; set; }
            public bool Borrado { get; set; }
        }

        public class AddCantidadPedidoJson : ModificarCantidadPedidoJson
        {
            public int IdProducto { get; set; }
            public string NombreProducto { get; set; }
            public string CodigoProducto { get; set; }
        }

        [HttpPost]
        public JsonResult AgregarItemPedidoCantidadProducto(int idPedido, int idProducto, int cantidad)
        {
            var ret = new AddCantidadPedidoJson()
            {
                IdPedido = idPedido,
                IdProducto = idProducto,
                Ok = false,
                Message = "Inicializacion"
            };
            try
            {
                //retorna el id del pedido resultado de la insersion en la base de datos
                int idCantidadProductoPedido = ManejadorPedido.GetInstance().AgregarCantidadPedido(idPedido, idProducto, cantidad);

                var prod = ManejadorProducto.GetInstance().GetProducto(idProducto);

                ret.CodigoProducto = prod.Codigo;
                ret.NombreProducto = prod.Nombre;
                ret.IdCantidadProductoPedido = idCantidadProductoPedido;
                ret.Cantidad = cantidad;
                ret.Ok = true;
            }
            catch (Exception e)
            {
                ret.Message = e.Message;
                ret.Ok = false;
            }
            return Json(ret, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ModificarPedidoCantidadProducto(int idPedido, int idCantidadProductoPedido,
            bool borrar, int cantidad)
        {

            var ret = new ModificarCantidadPedidoJson()
            {
                IdCantidadProductoPedido = idCantidadProductoPedido,
                IdPedido = idPedido,
                Message = ""
            };

            if (borrar)
            {
                try
                {
                    //retorna true o false si pudo borrar y tira excepcion si se produjo algun problema
                    ret.Borrado = ManejadorPedido.GetInstance().BajaCantidadPedido(idPedido, idCantidadProductoPedido);
                    ret.Ok = true;
                }
                catch (Exception e)
                {
                    ret.Message = e.Message;
                    ret.Ok = false;
                }
            }
            else
            {
                if (cantidad <= 0)
                {
                    ret.Ok = false;
                    ret.Message = "cantidad no puede ser negantivo ni cero";
                }
                else
                {
                    try
                    {
                        ret.Ok = ManejadorPedido.GetInstance().UpdateCantidadProductoPedido(idPedido, idCantidadProductoPedido, cantidad);
                        ret.Cantidad = cantidad;
                    } 
                    catch (Exception e)
                    {
                        ret.Message = e.Message;
                        ret.Ok = false;
                    }
                }
            }
            return Json(ret,  JsonRequestBehavior.AllowGet);
        }

        //
        // GET: /Pedido/Edit/5
        public ActionResult Edit(int idPedido)
        {
            return View(GetPedidoModelFromDB(idPedido));
        }

        //
        // POST: /Pedido/Edit
        [HttpPost]
        public ActionResult Edit(PedidoEditModel model)
        {

            if (!ModelState.IsValid)
            {
                LoadDistribuidoresYProductos(model);
                return View(model);
            }

            try
            {

                var pedido = new Pedido()
                {
                    Aprobado = model.Aprobado,
                    Descripcion = model.Descripcion,
                    Fecha = model.Fecha,
                    Activo = model.Activo,
                    PedidoID = model.PedidoID,
                };
               
                ManejadorPedido.GetInstance().Modificar(pedido);

                return RedirectToAction("List");
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
            var errorModel = GetPedidoModelFromDB(model.PedidoID);
            //errorModel.Activo = model.Activo;
            //errorModel.Descripcion = model.Descripcion;
            //errorModel.Codigo = model.Codigo;
            //errorModel.Nombre = model.Nombre;
            //errorModel.ProductoID = model.ProductoID;
            return View(errorModel);
        }

        //
        // GET: /Pedido/Delete/5

        public ActionResult Delete(int idPedido)
        {
            return View(new DeletePedidoModel { PedidoID = idPedido });
        }

        //
        // POST: /Pedido/Delete/5

        [HttpPost]
        public ActionResult Delete(int idPedido, DeletePedidoModel model)
        //int id, FormCollection collection)
        {
            if (!ModelState.IsValid) return View(model);
            try
            {
                ManejadorPedido.GetInstance().Baja(model.PedidoID);
                return RedirectToAction("List");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("PedidoID", ex.Message);
            }
            return View(model);
        }

        public ActionResult Detalle(int idPedido)
        {
            return View(ManejadorPedido.GetInstance().GetPedido(idPedido));
        }

    }

}

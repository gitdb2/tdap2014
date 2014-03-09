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
    public class PedidoController : Controller
    {
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

            if (!ModelState.IsValid )
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

             
                ManejadorPedido.GetInstance().Alta(pedido, model.DistribuidorID, model.EjecutivoId, model.Productos, model.Cantidades);


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


        //
        // GET: /Pedido/Edit/5

        public ActionResult Edit(int idPedido)
        {

            return View(GetPedidoModelFromDB(idPedido));
        }

        private PedidoEditModel GetPedidoModelFromDB(int idPedido)
        {
            //var producto = ManejadorProducto.GetInstance().GetProducto(idProducto);
            //if (producto == null) throw new Exception("El producto id " + idProducto + " no existe");


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

            ret.DistribuidoresDisponibles = ManejadorPerfilUsuario.GetInstance()
                .GetDistribuidoresConEmpresasDeEjecutivo(ret.EjecutivoId);
            ret.ProductosDisponibles = ManejadorProducto.GetInstance().ListarProductos();

            return ret;
        }


        //
        // POST: /Pedido/Edit

        [HttpPost]
        public ActionResult Edit(PedidoCreateModel model)
        {

            if (!ModelState.IsValid) return View(model);



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

                int idEj = model.EjecutivoId;
                int idDistrib = model.DistribuidorID;
                List<int> idprods = model.Productos;
                List<int> cantidades = model.Cantidades;


                throw new NotImplementedException();
                //ManejadorPedido.GetInstance().Modificar(pedido, Otros Params);


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

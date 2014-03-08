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
            var eject = (EjecutivoDeCuenta) Session["Perfil"];

            var distribuidores = ManejadorPerfilUsuario.GetInstance().GetDistribuidoresConEmpresasDeEjecutivo(eject.PerfilUsuarioID);
            var productosDisponibles = ManejadorProducto.GetInstance().ListarProductos();
            var model =  new PedidoCreateModel()
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
        public ActionResult Create(PedidoCreateModel model)
        {
            model.ProductosDisponibles= new List<Producto>();
            var valido = ModelState.IsValid;



            if (!ModelState.IsValid) return View(model);//RedirectToAction("Create");

        



            try
            {
                //var producto = new Producto()
                //{
                //    Codigo = model.Codigo,
                //    Descripcion = model.Descripcion,
                //    Nombre = model.Nombre,
                //    Activo = true,
                //    Archivos = new List<Archivo>()
                //};

                //fotoList = GetArchivosAndSaveFiles(model, producto, false);
                //videoList = GetArchivosAndSaveFiles(model, producto, true);


                // ManejadorProducto.GetInstance().AltaProducto(producto);


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
            return View(model);
        }

        
        //
        // GET: /Pedido/Edit/5

        public ActionResult Edit(int idPedido)
        {

            return View(GetPedidoModelFromDB(idPedido));
        }

        private PedidoCreateModel GetPedidoModelFromDB(int idPedido)
        {
            //var producto = ManejadorProducto.GetInstance().GetProducto(idProducto);
            //if (producto == null) throw new Exception("El producto id " + idProducto + " no existe");


            //return new ProductoConArchivosSubmitModel
            //{
            //    Activo = producto.Activo,
            //    Producto = producto,
            //    Descripcion = producto.Descripcion,
            //    Nombre = producto.Nombre,
            //    Codigo = producto.Codigo,
            //    ProductoID = producto.ProductoID
            //}; 
            var ret = new PedidoCreateModel();
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

                int idEj= model.EjecutivoId;
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

            //var model = new DeleteProductModel {id = id};

            try
            {
                throw new NotImplementedException();
                //if (ManejadorPedido.GetInstance().Baja(model.IdPedido))
                //{
                //    return RedirectToAction("List");
                //}
                //ModelState.AddModelError("idPedido", "El Pedido No fue  modificado");

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("idPedido", ex.Message);
            }

            return View(model);
        }


     

    }


}

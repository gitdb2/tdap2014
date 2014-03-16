using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using TallerAplicaciones.Filters;
using TallerAplicaciones.Models;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.dominio.Exceptions;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.negocio;

namespace TallerAplicaciones.Controllers
{
    [CustomAuthorize]
    public class ProductoController : Controller
    {
        //
        // GET: /Producto/
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            return View();
        }

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
            ProductoConArchivosSubmitModel model = null;
            try
            {
                IAtributo iAtributo = ManejadorAtributo.GetInstance();
                List<Atributo> listaAtributos = iAtributo.GetAtributosActivos();

                model = new ProductoConArchivosSubmitModel() { ListaDeAtributos = listaAtributos };
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "ERROR");
            }
            return View(model);

        }

        // POST: /Producto/Create
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Create(ProductoConArchivosSubmitModel model)
        {

            if (!ModelState.IsValid) return View(model);


            var videoList = new List<Archivo>();
            var fotoList = new List<Archivo>();

            //check de extensiones de archivos
            if (!CheckFileExtension(model.Fotos, new List<String> { "png", "jpg" }))
            {
                ModelState.AddModelError("Fotos", "Solo se permiten Fotos .jpg o .png");
                return View(model);
            }
            if (!CheckFileExtension(model.Videos, new List<String> { "wmv" }))
            {
                ModelState.AddModelError("Videos", "Solo se permiten Videos .wmv");
                return View(model);
            }

            try
            {
                var producto = new Producto()
                {
                    Codigo = model.Codigo,
                    Descripcion = model.Descripcion,
                    Nombre = model.Nombre,
                    Activo = true,
                    Archivos = new List<Archivo>()//archivos a cargar en GetArchivosAndSaveFiles
                };

                fotoList = GetArchivosAndSaveFiles(model, producto, false);
                videoList = GetArchivosAndSaveFiles(model, producto, true);


                ManejadorProducto.GetInstance().AltaProducto(producto,
                    model.IdAtributoSimple, model.ValorAtributoSimple,
                    model.ValorAtributoCombo, model.ValorAtributoMulti);


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
            model.ListaDeAtributos = ManejadorAtributo.GetInstance().GetAtributosActivos();
            return View(model);
        }


        private bool CheckFileExtension(IEnumerable<HttpPostedFileBase> archivos, List<string> extensiones)
        {
            var ret = true;
            foreach (var file in archivos)
            {
                if (file != null)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        var extension = Path.GetExtension(file.FileName);
                        if (extension != null && extension.Length > 2)
                        {
                            extension = extension.Replace(".", "");

                            ret = extensiones.Contains(extension) && ret;
                            if (ret == false)
                            {
                                return false;
                            }
                        }
                        else
                        {
                            return false;
                        }
                    }
                    else
                    {
                        return false;
                    }
                }

            }

            return ret;
        }


        //
        // GET: /Producto/Edit/5

        public ActionResult Edit(int idProducto)
        {
            var model = GetProductoConArchivosSubmitModelFromDB(idProducto);

            return View(model);
        }


        //
        // POST: /Producto/Edit
        [HttpPost]
        public ActionResult Edit(ProductoConArchivosSubmitModel model)
        {

            if (!ModelState.IsValid)
            {

                return View(model);
            }

            //chequeo de archivos
            if (!CheckFileExtension(model.Fotos, new List<String> { "png", "jpg" }))
            {
                ModelState.AddModelError("Fotos", "Solo se permiten Fotos .jpg o .png");
                return View(model);
            }
            if (!CheckFileExtension(model.Videos, new List<String> { "wmv" }))
            {
                ModelState.AddModelError("Videos", "Solo se permiten Videos .wmv");
                return View(model);
            }


            var filesToDelete = (model.DeleteFiles != null && model.DeleteFiles.Any())
                ? model.DeleteFiles
                : new List<int>();

            var videoList = new List<Archivo>();
            var fotoList = new List<Archivo>();


            try
            {
                var producto = new Producto()
                {
                    Codigo = model.Codigo,
                    Descripcion = model.Descripcion,
                    Nombre = model.Nombre,
                    Activo = model.Activo,
                    ProductoID = model.ProductoID,
                    Archivos = new List<Archivo>()//archivos  a cargar en GetArchivosAndSaveFiles
                };

                fotoList = GetArchivosAndSaveFiles(model, producto, false);
                videoList = GetArchivosAndSaveFiles(model, producto, true);


                var idAtributoSimpleToAdd = new List<int>();
                var valorAtributoSimpleToAdd = new List<string>();
                var valorAtributoComboToAdd = new List<string>();
                var valorAtributoMultiToAdd = new List<string>();

                ManejadorProducto.GetInstance().Modificar(producto, filesToDelete,
                    idAtributoSimpleToAdd,
                    valorAtributoSimpleToAdd,
                    valorAtributoComboToAdd,
                    valorAtributoMultiToAdd);


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
            var errorModel = GetProductoConArchivosSubmitModelFromDB(model.ProductoID);
            errorModel.Activo = model.Activo;
            errorModel.Descripcion = model.Descripcion;
            errorModel.Codigo = model.Codigo;
            errorModel.Nombre = model.Nombre;
            errorModel.ProductoID = model.ProductoID;

            return View(errorModel);


        }


        private ProductoConArchivosSubmitModel GetProductoConArchivosSubmitModelFromDB(int idProducto)
        {
            var producto = ManejadorProducto.GetInstance().GetProducto(idProducto);

            if (producto == null)
            {
                throw new Exception("El producto id " + idProducto + " no existe");
            }


            var simples = ManejadorProducto.GetInstance().ObtenerValoresSimples(producto);
            var combos = ManejadorProducto.GetInstance().ObtenerValoresCombo(producto);
            var atributosDisponibles = ManejadorAtributo.GetInstance().GetAtributos();

            var model = new ProductoConArchivosSubmitModel
            {
                Activo = producto.Activo,
                Producto = producto,
                Descripcion = producto.Descripcion,
                Nombre = producto.Nombre,
                Codigo = producto.Codigo,
                ProductoID = producto.ProductoID,
                ListaValorAtributosCombo = combos,
                ListaValorAtributosSimple = simples,
                ListaDeAtributos = atributosDisponibles
            };
            return model;
        }

        //
        // GET: /Producto/Delete/5

        public ActionResult Delete(int idProducto)
        {
            return View(new DeleteProductModel { IdProducto = idProducto });
        }

        //
        // POST: /Producto/Delete/5

        [HttpPost]
        public ActionResult Delete(int idProducto, DeleteProductModel model)
        {

            //var model = new DeleteProductModel {id = id};

            try
            {
                if (ManejadorProducto.GetInstance().BajaProducto(model.IdProducto))
                {
                    return RedirectToAction("List");
                }
                ModelState.AddModelError("idProducto", "El producto No fue  modificado");

            }

            catch (Exception ex)
            {
                ModelState.AddModelError("idProducto", ex.Message);
            }

            return View(model);
        }


        /// <summary>
        /// Genera y salva los archivos de fotos y video y los pone en el producto
        /// </summary>
        /// <param name="model"></param>
        /// <param name="producto"></param>
        /// <param name="isVideo"></param>
        /// <returns></returns>
        protected List<Archivo> GetArchivosAndSaveFiles(ProductoConArchivosSubmitModel model, Producto producto, bool isVideo)
        {
            String defaultFolder = "Images";
            string tipo = isVideo ? "Videos" : "Fotos";
            var fileList = new List<Archivo>();
            string basePath = Server.MapPath("~/" + defaultFolder);
            basePath = Path.Combine(basePath, tipo);
            if (!Directory.Exists(basePath))
            {
                Directory.CreateDirectory(basePath);
            }
            var uploadeFiles = isVideo ? model.Videos : model.Fotos;

            if (uploadeFiles == null) return fileList;

            foreach (var file in uploadeFiles)
            {
                if (file != null && file.ContentLength > 0)
                {
                    var fileName = Path.GetFileName(file.FileName);
                    if (fileName != null)
                    {
                        var extension = Path.GetExtension(file.FileName);
                        var fsName = Guid.NewGuid().ToString() + extension;
                        var path = Path.Combine(basePath, fsName);


                        Archivo archivo = null;
                        if (isVideo)
                        {
                            archivo = new Video();
                        }
                        else
                        {
                            archivo = new Foto();
                        }

                        archivo.Url = "/" + defaultFolder + "/" + tipo + "/" + fsName;
                        archivo.PathFileSystem = path;
                        archivo.Nombre = fileName;
                        archivo.Activo = true;

                        producto.Archivos.Add(archivo);
                        fileList.Add(archivo);
                        file.SaveAs(path);
                    }
                }
            }
            return fileList;
        }

        /// <summary>
        /// aplica para valor simple, combo y combomulti
        /// </summary>
        /// <param name="idProducto"></param>
        /// <param name="idValorAtributo"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult RemoverValorAtributo(int idProducto, int idValorAtributo)
        {
            var errorString = "Ocurrio un error al borrar el valor";
            var resJson = new RemoverValorAtributoJson()
            {
                Message = "",
                Ok = false,
                ProductoId = idProducto,
                ValorAtributoId = idValorAtributo
            };
            try
            {
                var resultadoOK = ManejadorProducto.GetInstance().RemoverValorAtributo(idProducto, idValorAtributo);
                resJson.Ok = resultadoOK;
                resJson.Message = resultadoOK ? "Se elimino el atributo" : errorString;
            }
            catch (Exception ex)
            {
                resJson.Ok = false;
                resJson.Message = ex.Message;
            }
            return Json(resJson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ModificarValorAtributoSimple(int idValorAtributoSimple, string nuevoValor)
        {
            var errorString = "Ocurrio un error al editar el valor";
            var resJson = new EditarValorAtributoSimpleJson()
            {
                Message = "",
                Ok = false,
                ValorAtributoId = idValorAtributoSimple,
                NuevoValor = null
            };
            try
            {
                var resultadoOK = ManejadorProducto.GetInstance().ModificarValorAtributoSimple(idValorAtributoSimple, nuevoValor);
                resJson.Ok = resultadoOK;
                resJson.Message = resultadoOK ? "Se modifico el atributo" : errorString;
                resJson.NuevoValor = nuevoValor;
            }
            catch (Exception ex)
            {
                resJson.Ok = false;
                resJson.Message = ex.Message;
            }
            return Json(resJson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult ModificarValorCombo(int idProducto, int idAtributo, int idValorAtributo, List<int> listaIdValorAtributo)
        {
            var errorString = "Ocurrio un error al editar el atributo";
            var resJson = new EditarValorAtributoComboJson()
            {
                Message = "",
                Ok = false,
                ProductoId = idProducto,
                AtributoId = idAtributo,
                ValorAtributoId = idValorAtributo,
                ListaValorPredefinidoId = listaIdValorAtributo
            };
            try
            {
                var resultadoOK = ManejadorProducto.GetInstance().ModificarValorAtributoCombo(idProducto, listaIdValorAtributo);
                resJson.Ok = resultadoOK;
                resJson.Message = resultadoOK ? "Se modifico el atributo" : errorString;
            }
            catch (Exception ex)
            {
                resJson.Ok = false;
                resJson.Message = ex.Message;
            }
            return Json(resJson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AgregarValorAtributoSimple(int idProducto, int idAtributoSimple, string nuevoValor)
        {
            var resJson = new AgregarValorAtributoSimpleJson()
            {
                Message = "Ocurrio un error al agregar el valor",
                Ok = false,
                ProductoId = idProducto,
                AtributoId = idAtributoSimple,
                NuevoValor = nuevoValor
            };
            try
            {
                ValorAtributoSimple nuevoAtributo = ManejadorProducto.GetInstance().AgregarValorAtributoSimple(idProducto, idAtributoSimple, nuevoValor);
                if (nuevoAtributo != null)
                {
                    resJson.Ok = true;
                    resJson.AtributoId = nuevoAtributo.ValorAtributoID;
                    resJson.NombreAtributo = nuevoAtributo.Atributo.Nombre;
                    resJson.Message = "Se agrego el atributo";
                }
            }
            catch (Exception ex)
            {
                resJson.Ok = false;
                resJson.Message = ex.Message;
            }
            return Json(resJson, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AgregarValorAtributoCombo(int idProducto, int idAtributo, int idValorPredefinido)
        {
            List<int> listaIdValorAtributo = new List<int>();
            listaIdValorAtributo.Add(idValorPredefinido);
            return AgregarValorAtributoComboMulti(idProducto, idAtributo, listaIdValorAtributo, null);
        }

        [HttpPost]
        public JsonResult AgregarValorAtributoComboMulti(int idProducto, int idAtributo, List<int> listaIdValorPredefinido, FormCollection collection)
        {
            var resJson = new AgregarValorAtributoComboJson()
            {
                Message = "Ocurrio un error al crear el atributo",
                Ok = false,
                ProductoId = idProducto,
                AtributoId = idAtributo,
                ListaValorPredefinidoId = listaIdValorPredefinido
            };
            try
            {
                var nuevoAtributo = ManejadorProducto.GetInstance().AgregarValorAtributoCombo(idProducto, idAtributo, listaIdValorPredefinido);
                if (nuevoAtributo != null)
                {
                    resJson.Ok = true;
                    resJson.Message = "Se creo el atributo";
                    resJson.AtributoId = nuevoAtributo.Atributo.AtributoID;
                    resJson.NombreAtributo = nuevoAtributo.Atributo.Nombre;
                    resJson.ListaValorPredefinidoId = new List<int>();
                    foreach (var valorPredefinido in nuevoAtributo.Valores)
                    {
                        resJson.ListaValorPredefinidoId.Add(valorPredefinido.ValorPredefinidoID);
                    }
                }
            }
            catch (Exception ex)
            {
                resJson.Ok = false;
                resJson.Message = ex.Message;
            }
            return Json(resJson, JsonRequestBehavior.AllowGet);
        }

    }

}

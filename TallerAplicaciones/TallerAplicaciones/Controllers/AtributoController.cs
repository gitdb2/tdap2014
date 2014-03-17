using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using TallerAplicaciones.Filters;
using TallerAplicaciones.Models;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.dominio.Exceptions;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.negocio;

namespace TallerAplicaciones.Controllers
{
    [CustomAuthorize]
    public class AtributoController : Controller
    {
        //
        // GET: /Atributo/
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            return View("Create");
        }

        //
        // GET: /Atributo/Create
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Atributo/Create

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create(AtributoModel model)
        {
            try
            {
                // TODO: Add insert logic here

                IAtributo iAtributo = ManejadorAtributo.GetInstance();

                if (model.DataCombo)
                {
                    iAtributo.AltaAtributoCombo(new AtributoCombo()
                    {
                        Nombre = model.Nombre,
                        DataCombo = model.DataCombo,
                        Valores = ValoresPredefinidosAtributos(model.Valores),
                        Activo = true,
                        EsSeleccionMultiple = model.MultiSeleccion
                    });
                }
                else
                {
                    iAtributo.AltaAtributoSimple(new AtributoSimple()
                    {
                        Nombre = model.Nombre,
                        DataCombo = model.DataCombo,
                        Activo = true
                    });
                }

                return RedirectToAction("Create");

            }
            catch (Exception e)
            {
                return View();
            }
        }


        private List<ValorPredefinido> ValoresPredefinidosAtributos(List<String> valores)
        {
            List<ValorPredefinido> aRetornar = new List<ValorPredefinido>();
            foreach (var v in valores)
            {
                ValorPredefinido valorPredefinido = new ValorPredefinido(v, true);
                aRetornar.Add(valorPredefinido);
            }
            return aRetornar;
        }

        //
        // GET: /Atributo/Edit/5
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult List()
        {
            ListAtributoModel model = null;
            try
            {
                IAtributo iAtributo = ManejadorAtributo.GetInstance();
                List<Atributo> listaAtributos = iAtributo.GetAtributos();
                model = new ListAtributoModel() { Atributos = listaAtributos }; 
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "ERROR");
            }
            return View(model);
        }

        //
        // GET: /Atributo/Edit/5
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(int idAtributo)
        {
            EditAtributoModel model = null;
            try
            {
                IAtributo iAtributo = ManejadorAtributo.GetInstance();
                Atributo atributos = iAtributo.GetAtributo(idAtributo);
                model = new EditAtributoModel()
                {
                    IdAtributo = atributos.AtributoID, 
                    Activo = atributos.Activo, 
                    Nombre = atributos.Nombre, 
                    DataCombo = atributos.DataCombo
                };
                if (atributos.DataCombo)
                {
                    AtributoCombo atributoCombo = iAtributo.GetAtributoCombo(idAtributo);
                    model.Valores = atributoCombo.Valores;
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "ERROR");
            }
            return View(model);
        }

        //
        // POST: /Atributo/Edit/5

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(EditPostAtributoModel model)
        {
            IAtributo iAtributo = ManejadorAtributo.GetInstance();
            if (ModelState.IsValid)
            {
                try
                {
                    if (model.DataCombo)
                    {
                        iAtributo.EditarAtributoCombo(model.IdAtributo, model.Activo, model.Nombre, model.DataCombo,
                            model.ListaBorrar, model.ValoresNuevos);
                    }
                    else
                    {
                        iAtributo.EditarAtributoSimple(model.IdAtributo, model.Activo, model.Nombre);
                    }

                    return RedirectToAction("List");

                }
                catch (CustomException e)
                {
                    ModelState.AddModelError(e.Key, e.Message);
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", e.Message);
                }
            }

            var errModel = new EditAtributoModel()
            {
                Activo = model.Activo,
                DataCombo = model.DataCombo,
                IdAtributo = model.IdAtributo,
                Nombre = model.Nombre
            };
            if (model.DataCombo)
            {
                AtributoCombo atributoCombo = iAtributo.GetAtributoCombo(model.IdAtributo);
                errModel.Valores = atributoCombo.Valores;
            }
            
            return View(errModel);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public JsonResult ObtenerValoresAtributos(int valorIdAtributo)
        {

            var ret = new ValoresAtributoJson();
            try
            {
                IAtributo iAtributo = ManejadorAtributo.GetInstance();
                Atributo atributo = iAtributo.GetAtributo(valorIdAtributo);
                ret.idAtributo = atributo.AtributoID;
                ret.nombreAtributo = atributo.Nombre;
                ret.esCombo = atributo.DataCombo;
                if (atributo.DataCombo)
                {
                    atributo = iAtributo.GetAtributoCombo(valorIdAtributo);
                    ret.esMultiselec = atributo.EsMultiseleccion();
                    ret.litaValores = atributo.ListaDeValoresActivosDeAtributo();
                }
                ret.Ok = true;
                ret.Message = atributo.Nombre;
            }
            catch (Exception e)
            {
                ret.Ok = false;
                ret.Message = e.Message;
            }
            return Json(ret);
        }
    }
}

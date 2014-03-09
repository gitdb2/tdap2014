using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Services.Description;
using TallerAplicaciones.Models;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.negocio;

namespace TallerAplicaciones.Controllers
{
    public class AtributoController : Controller
    {
        //
        // GET: /Atributo/

        public ActionResult Index()
        {
            return View("Create");
        }

        //
        // GET: /Atributo/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Atributo/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Atributo/Create

        [HttpPost]
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
                        EsSeleccionMultiple = model.DataCombo
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

        public ActionResult Edit(int idAtributo)
        {
            EditAtributoModel model = null;
            try
            {
                IAtributo iAtributo = ManejadorAtributo.GetInstance();
                Atributo atributos = iAtributo.GetAtributo(idAtributo);
                model = new EditAtributoModel() { IdAtributo = atributos.AtributoID, Activo = atributos.Activo, Nombre = atributos.Nombre, DataCombo = atributos.DataCombo};
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
        public ActionResult Edit(EditPostAtributoModel model)
        {
            try
            {
                // TODO: Add insert logic here

                IAtributo iAtributo = ManejadorAtributo.GetInstance();

                if (model.DataCombo)
                {
                    iAtributo.EditarAtributoCombo(model.IdAtributo, model.Activo, model.Nombre, model.DataCombo, model.ListaBorrar, model.ValoresNuevos);
                }
                else
                {
                    iAtributo.EditarAtributoSimple(model.IdAtributo, model.Activo, model.Nombre);
                }

                return RedirectToAction("List");

            }
            catch (Exception e)
            {
                return RedirectToAction("List");
            }
        }

        //
        // GET: /Atributo/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Atributo/Delete/5

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

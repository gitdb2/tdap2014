using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallerAplicaciones.Models;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.negocio;

namespace TallerAplicaciones.Controllers
{
    public class EmpresaController : Controller
    {
        //
        // GET: /Empresa/

        public ActionResult Index()
        {
            return View();
        }


        // GET: /Empresa/List
        public ActionResult List()
        {

            EmpresaListModel model = null;
            try
            {
                IEmpresaDistribuidora iEmpresa = ManejadorEmpresaDistribuidora.GetInstance();

                model = new EmpresaListModel() { Empresas = iEmpresa.ListarEmpresasDistribuidoras() };

            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "ERROR");
            }
            return View(model);

          
        }

        //
        // GET: /Empresa/Details/5

        public ActionResult Details(int id)
        {
            return View();
        }

        //
        // GET: /Empresa/Create
            [AllowAnonymous]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Empresa/Create

        [HttpPost]
        public ActionResult Create(AltaEmpresaModel model)
        {
            if (ModelState.IsValid)
            {
              
                try
                {
                    IEmpresaDistribuidora iEmpresa = ManejadorEmpresaDistribuidora.GetInstance();
                    iEmpresa.AltaEmpresa(new EmpresaDistribuidora() { Nombre = model.Nombre});

                    return RedirectToAction("List");
                  
                }
                catch (Exception e)
                {
                    ModelState.AddModelError("","ERROR");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);

        }

        //
        // GET: /Empresa/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Empresa/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, AltaEmpresaModel collection)
        {
            try
            {
                // TODO: Add update logic here

                return RedirectToAction("Index");
            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Empresa/Delete/5

        public ActionResult Delete(int id)
        {
            return View();
        }

        //
        // POST: /Empresa/Delete/5

        [HttpPost]
        public ActionResult Delete(int id, AltaEmpresaModel collection)
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

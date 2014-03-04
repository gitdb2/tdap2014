using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallerAplicaciones.Models;
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
            return View();
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
        public ActionResult Create(AtributoModel collection)
        {
            try
            {
                // TODO: Add insert logic here

                // Creo objeto a persistir con el maneger
                IAtributo iAtributo = ManejadorAtributo.GetInstance();
                //iEmpresa.AltaEmpresa(new EmpresaDistribuidora() { Nombre = model.Nombre});  ==> Analogo pero con Atributo <==

                return RedirectToAction("Index");

            }
            catch
            {
                return View();
            }
        }

        //
        // GET: /Atributo/Edit/5

        public ActionResult Edit(int id)
        {
            return View();
        }

        //
        // POST: /Atributo/Edit/5

        [HttpPost]
        public ActionResult Edit(int id, FormCollection collection)
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

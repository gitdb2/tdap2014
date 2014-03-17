using System;
using System.Collections.Generic;
using System.Linq;
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
    public class EmpresaController : Controller
    {
        //
        // GET: /Empresa/
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Index()
        {
            return View();
        }


        // GET: /Empresa/List
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult List(EmpresaListModel model)
        {

            //EmpresaListModel model = null;
            try
            {
                var iEmpresa = ManejadorEmpresaDistribuidora.GetInstance();

                model = new EmpresaListModel()
                {
                    Empresas = iEmpresa.ListarEmpresasDistribuidoras(ActivoEnum.Todos)
                };

            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "ERROR");
            }
            return View(model);


        }


        // GET: /Empresa/Create
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Empresa/Create

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Create(AltaEmpresaModel model)
        {
            if (ModelState.IsValid)
            {

                try
                {
                    IEmpresaDistribuidora iEmpresa = ManejadorEmpresaDistribuidora.GetInstance();
                    iEmpresa.AltaEmpresa(new EmpresaDistribuidora()
                    {
                        Nombre = model.Nombre,

                    });

                    return RedirectToAction("List");

                }
                catch (Exception e)
                {
                    ModelState.AddModelError("", "ERROR");
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);

        }

        //
        // GET: /Empresa/Edit/
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(int idEmpresa)
        {
            var model = GetEditEmpresaModel(idEmpresa);

            return View(model);
        }

        private static EmpresaEditModel GetEditEmpresaModel(int idEmpresa)
        {
            var empresa = ManejadorEmpresaDistribuidora.GetInstance().GetEmpresaDistribuidora(idEmpresa);
            if (empresa != null)
            {

                var model = new EmpresaEditModel
                {
                    Activo = empresa.Activo,
                    EmpresaId = empresa.EmpresaDistribuidoraID,
                    Nombre = empresa.Nombre
                };
                return model;
            }
            return null;


        }

        //
        // POST: /Empresa/Edit/5

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Edit(int idEmpresa, EmpresaEditModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var empresa = new EmpresaDistribuidora()
                {
                    Nombre = model.Nombre,
                    Activo = model.Activo,
                    EmpresaDistribuidoraID = model.EmpresaId,
                };

                ManejadorEmpresaDistribuidora.GetInstance().Modificar(empresa);


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
            var errorModel = GetEditEmpresaModel(model.EmpresaId);
            errorModel.Activo = model.Activo;
            errorModel.Nombre = model.Nombre;
         
            return View(errorModel);
        }

        //
        // GET: /Empresa/Delete/5
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Delete(int idEmpresa)
        {
            return View(new DeleteEmpresaModel { IdEmpresa = idEmpresa });
        }
        //
        // POST: /Empresa/Delete/5

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Delete(DeleteEmpresaModel model)
        {
            try
            {
                if (ManejadorEmpresaDistribuidora.GetInstance().Baja(model.IdEmpresa))
                {
                    return RedirectToAction("List");
                }
                ModelState.AddModelError("idEmpresa", "La empresa No fue  modificada");

            }

            catch (CustomException ex)
            {
                ModelState.AddModelError(ex.Key, ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("IdEmpresa", ex.Message);
            }

            return View(model);
        }
    }
}

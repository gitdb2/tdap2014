using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uy.edu.ort.taller.aplicaciones.dominio.Constants;

namespace TallerAplicaciones.Controllers
{
    public class ErrorController : Controller
    {
    
        public ActionResult AccessDenied()
        {
            Response.StatusCode = 403;

            ViewData[Constants.REQUEST__MESSAGE] = Session[Constants.SESSION_ERROR_MESSAGE] ?? (
                    Request.Params[Constants.REQUEST__MESSAGE] ?? "Acceso no permitido");
            return View();
        }

    }
}

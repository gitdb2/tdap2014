using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace TallerAplicaciones.Controllers
{
    public class ErrorController : Controller
    {
    
        public ActionResult AccessDenied()
        {
            Response.StatusCode = 403;

            ViewData["message"] =   Session["errorMessage"]??(
                    Request.Params["message"] ?? "Acceso no permitido");
            return View();
        }

    }
}

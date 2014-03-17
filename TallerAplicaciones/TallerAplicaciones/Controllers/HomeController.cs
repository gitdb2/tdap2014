using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TallerAplicaciones.Filters;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.dominio.Constants;

namespace TallerAplicaciones.Controllers
{
    [CustomAuthorize(Roles = "Administrador, EjecutivoDeCuenta")]
    public class HomeController : Controller
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public ActionResult Index()
        {
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";
            log.InfoFormat("Index de Home");

            var perfil = (PerfilUsuario) Session[Constants.SESSION_PERFIL];


            ViewBag.HomeDe = "Home de ";

            if (perfil.GetRolEnum() == UserRole.Administrador)
            {
                ViewBag.HomeDe += UserRole.Administrador.ToString();
                return View("Index_Admin");
            }

            ViewBag.HomeDe += "Ejecutivo de Cuenta";
            return View("Index_Ejecutivo");
           
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}

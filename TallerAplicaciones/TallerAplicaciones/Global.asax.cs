using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using TallerAplicaciones.Filters;
using uy.edu.ort.taller.aplicaciones.negocio;
using WebMatrix.WebData;

namespace TallerAplicaciones
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801


    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {


            using (var db = new Persistencia())
            {
                db.Productos.ToList();
            }

            if (!WebSecurity.Initialized)
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Usuario", "UsuarioID", "Login", autoCreateTables: true);

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
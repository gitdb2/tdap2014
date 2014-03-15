using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using log4net;
using TallerAplicaciones.Filters;
using TallerAplicaciones.logs;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.negocio;
using WebMatrix.WebData;

namespace TallerAplicaciones
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801


    public class MvcApplication : System.Web.HttpApplication
    {
        private static ILog log;
        protected void Application_Start()
        {
            //para que inicialice la base de datos
            using (var db = new Persistencia())
            {
                db.Productos.ToList();

                if (db.PerfilesUsuario.OfType<Administrador>().Any(p=>p.Activo))
                {
                    WebSecurity.CreateUserAndAccount("admin", "admin", propertyValues: new { Activo = true });

                    var perfil = new Administrador()
                    {
                        Nombre = "admin",
                        Apellido = "admin",
                        Activo = true,
                        Email = ""
                    };
                    ManejadorPerfilUsuario.GetInstance().AltaPerfilUsuario(perfil, "admin");
                }
            }

            if (!WebSecurity.Initialized)
                WebSecurity.InitializeDatabaseConnection("DefaultConnection", "Usuario", "UsuarioID", "Login", autoCreateTables: true);

            //FileInfo conf = new FileInfo("log4net.config");
            //bool exists = conf.Exists;
            //log4net.Config.XmlConfigurator.Configure(conf);
            log4net.Config.XmlConfigurator.Configure();
            log4net.GlobalContext.Properties["user"] = new HttpContextUserNameProvider();

            //log4net.GlobalContext.Properties["user"] = "System";
            log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

            log.Info("mierda");

            AreaRegistration.RegisterAllAreas();
            WebApiConfig.Register(GlobalConfiguration.Configuration);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
        }
    }
}
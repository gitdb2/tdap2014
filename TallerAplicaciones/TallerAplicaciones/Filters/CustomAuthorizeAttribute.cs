using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using System.Web.Routing;
using System.Web.Security;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.negocio;
using System.Security.Principal;

namespace TallerAplicaciones.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
      
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            if (!filterContext.ActionDescriptor.IsDefined
                      (typeof(AllowAnonymousAttribute), true) &&
                !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined
                 (typeof(AllowAnonymousAttribute), true))
            {


                var httpContext = filterContext.HttpContext;
                if (filterContext.HttpContext.Session == null ||
                    filterContext.HttpContext.Session["login"] == null)
                {

                  
                    //System.Web.HttpContext.Current.Application.Remove(System.Web.HttpContext.Current.User.Identity.Name);
                    filterContext.Result = new HttpUnauthorizedResult("Session perdida");
                    FormsAuthentication.SignOut();
                    HttpContext.Current.User =
                        new GenericPrincipal(new GenericIdentity(string.Empty), null);
                    return;
                }

                if (filterContext.HttpContext.Request.IsAuthenticated)
                {



                    string username = httpContext.User.Identity.Name;

                    var perfil = ManejadorPerfilUsuario.GetInstance().GetPerfilUsuarioByLogin(username);

                    //var session = httpContext.Session;
                    //usuario no existe, por lo que no tiene permiso
                    if (perfil == null || perfil.Activo == false)
                    {
                        base.OnAuthorization(filterContext); //returns to login url
                        return;
                    }


                    if (filterContext.HttpContext != null && filterContext.HttpContext.Session != null)
                    {
                        if (filterContext.HttpContext.Session["perfil"] == null)
                        {
                            filterContext.HttpContext.Session["perfil"] = perfil;
                        }

                        if (filterContext.HttpContext.Session["login"] == null)
                        {
                            filterContext.HttpContext.Session["login"] = username;
                        }
                    }

                    if (this.Roles == null || this.Roles.Trim().Length == 0 || IsInRole(perfil, CleanRoles()))
                        return;


                    //filterContext.Result = new HttpStatusCodeResult(403, "Ud no tiene permiso para acceder a esta pagina. Solo los " + this.Roles +" pueden hacerlo.");

                    if (httpContext.Session != null)
                    {


                        httpContext.Session["errorMessage"] =
                            "Ud no tiene permiso para acceder a esta pagina. Solo los usuarios: " + this.Roles +
                            " pueden hacerlo.";
                        filterContext.Result = new RedirectToRouteResult(new
                            RouteValueDictionary
                            (new
                            {
                                controller = "Error",
                                action = "AccessDenied"


                            }));
                    }
                    else
                    {
                        filterContext.Result = new RedirectToRouteResult(new
                       RouteValueDictionary
                       (new
                       {
                           controller = "Error",
                           action = "AccessDenied",
                           message = "No tienes acceso a ese recurso"


                       }));
                    }




                }
                else
                {
                    Console.Out.WriteLine("ccccccccccc");
                    base.OnAuthorization(filterContext);
                }
            }

        }




        private List<string> CleanRoles()
        {
            var ret = new List<string>();
            var arrRoles = this.Roles.Split(',');

            foreach (var rol in arrRoles)
            {
                if (rol != null && rol.Trim().Length > 0)
                {
                    ret.Add(rol.Trim());
                }

            }
            return ret;
        }

        private bool IsInRole(PerfilUsuario perfil, List<string> roles)
        {
            if (roles == null || !roles.Any()) return true;

            bool found = false;

            foreach (var role in roles)
            {
                UserRole tmp;
                if (Enum.TryParse(role, true, out tmp))
                {
                    if (perfil.GetRolEnum() == tmp)
                    {
                        found = true;
                        break;
                    }
                }
            }

            return found;
        }
    }
}
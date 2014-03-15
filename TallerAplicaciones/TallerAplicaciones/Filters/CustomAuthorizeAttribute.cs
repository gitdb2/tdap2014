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
using uy.edu.ort.taller.aplicaciones.dominio.Constants;
using uy.edu.ort.taller.aplicaciones.negocio;
using System.Security.Principal;

namespace TallerAplicaciones.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
      
        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            //chequea si no tiene annotation AllowAnonymous
            if (!filterContext.ActionDescriptor.IsDefined
                      (typeof(AllowAnonymousAttribute), true) &&
                !filterContext.ActionDescriptor.ControllerDescriptor.IsDefined
                 (typeof(AllowAnonymousAttribute), true))
            {

                //En la sesion debe estar el login, de lo contrario de fuera que el usuario se loguee de nuevo
                var httpContext = filterContext.HttpContext;
                if (filterContext.HttpContext.Session == null ||
                    filterContext.HttpContext.Session[Constants.SESSION_LOGIN] == null)
                {

                  
                    filterContext.Result = new HttpUnauthorizedResult("Session perdida");
                    FormsAuthentication.SignOut();
                    HttpContext.Current.User =
                        new GenericPrincipal(new GenericIdentity(string.Empty), null);
                    return;
                }

                //si esta la cookie de identificacion, voy a chequear los permisos de acceso
                if (filterContext.HttpContext.Request.IsAuthenticated)
                {



                    string username = httpContext.User.Identity.Name;

                    var perfil = ManejadorPerfilUsuario.GetInstance().GetPerfilUsuarioByLogin(username);

                
                    //usuario no existe, por lo que no tiene permiso
                    if (perfil == null || perfil.Activo == false)
                    {
                        //base.OnAuthorization(filterContext); //returns to login url
                        //return;
                        filterContext.Result = new HttpUnauthorizedResult("Session perdida");
                        FormsAuthentication.SignOut();
                        HttpContext.Current.User =
                            new GenericPrincipal(new GenericIdentity(string.Empty), null);
                        return;
                    }


                    if (filterContext.HttpContext != null && filterContext.HttpContext.Session != null)
                    {
                        if (filterContext.HttpContext.Session[Constants.SESSION_PERFIL] == null)
                        {
                            filterContext.HttpContext.Session[Constants.SESSION_PERFIL] = perfil;
                        }

                        if (filterContext.HttpContext.Session[Constants.SESSION_LOGIN] == null)
                        {
                            filterContext.HttpContext.Session[Constants.SESSION_LOGIN] = username;
                        }
                    }

                    if (this.Roles == null || this.Roles.Trim().Length == 0 || IsInRole(perfil, CleanRoles()))
                        return;


                    if (httpContext.Session != null)
                    {


                        httpContext.Session[Constants.SESSION_ERROR_MESSAGE] =
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
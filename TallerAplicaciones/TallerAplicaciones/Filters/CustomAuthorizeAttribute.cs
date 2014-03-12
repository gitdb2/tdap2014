using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.negocio;

namespace TallerAplicaciones.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        //protected override bool AuthorizeCore(HttpContextBase httpContext)
        //{
        //    if (!httpContext.User.Identity.IsAuthenticated)
        //    {
        //        // no user is authenticated => no need to go any further
        //        return false;

        //    }


        //    // at this stage we have an authenticated user
        //    string username = httpContext.User.Identity.Name;

        //    var perfil = ManejadorPerfilUsuario.GetInstance().GetPerfilUsuarioByLogin(username);

        //    var session = httpContext.Session;
        //    //usuario no existe, por lo que no tiene permiso
        //    if (perfil == null || perfil.Activo == false) return false;

        //    //if (perfil.GetRol() == (int) UserRole.EjecutivoDeCuenta)
        //    //{
        //    //    session
        //    //}

        //    if (this.Roles == null || this.Roles.Trim().Length == 0)
        //        return true;
        //    if (IsInRole(perfil, CleanRoles()))
        //    {
        //        return true;
        //    }
        //    else
        //    {
        //        throw new UnauthorizedAccessException("No tiene permiso para acceder a esa pagina.");

        //    }
        //}



        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            if (filterContext.HttpContext.Request.IsAuthenticated)
            {

                var httpContext = filterContext.HttpContext;

                string username = httpContext.User.Identity.Name;

                var perfil = ManejadorPerfilUsuario.GetInstance().GetPerfilUsuarioByLogin(username);

                //var session = httpContext.Session;
                //usuario no existe, por lo que no tiene permiso
                if (perfil == null || perfil.Activo == false)
                {
                    base.OnAuthorization(filterContext); //returns to login url
                    return;
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
                       message="No tienes acceso a ese recurso"


                   }));
                }




            }
            else
            {
                base.OnAuthorization(filterContext);
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
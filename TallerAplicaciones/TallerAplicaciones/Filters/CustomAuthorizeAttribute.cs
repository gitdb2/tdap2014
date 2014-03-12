using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.negocio;

namespace TallerAplicaciones.Filters
{
    public class CustomAuthorizeAttribute : AuthorizeAttribute
    {
        protected override bool AuthorizeCore(HttpContextBase httpContext)
        {
            if (!httpContext.User.Identity.IsAuthenticated)
            {
                // no user is authenticated => no need to go any further
                return false;
             
            }


            // at this stage we have an authenticated user
            string username = httpContext.User.Identity.Name;

            var perfil = ManejadorPerfilUsuario.GetInstance().GetPerfilUsuarioByLogin(username);

            var session = httpContext.Session;
            //usuario no existe, por lo que no tiene permiso
            if (perfil == null || perfil.Activo == false) return false;

            //if (perfil.GetRol() == (int) UserRole.EjecutivoDeCuenta)
            //{
            //    session
            //}

            if (this.Roles == null || this.Roles.Trim().Length == 0)
                return true;
            if (IsInRole(perfil, CleanRoles()))
            {
                return true;
            }
            else
            {
                throw new UnauthorizedAccessException("No tiene permiso para acceder a esa pagina.");

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
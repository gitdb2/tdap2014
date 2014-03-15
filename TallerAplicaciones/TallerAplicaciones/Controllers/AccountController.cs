using System;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Web.WebPages.OAuth;
using uy.edu.ort.taller.aplicaciones.dominio.Constants;
using WebMatrix.WebData;
using TallerAplicaciones.Filters;
using TallerAplicaciones.Models;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.negocio;

namespace TallerAplicaciones.Controllers
{
    [CustomAuthorize]
    //[InitializeSimpleMembership]
    public class AccountController : Controller
    {
        
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //
        // GET: /Account/Login

        [AllowAnonymous]
        public ActionResult Login(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View();
        }

        //
        // POST: /Account/Login

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginModel model, string returnUrl)
        {
            IPerfilUsuario iPerfilUsuario = ManejadorPerfilUsuario.GetInstance();
            var usuario = iPerfilUsuario.ObtenerUsuario(model.UserName);
            if (usuario != null && usuario.Activo)
            {
                if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
                {
                    Session[Constants.SESSION_LOGIN] = model.UserName;

                    Session[Constants.SESSION_PERFIL] = ManejadorPerfilUsuario.GetInstance().GetPerfilUsuarioByLogin(model.UserName);
                    log.InfoFormat("Logueo correcto");
                    return RedirectToLocal(returnUrl);
                }    
            }
            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "Usuario y/o Password invalido o Usuario inactivo");
            log.ErrorFormat("Usuario y/o Password invalido o Usuario inactivo");
            return View(model);
        }

        //
        // POST: /Account/LogOff

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult LogOff()
        {
            WebSecurity.Logout();

            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/Register

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Register()
        {
            var model = new RegisterModel();
            model.EmpresasDistribuidoras = ManejadorEmpresaDistribuidora.GetInstance().ListarEmpresasDistribuidoras();
            return View(model);
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password, propertyValues: new { Activo = model.Activo });
                    AltaUsuario(model);
                    return RedirectToAction("Index", "Home");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        private void AltaUsuario(RegisterModel model)
        {
           var iPerfil = ManejadorPerfilUsuario.GetInstance();
           
            PerfilUsuario perfil = null;
            switch (model.Rol)
            {
                case (int) UserRole.Administrador:
                    perfil= new Administrador()
                    {
                        Nombre = model.Nombre,
                        Apellido = model.Apellido,
                        Activo = true,
                        Email = model.Email
                    };
                    iPerfil.AltaPerfilUsuario(perfil, model.UserName);
                    break;

                case (int) UserRole.EjecutivoDeCuenta:
                    perfil = new EjecutivoDeCuenta()
                    {
                        Nombre = model.Nombre,
                        Apellido = model.Apellido,
                        Activo = true,
                        Email = model.Email,
                     };
                    iPerfil.AltaPerfilUsuario((EjecutivoDeCuenta) perfil, model.UserName, model.EmpresasSeleccionadas);
                    break;

                case (int) UserRole.Distribuidor:
                    perfil = new Distribuidor()
                    {
                        Nombre = model.Nombre,
                        Apellido = model.Apellido,
                        Activo = true,
                        Email = model.Email
                    };
                    iPerfil.AltaPerfilUsuario((Distribuidor) perfil, model.EmpresaDelDistribuidor, model.UserName);
                    break;

                default:
                    throw new ArgumentException("Tipo de usuario invalido");
            }
        }

        //
        // GET: /Account/List

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult List()
        {
            UsuarioListModel model = null;
            try
            {
                IPerfilUsuario iPerfilUsuario = ManejadorPerfilUsuario.GetInstance();
                model = new UsuarioListModel() { PerfilesDeUsuario = iPerfilUsuario.ListarUsuarios() };
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "ERROR");
            }
            return View(model);
        }

        //
        // GET: /Account/Modify

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Modify(int idPerfilUsuario)
        {
            ModificarUsuarioModel model = null;
            try
            {
                IPerfilUsuario iPerfilUsuario = ManejadorPerfilUsuario.GetInstance();
                PerfilUsuario perfil = iPerfilUsuario.ObtenerPerfil(idPerfilUsuario);

                if (perfil.GetRol() == (int) UserRole.Distribuidor)
                {
                    return RedirectToActionPermanent("ModifyDistribuidor", new { idDistrib = idPerfilUsuario });
                }
                
                if (perfil.GetRol() == (int) UserRole.EjecutivoDeCuenta)
                {
                    return RedirectToActionPermanent("ModifyEjecutivo", new { idDistrib = idPerfilUsuario });
                }
                
                model = new ModificarUsuarioModel() { 
                    PerfilUsuarioID = idPerfilUsuario,
                    Rol = perfil.GetRol(),
                    Nombre = perfil.Nombre,
                    Apellido = perfil.Apellido,
                    Email = perfil.Email,
                    UserName = perfil.Usuario.Login,
                    Activo = perfil.Activo
                };
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "ERROR");
            }
            return View(model);
        }

        //
        // POST: /Account/Modify

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult Modify(ModificarUsuarioModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IPerfilUsuario iPerfilUsuario = ManejadorPerfilUsuario.GetInstance();
                    PerfilUsuario perfilUsuario = iPerfilUsuario.ObtenerPerfilUsuarioSegunRol(model.Rol);
                    perfilUsuario.PerfilUsuarioID = model.PerfilUsuarioID;
                    perfilUsuario.Nombre = model.Nombre;
                    perfilUsuario.Apellido = model.Apellido;
                    perfilUsuario.Email = model.Email;
                    perfilUsuario.Activo = model.Activo;

                    iPerfilUsuario.ModificarPerfilUsuario(perfilUsuario);

                    return RedirectToAction("List");
                }
                catch (MembershipCreateUserException e)
                {
                    ModelState.AddModelError("", ErrorCodeToString(e.StatusCode));
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/ChangePassword

        [HttpPost]
        [AllowAnonymous]
        public ActionResult ChangePassword(LocalPasswordModel model)
        {
            if (ModelState.IsValid)
            {
                bool changePasswordSucceeded;
                try
                {
                    changePasswordSucceeded = WebSecurity.ChangePassword(model.LoginUsuario, model.OldPassword, model.NewPassword);
                }
                catch (Exception)
                {
                    changePasswordSucceeded = false;
                }
                if (changePasswordSucceeded)
                {
                    return RedirectToAction("List", "Account");
                }
                else
                {
                    ModelState.AddModelError("", "El password actual es incorrecto o el nuevo password es invalido");
                }
            }
            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // POST: /Account/ChangePassword

        [AllowAnonymous]
        public ActionResult ChangePassword(string login)
        {
            LocalPasswordModel model = null;
            try
            {
                model = new LocalPasswordModel() { LoginUsuario = login };
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "ERROR");
            }
            return View(model);
        }

        #region Helpers
        private ActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }
        }

        public enum ManageMessageId
        {
            ChangePasswordSuccess,
            SetPasswordSuccess,
            RemoveLoginSuccess,
        }

        private static string ErrorCodeToString(MembershipCreateStatus createStatus)
        {
            // See http://go.microsoft.com/fwlink/?LinkID=177550 for
            // a full list of status codes.
            switch (createStatus)
            {
                case MembershipCreateStatus.DuplicateUserName:
                    return "User name already exists. Please enter a different user name.";

                case MembershipCreateStatus.DuplicateEmail:
                    return "A user name for that e-mail address already exists. Please enter a different e-mail address.";

                case MembershipCreateStatus.InvalidPassword:
                    return "The password provided is invalid. Please enter a valid password value.";

                case MembershipCreateStatus.InvalidEmail:
                    return "The e-mail address provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidAnswer:
                    return "The password retrieval answer provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidQuestion:
                    return "The password retrieval question provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.InvalidUserName:
                    return "The user name provided is invalid. Please check the value and try again.";

                case MembershipCreateStatus.ProviderError:
                    return "The authentication provider returned an error. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                case MembershipCreateStatus.UserRejected:
                    return "The user creation request has been canceled. Please verify your entry and try again. If the problem persists, please contact your system administrator.";

                default:
                    return "An unknown error occurred. Please verify your entry and try again. If the problem persists, please contact your system administrator.";
            }
        }
        #endregion

        public ActionResult Index()
        {
            throw new NotImplementedException();
        }

        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult ModifyDistribuidor(int idDistrib)
        {
            Distribuidor dist = ManejadorPerfilUsuario.GetInstance().FindDistribuidor(idDistrib);
            var model = new RegisterModel
            {
                Activo = dist.Activo,
                PerfilUsuario = dist,
                idPerfil = idDistrib,
                Rol = dist.GetRol(),
                Nombre = dist.Nombre,
                Apellido = dist.Apellido,
                Email = dist.Email,
                UserName = dist.Usuario.Login,
                EmpresaDelDistribuidor = dist.Empresa!=null ? dist.Empresa.EmpresaDistribuidoraID : -1,
                EmpresasDistribuidoras = ManejadorEmpresaDistribuidora.GetInstance().ListarEmpresasDistribuidoras()
            };

            return View(model);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
         public ActionResult ModifyDistribuidor(RegisterModel model)
        {
            Distribuidor dist = ManejadorPerfilUsuario.GetInstance().FindDistribuidor(model.idPerfil);
              dist.Apellido = model.Apellido;
              dist.Email = model.Email;

              dist.PerfilUsuarioID = model.idPerfil;
              dist.Nombre = model.Nombre;
              dist.Apellido = model.Apellido;
              dist.Email = model.Email;
            dist.Activo = model.Activo;

           ManejadorPerfilUsuario.GetInstance().ModificarPerfilUsuario(dist);
           ManejadorPerfilUsuario.GetInstance().UpdateCompany(model.idPerfil, model.EmpresaDelDistribuidor);

           return RedirectToAction("List");
        }

         [CustomAuthorize(Roles = "Administrador")]
        public ActionResult ModifyEjecutivo(int idDistrib)
        {
            EjecutivoDeCuenta ejec = ManejadorPerfilUsuario.GetInstance().FindEjecutivo(idDistrib);
            var model = new RegisterModel
            {
                PerfilUsuario = ejec,
                Activo = ejec.Activo,
                idPerfil = idDistrib,
                Rol = ejec.GetRol(),
                Nombre = ejec.Nombre,
                Apellido = ejec.Apellido,
                Email = ejec.Email,
                UserName = ejec.Usuario.Login,
                EmpresasSeleccionadas = ManejadorEmpresaDistribuidora.GetInstance().GetEmpresasDeEjecutivo(idDistrib),
                EmpresasDistribuidoras = ManejadorEmpresaDistribuidora.GetInstance().ListarEmpresasDistribuidoras()
            };

            return View(model);
        }

        [HttpPost]
        [CustomAuthorize(Roles = "Administrador")]
        public ActionResult ModifyEjecutivo(RegisterModel model)
        {
            EjecutivoDeCuenta dist = ManejadorPerfilUsuario.GetInstance().FindEjecutivo(model.idPerfil);
            dist.Apellido = model.Apellido;
            dist.Email = model.Email;

            dist.PerfilUsuarioID = model.idPerfil;
            dist.Nombre = model.Nombre;
            dist.Apellido = model.Apellido;
            dist.Email = model.Email;
            dist.Activo = model.Activo;

            ManejadorPerfilUsuario.GetInstance().ModificarPerfilUsuario(dist, model.EmpresasSeleccionadas);

            return RedirectToAction("List");
        }

    }
}

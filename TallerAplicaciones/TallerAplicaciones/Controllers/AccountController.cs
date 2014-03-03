using System;
using System.Transactions;
using System.Web.Mvc;
using System.Web.Security;
using Microsoft.Web.WebPages.OAuth;
using WebMatrix.WebData;
using TallerAplicaciones.Filters;
using TallerAplicaciones.Models;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.negocio;

namespace TallerAplicaciones.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    public class AccountController : Controller
    {
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
            if (ModelState.IsValid && WebSecurity.Login(model.UserName, model.Password, persistCookie: model.RememberMe))
            {
                return RedirectToLocal(returnUrl);
            }

            // If we got this far, something failed, redisplay form
            ModelState.AddModelError("", "The user name or password provided is incorrect.");
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

        [AllowAnonymous]
        public ActionResult Register()
        {
            var model = new RegisterModel();
            model.EmpresasDistribuidoras = ManejadorEmpresaDistribuidora.GetInstance().ListarEmpresasDistribuidoras();
            return View(model);
        }

        //
        // POST: /Account/Register

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                // Attempt to register the user
                try
                {
                    WebSecurity.CreateUserAndAccount(model.UserName, model.Password, propertyValues: new { Activo = model.Activo });
                    WebSecurity.Login(model.UserName, model.Password);

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
            IPerfilUsuario iPerfil = ManejadorPerfilUsuario.GetInstance();
           
            PerfilUsuario perfil = null;
            switch (model.Rol)
            {
                case 0:
                    perfil= new Administrador()
                    {
                        Nombre = model.Nombre,
                        Apellido = model.Apellido,
                        Activo = true,
                        Email = model.Email
                    };
                    iPerfil.AltaPerfilUsuario(perfil, model.UserName);
                    break;
                case 1:
                    perfil = new EjecutivoDeCuenta()
                    {
                        Nombre = model.Nombre,
                        Apellido = model.Apellido,
                        Activo = true,
                        Email = model.Email,
                     };
                    iPerfil.AltaPerfilUsuario((EjecutivoDeCuenta) perfil, model.UserName, model.EmpresasSeleccionadas);
                    break;
                case 2:
                    perfil = new Distribuidor()
                    {
                        Nombre = model.Nombre,
                        Apellido = model.Apellido,
                        Activo = true,
                        Email = model.Email,
                        Empresa = ManejadorEmpresaDistribuidora.GetInstance().GetEmpresaDistribuidora(model.EmpresaDelDistribuidor)
                    };
                    iPerfil.AltaPerfilUsuario(perfil, model.UserName);
                    break;
                default:
                    throw new ArgumentException("Tipo de usuario invalido");
            }
           
        }

        // TODO
        // esto esta muy mal hecho
        // hay que arreglarlo
//        private int ObtenerRolSegunPerfilUsuario(PerfilUsuario perfil)
//        {
//            if (perfil is Administrador)
//            {
//                return 0;
//            } 
//            else if (perfil is EjecutivoDeCuenta)
//            {
//                return 1;
//            }
//            else if (perfil is Distribuidor)
//            {
//                return 2;
//            }
//            else
//            {
//                throw new ArgumentException("Tipo de usuario invalido");
//            }
//        }

        // TODO
        // esto esta muy mal hecho
        // hay que arreglarlo
        private PerfilUsuario ObtenerPerfilUsuarioSegunRol(int rolId)
        {
            switch (rolId)
            {
                case 0:
                    return new Administrador();
                case 1:
                    return new EjecutivoDeCuenta();
                case 2:
                    return new Distribuidor();
                default:
                    throw new ArgumentException("Tipo de usuario invalido");
            }
        }

        //
        // POST: /Account/Disassociate

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Disassociate(string provider, string providerUserId)
        {
            string ownerAccount = OAuthWebSecurity.GetUserName(provider, providerUserId);
            ManageMessageId? message = null;

            // Only disassociate the account if the currently logged in user is the owner
            if (ownerAccount == User.Identity.Name)
            {
                // Use a transaction to prevent the user from deleting their last login credential
                using (var scope = new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.Serializable }))
                {
                    bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
                    if (hasLocalAccount || OAuthWebSecurity.GetAccountsFromUserName(User.Identity.Name).Count > 1)
                    {
                        OAuthWebSecurity.DeleteAccount(provider, providerUserId);
                        scope.Complete();
                        message = ManageMessageId.RemoveLoginSuccess;
                    }
                }
            }

            return RedirectToAction("Manage", new { Message = message });
        }

        //
        // GET: /Account/Manage

        public ActionResult Manage(ManageMessageId? message)
        {
            ViewBag.StatusMessage =
                message == ManageMessageId.ChangePasswordSuccess ? "Your password has been changed."
                : message == ManageMessageId.SetPasswordSuccess ? "Your password has been set."
                : message == ManageMessageId.RemoveLoginSuccess ? "The external login was removed."
                : "";
            ViewBag.HasLocalPassword = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.ReturnUrl = Url.Action("Manage");
            return View();
        }

        //
        // POST: /Account/Manage

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Manage(LocalPasswordModel model)
        {
            bool hasLocalAccount = OAuthWebSecurity.HasLocalAccount(WebSecurity.GetUserId(User.Identity.Name));
            ViewBag.HasLocalPassword = hasLocalAccount;
            ViewBag.ReturnUrl = Url.Action("Manage");
            if (hasLocalAccount)
            {
                if (ModelState.IsValid)
                {
                    // ChangePassword will throw an exception rather than return false in certain failure scenarios.
                    bool changePasswordSucceeded;
                    try
                    {
                        changePasswordSucceeded = WebSecurity.ChangePassword(User.Identity.Name, model.OldPassword, model.NewPassword);
                    }
                    catch (Exception)
                    {
                        changePasswordSucceeded = false;
                    }

                    if (changePasswordSucceeded)
                    {
                        return RedirectToAction("Manage", new { Message = ManageMessageId.ChangePasswordSuccess });
                    }
                    else
                    {
                        ModelState.AddModelError("", "The current password is incorrect or the new password is invalid.");
                    }
                }
            }
            else
            {
                // User does not have a local password so remove any validation errors caused by a missing
                // OldPassword field
                ModelState state = ModelState["OldPassword"];
                if (state != null)
                {
                    state.Errors.Clear();
                }

                if (ModelState.IsValid)
                {
                    try
                    {
                        WebSecurity.CreateAccount(User.Identity.Name, model.NewPassword);
                        return RedirectToAction("Manage", new { Message = ManageMessageId.SetPasswordSuccess });
                    }
                    catch (Exception e)
                    {
                        ModelState.AddModelError("", e);
                    }
                }
            }

            // If we got this far, something failed, redisplay form
            return View(model);
        }

        //
        // GET: /Account/List

        [AllowAnonymous]
        public ActionResult List()
        {
            UsuarioListModel model = null;
            try
            {
                IPerfilUsuario iPerfilUsuario = ManejadorPerfilUsuario.GetInstance();
                model = new UsuarioListModel() { PerfilesDeUsuario = iPerfilUsuario.ListarUsuarios(false) };
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "ERROR");
            }
            return View(model);
        }

        //
        // GET: /Account/Delete

        [AllowAnonymous]
        public ActionResult Delete(int idPerfilUsuario)
        {
            try
            {
                IPerfilUsuario iPerfilUsuario = ManejadorPerfilUsuario.GetInstance();
                iPerfilUsuario.BajaPerfilUsuario(idPerfilUsuario);
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "ERROR");
            }
            return RedirectToAction("List");
        }

        //
        // GET: /Account/Modify
        [AllowAnonymous]
        public ActionResult EditEmpresa(int idPerfilUsuario)
        {
            Distribuidor dist = ManejadorPerfilUsuario.GetInstance().FindDistribuidor(idPerfilUsuario);
            var model = new RegisterModel()

            {
                PerfilUsuario = dist,
                EmpresaDelDistribuidor = dist.Empresa.EmpresaDistribuidoraID,
                EmpresasDistribuidoras =  ManejadorEmpresaDistribuidora.GetInstance().ListarEmpresasDistribuidoras()

            };
             return View(model);
            
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult EditEmpresa(RegisterModel model)
        {


           ManejadorPerfilUsuario.GetInstance().UpdateCompany(model.idPerfil, model.EmpresaDelDistribuidor);
//           EmpresaDistribuidora emp=  ManejadorEmpresaDistribuidora.GetInstance().GetEmpresaDistribuidora(model.EmpresaDelDistribuidor);
//
//            dist.Empresa = emp;
//
//            ManejadorPerfilUsuario.GetInstance().ModificarPerfilUsuario(dist);
//

            return View("WindowClose");

        }


        [AllowAnonymous]
        public ActionResult Modify(int idPerfilUsuario)
        {
            ModificarUsuarioModel model = null;
            try
            {
                IPerfilUsuario iPerfilUsuario = ManejadorPerfilUsuario.GetInstance();
                PerfilUsuario perfil = iPerfilUsuario.ObtenerPerfil(idPerfilUsuario);
                model = new ModificarUsuarioModel() { 
                    PerfilUsuarioID = idPerfilUsuario,
                    Rol = perfil.GetRol(),
                    Nombre = perfil.Nombre,
                    Apellido = perfil.Apellido,
                    Email = perfil.Email,
                    UserName = perfil.Usuario.Login
                };
            }
            catch (Exception e)
            {
                ModelState.AddModelError("", "ERROR");
            }
            return View(model);
        }

        [HttpPost]
        [AllowAnonymous]
        public ActionResult Modify(ModificarUsuarioModel model)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    IPerfilUsuario iPerfilUsuario = ManejadorPerfilUsuario.GetInstance();
                    PerfilUsuario perfilUsuario = ObtenerPerfilUsuarioSegunRol(model.Rol);
                    perfilUsuario.PerfilUsuarioID = model.PerfilUsuarioID;
                    perfilUsuario.Nombre = model.Nombre;
                    perfilUsuario.Apellido = model.Apellido;
                    perfilUsuario.Email = model.Email;
                    perfilUsuario.Activo = true;

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
    }
}

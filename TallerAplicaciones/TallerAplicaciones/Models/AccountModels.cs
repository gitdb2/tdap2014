using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data.Entity;
using System.Globalization;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.UI.WebControls;
using System.Web.DynamicData;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace TallerAplicaciones.Models
{

    public class LocalPasswordModel
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Current password")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "New password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm new password")]
        [Compare("NewPassword", ErrorMessage = "The new password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "User name")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        public bool Activo = true;

        [Required]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Rol")]
        public int Rol { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required]
        [Display(Name = "Email")]
   //     [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Ingrese una direccion de email valida")]
        public string Email { get; set; }

        [Required]
    //    [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar password")]
        [Compare("Password", ErrorMessage = "El password y la confirmacion no coinciden")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Empresas Distribuidoras")]
        public List<EmpresaDistribuidora> EmpresasDistribuidoras { get; set; }

   //     [Required]
        [Display(Name = "Empresa del Distribuidor")]
        public int EmpresaDelDistribuidor { get; set; }

      //  [Required]
        [Display(Name = "Empresa Distribuidoras Asosciadas")]
        public List<int> EmpresasSeleccionadas { get; set; }

    }

    public class UsuarioListModel
    {
        [Display(Name = "Usuarios")]
        public List<PerfilUsuario> PerfilesDeUsuario { get; set; }
    }

}

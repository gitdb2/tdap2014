using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace TallerAplicaciones.Models
{

    public class LocalPasswordModel
    {
        public string LoginUsuario { get; set; }
        
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password actual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nuevo password")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar el nuevo password")]
        [Compare("NewPassword", ErrorMessage = "El password y su confirmacion no coinciden")]
        public string ConfirmPassword { get; set; }
    }

    public class LoginModel
    {
        [Required]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [Display(Name = "Recordarme?")]
        public bool RememberMe { get; set; }
    }

    public class RegisterModel
    {
        public RegisterModel ()
        {
            EmpresasSeleccionadas = new List<int>();
            EmpresasDistribuidoras =  new List<EmpresaDistribuidora>();
            Activo = true;
        }

        public PerfilUsuario PerfilUsuario { get; set; }

        public int idPerfil { get; set; }

        public bool Activo { get; set; }

        [Required]
        [Display(Name = "Login")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)$", ErrorMessage = "Ingrese Login sin simbolos ni espacions")]
        [MaxLength(12, ErrorMessage = "El Login debe tener como máximo {2} caracteres")] 
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
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Ingrese una direccion de email valida")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "El {0} debe tener al menos {2} caracteres", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar password")]
        [Compare("Password", ErrorMessage = "El password y la confirmacion no coinciden")]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Empresas Distribuidoras")]
        public List<EmpresaDistribuidora> EmpresasDistribuidoras { get; set; }

        [Required]
        [Display(Name = "Empresa del Distribuidor")]
        public int EmpresaDelDistribuidor { get; set; }

        [Required]
        [Display(Name = "Empresa Distribuidoras Asosciadas")]
        public List<int> EmpresasSeleccionadas { get; set; }

    }

    public class UsuarioListModel
    {
        [Display(Name = "Usuarios")]
        public List<PerfilUsuario> PerfilesDeUsuario { get; set; }
    }

    public class ModificarUsuarioModel
    {
        public int PerfilUsuarioID { get; set; }
        public int Rol { get; set; }

        [Required]
        [Display(Name = "Login")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Apellido")]
        public string Apellido { get; set; }

        [Required]
        [Display(Name = "Email")]
        [RegularExpression(@"^([a-zA-Z0-9_\-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([a-zA-Z0-9\-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", ErrorMessage = "Ingrese una direccion de email valida")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Activo")]
        public bool Activo { get; set; }
    }

}

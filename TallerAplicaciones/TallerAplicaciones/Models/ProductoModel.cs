﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web;
using System.Web.Mvc;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace TallerAplicaciones.Models
{
    public class ProductoModel
    {
        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(5, ErrorMessage = "El {0} debe tener {2} caracteres", MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9]{5}$", ErrorMessage = "Tiene que tener exactamente 5 caracteres a-z 0-9 sin blancos")]
        [Display(Name = "Codigo")]
        public string Codigo { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        public bool Activo { get; set; }

        public int ProductoID { get; set; }
    }

    public class ProductoListModel
    {
        [Display(Name = "Productos")]
        public List<Producto> Productos { get; set; }
         
    }


     public class ProductoConArchivosSubmitModel
    {
        [Required]
        [Display(Name = "Nombre")]
        public string Nombre { get; set; }

        [Required]
        [StringLength(5, ErrorMessage = "El {0} debe tener {2} caracteres", MinimumLength = 5)]
        [RegularExpression(@"^[a-zA-Z0-9]{5}$", ErrorMessage = "Tiene que tener exactamente 5 caracteres a-z 0-9 sin blancos")]
        [Display(Name = "Codigo")]
        public string Codigo { get; set; }

        [Required]
        [Display(Name = "Descripcion")]
        public string Descripcion { get; set; }

        [Display(Name = "Fotos")]
        public IEnumerable<HttpPostedFileBase> Fotos { get; set; }
        [Display(Name = "Videos")]
        public IEnumerable<HttpPostedFileBase> Videos { get; set; } 

        public bool Activo { get; set; }

        public int ProductoID { get; set; }
    }
    

}
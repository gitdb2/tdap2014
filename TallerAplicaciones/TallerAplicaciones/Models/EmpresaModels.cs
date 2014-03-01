using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace TallerAplicaciones.Models
{
    public class AltaEmpresaModel
    {
        [Required]
        [Display(Name = "Nombre de Empresa")]
        public string Nombre { get; set; }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uy.edu.ort.taller.aplicaciones.dominio;
using System.ComponentModel.DataAnnotations;

namespace TallerAplicaciones.Models
{
    public class AltaEmpresaModel
    {
        [Required]
        [Display(Name = "Nombre de Empresa")]
        public string Nombre { get; set; }
    }

    public class EmpresaListModel
    {
        [Display(Name = "Empresas Distribuidoras")]
        public List<EmpresaDistribuidora> Empresas { get; set; }
    }
}
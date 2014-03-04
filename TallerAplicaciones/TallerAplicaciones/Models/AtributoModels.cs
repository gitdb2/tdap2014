using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace TallerAplicaciones.Models
{
    public class AtributoModel
    {
        [Required]
        [Display(Name = "Nombre de Atributo")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Atributo tipo combo")]
        public bool DataCombo { get; set; }

        [Required]
        [Display(Name = "Valores")]
        public List<String> Valores { get; set; }
    }

    public class ListAtributoModel
    {
        [Required]
        [Display(Name = "Identificador de Atributos")]
        public List<Atributo> Atributos { get; set; }
    }
}

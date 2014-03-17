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
        [Display(Name = "Multiseleccion")]
        public bool MultiSeleccion { get; set; }

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

    public class EditAtributoModel
    {
        [Required]
        [Display(Name = "Identificador de Atributo")]
        public int IdAtributo { get; set; }

        [Required]
        [Display(Name = "Atributo activo")]
        public bool Activo { get; set; }

        [Required]
        [Display(Name = "Nombre de Atributo")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Valor unico")]
        public bool DataCombo { get; set; }

        [Required]
        [Display(Name = "Valores")]
        public List<ValorPredefinido> Valores { get; set; }
    }

    public class EditPostAtributoModel
    {
        [Required]
        [Display(Name = "Id de Atributo")]
        public int IdAtributo { get; set; }

        [Required]
        [Display(Name = "Atributo activo")]
        public bool Activo { get; set; }

        [Required]
        [Display(Name = "Nombre de Atributo")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Atributo tipo combo")]
        public bool DataCombo { get; set; }

        [Required]
        [Display(Name = "Lista a Borrar")]
        public List<int> ListaBorrar { get; set; }

        [Required]
        [Display(Name = "Valores Nuevos")]
        public List<String> ValoresNuevos { get; set; }
    }
}

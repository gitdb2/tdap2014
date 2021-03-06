﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace TallerAplicaciones.Models
{
    public class AtributoModel
    {
        [Required]
        [RegularExpression(@"^([a-zA-Z0-9 ]+)$", ErrorMessage = "Ingrese {0} sin simbolos")]
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
        [RegularExpression(@"^([a-zA-Z0-9 ]+)$", ErrorMessage = "Ingrese {0} sin simbolos")]
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
        [RegularExpression(@"^([a-zA-Z0-9 ]+)$", ErrorMessage = "Ingrese {0} sin simbolos")]
        [Display(Name = "Nombre de Atributo")]
        public string Nombre { get; set; }

        [Required]
        [Display(Name = "Atributo tipo combo")]
        public bool DataCombo { get; set; }

        [Required (ErrorMessage = "Debe haber al menos un valor existente")]
        [Display(Name = "Valores del combo")]
        public List<int> ListaBorrar { get; set; }

        
        [Display(Name = "Valores Nuevos")]
        public List<String> ValoresNuevos { get; set; }
    }
}

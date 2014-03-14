using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace TallerAplicaciones.Models
{
    public class ReporteLogEntradasModel
    {

        [Required]
        [Display(Name = "Fecha Inicio")]
        public DateTime FechaInicio { get; set; }

        [Required]
        [Display(Name = "Fecha Fin")]
        public DateTime FechaFin { get; set; }

    }

    public class ReportePedidosModel
    {

        [Required]
        [Display(Name = "Fecha Inicio")]
        public DateTime FechaInicio { get; set; }

        [Required]
        [Display(Name = "Fecha Fin")]
        public DateTime FechaFin { get; set; }

        [Display(Name = "Ejecutivos de Cuenta")]
        public List<Usuario> Ejecutivos { get; set; }

        [Display(Name = "Empresas Distribuidoras")]
        public List<Usuario> Distribuidores { get; set; }

    }

    public class ReporteProductosModel
    {

        [Display(Name = "Productos")]
        public List<CantidadProductoPedido> Productos { get; set; }

    }

    public class ReporteTopProductosModel
    {

        [Display(Name = "Top 5 de Productos")]
        public List<CantidadProductoPedido> TopProductos { get; set; }

    }
    public class ReporteLogsModel
    {
        [Required]
        [Display(Name = "Fecha desde")]
        public DateTime FechaDesde { get; set; }

        [Required]
        [Display(Name = "Fecha hasta")]
        public DateTime FechaHasta { get; set; }

        [Display(Name = "Resultados")]
        public List<LogInfo> Logs { get; set; }

    }



}

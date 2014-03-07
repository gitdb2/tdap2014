﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace TallerAplicaciones.Models
{
    public class PedidoModel
    {
        public int PedidoID { get; set; }

        [Required]
        public bool Aprobado { get; set; }
        [Required]
        public string Descripcion { get; set; }
        [Required]
        public DateTime Fecha { get; set; }
        [Required]
        public bool Activo { get; set; }

        [Required]
        public int EjecutivoId { get; set; }

        [Required]
        public int DistribuidorID { get; set; }

        public List<int> Productos { get; set; }

        public List<int> Cantidades { get; set; }

        public Pedido Pedido { get; set; }


        public EjecutivoDeCuenta EjecutivoDeCuenta { get; set; }
        public CantidadProductoPedido CantidadProductoPedido { get; set; }

    }


    public class PedidoListModel
    {
        public List<Pedido> Pedidos { get; set; } 
    }
}
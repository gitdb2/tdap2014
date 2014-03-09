using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public class CantidadProductoPedido
    {
        public int CantidadProductoPedidoID { get; set; }

        public int Cantidad { get; set; }
        public Producto Producto { get; set; }
        public Pedido Pedido { get; set; }
        //public bool Activo { get; set; }
    }
}

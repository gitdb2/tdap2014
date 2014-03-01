using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public class Pedido
    {
        public int PedidoId { get; set; }
        public DateTime Fecha { get; set; }
        public string Descripcion { get; set; }
        public bool Aprobado { get; set; }
        public List<CantidadProductoPedido> CantidadProductoPedidoList { get; set; }
        public Distribuidor Distribuidor { get; set; }
        public EjecutivoDeCuenta Ejecutivo { get; set; }
    }
}

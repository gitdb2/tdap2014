using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace TallerAplicaciones.Models
{

    public class BaseJson
    {
        public bool Ok { get; set; }
        public string Message { get; set; }
    }

    public class ModificarCantidadPedidoJson : BaseJson
    {
        public int IdPedido { get; set; }
        public int IdCantidadProductoPedido { get; set; }
        public int Cantidad { get; set; }
        public bool Borrado { get; set; }
    }

    public class AddCantidadPedidoJson : ModificarCantidadPedidoJson
    {
        public int IdProducto { get; set; }
        public string NombreProducto { get; set; }
        public string CodigoProducto { get; set; }
    }

    public class RemoverValorAtributoSimple : BaseJson
    {
        public int ProductoId { get; set; }
        public int ValorAtributoId { get; set; }
    }

}
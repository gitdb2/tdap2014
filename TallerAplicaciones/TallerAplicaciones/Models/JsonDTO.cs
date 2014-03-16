using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uy.edu.ort.taller.aplicaciones.dominio;

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

    public class RemoverValorAtributoJson : BaseJson
    {
        public int ProductoId { get; set; }
        public int ValorAtributoId { get; set; }
    }

    public class EditarValorAtributoSimpleJson : BaseJson
    {
        public int ValorAtributoId { get; set; }
        public string NuevoValor { get; set; }
    }

    public class AgregarValorAtributoSimpleJson : BaseJson
    {
        public int ProductoId { get; set; }
        public int AtributoId { get; set; }
        public string NuevoValor { get; set; }
        public int ValorAtributoId { get; set; }
        public string NombreAtributo { get; set; }
    }

    public class EditarValorAtributoComboJson : BaseJson
    {
        public int ProductoId { get; set; }
        public int AtributoId { get; set; }
        public int ValorAtributoId { get; set; }
        public List<int> ListaValorPredefinidoId { get; set; }
    }

    public class AgregarValorAtributoComboJson : BaseJson
    {
        public int ProductoId { get; set; }
        public List<int> ListaValorPredefinidoId { get; set; }
        public int AtributoId { get; set; }
        public int ValorAtributoId { get; set; }
        public string NombreAtributo { get; set; }
    }

    public class ValoresAtributoJson
    {
        public int idAtributo { get; set; }
        public String nombreAtributo { get; set; }
        public bool esCombo { get; set; }
        public bool esMultiselec { get; set; }
        public List<ValoresJson> litaValores { get; set; }
        public bool Ok { get; set; }
        public string Message { get; set; }
    }
}
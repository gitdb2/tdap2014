using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio.DTO
{
    [DataContract]
    public class CantidadProductoPedidoDTO
    {

        [DataMember]
        public int ProductoId { get; set; }

        [DataMember]
        public string ProductoCodigo { get; set; }

        [DataMember]
        public string ProductoNombre { get; set; }

        [DataMember]
        public int CantidadPedida { get; set; }

    }
}

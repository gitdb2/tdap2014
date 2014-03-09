using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio.DTO
{
    [DataContract]
    public class PedidoDTO
    {

        [DataMember]
        public int PedidoId { get; set; }

        [DataMember]
        public DateTime Fecha { get; set; }
        
        [DataMember]
        public string Descripcion { get; set; }
        
        [DataMember]
        public string Ejecutivo { get; set; }

        [DataMember]
        public bool Aprobado { get; set; }

        public PedidoDTO() { }

    }
}

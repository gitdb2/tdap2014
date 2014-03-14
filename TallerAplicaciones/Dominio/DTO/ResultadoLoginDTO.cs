using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio.DTO
{
    [DataContract]
    public class ResultadoLoginDTO
    {

        [DataMember]
        public bool LoginOk { get; set; }

        [DataMember]
        public string Mensaje { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio.DTO
{
    [DataContract]
    public class ArchivoDTO
    {

        [DataMember]
        public int ArchivoId { get; set; }

        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public string Url { get; set; }

    }
}

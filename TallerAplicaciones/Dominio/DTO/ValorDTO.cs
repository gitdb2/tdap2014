using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio.DTO
{
    [DataContract]
    public class ValorDTO
    {

        [DataMember]
        public string ValorString { get; set; }

    }
}

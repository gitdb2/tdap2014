using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio.DTO
{
    [DataContract]
    public class ValorAtributoDTO
    {
        
        [DataMember]
        public string Nombre { get; set; }

        [DataMember]
        public List<ValorDTO> Valores { get; set; }

    }
}

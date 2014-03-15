using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public abstract class ValorAtributo
    {

        public int ValorAtributoID { get; set; }

        public Atributo Atributo { get; set; }
        
    }
}

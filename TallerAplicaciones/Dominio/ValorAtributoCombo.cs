using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public class ValorAtributoCombo : ValorAtributo
    {

        public ValorAtributoCombo()
            : base()
        { 
        
        }

        public List<ValorPredefinido> Valores { get; set; }

        public AtributoCombo Atributo { get; set; }

    }
}

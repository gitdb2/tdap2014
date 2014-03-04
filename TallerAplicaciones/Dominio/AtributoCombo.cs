using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public class AtributoCombo : Atributo
    {

        public bool EsSeleccionMultiple { get; set; }

        public List<ValorPredefinido> Valores { get; set; }
        
        public AtributoCombo()
            : base()
        { 
        
        }

    }
}

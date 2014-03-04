using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public abstract class Atributo
    {

        public int AtributoID { get; set; }

        public string Nombre { get; set; }
        public bool Activo { get; set; }
        public bool DataCombo { get; set; }

        
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public class ValorAtributoSimple : ValorAtributo
    {

        public string Valor { get; set; }

        ValorAtributoSimple()
            : base()
        { 
        
        }

        public ValorAtributoSimple Atributo { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public class AtributoSimple : Atributo
    {
        public ValorAtributo Valor { get; set; }

        public AtributoSimple()
            : base()
        { 
        
        }

        public override void Accept(IVisitorHtmlAtributo visitor)
        {
            visitor.Visit(this);
        }

        public override bool EsMultiseleccion()
        {
            return false;
        }

        public override List<ValoresJson> ListaDeValoresActivosDeAtributo()
        {
            return null;
        }
    }
}

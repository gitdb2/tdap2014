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

        public override void Accept(IVisitorHtmlAtributo visitor)
        {
            visitor.Visit(this);
        }

        public override bool EsMultiseleccion()
        {
            return EsSeleccionMultiple;
        }

        public override List<ValoresJson> ListaDeValoresActivosDeAtributo()
        {
            List<ValoresJson> aRetornar = new List<ValoresJson>();
            if (Valores != null)
            {
                foreach (var valorPredefinido in Valores)
                {
                    if (valorPredefinido.Activo)
                    {
                        aRetornar.Add(new ValoresJson()
                        {
                            id = valorPredefinido.ValorPredefinidoID, 
                            valor = valorPredefinido.Valor 
                        });
                    }
                }
            }
            return aRetornar;
        }

    }
}

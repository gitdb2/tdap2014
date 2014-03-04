using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public class Foto : Archivo
    {

        public Foto()
            : base()
        {

        }


        public override void Accept(IVisitorHtmlFormatter visitor)
        {
            visitor.Visit(this);
        }
    }
}

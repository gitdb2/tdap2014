using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public interface IVisitorHtmlAtributo
    {
        void Visit(AtributoCombo elem);
        void Visit(AtributoSimple elem);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public interface IVisitorHtmlFormatter
    {
        void Visit(Foto elem);
        void Visit(Video elem);
    }
}

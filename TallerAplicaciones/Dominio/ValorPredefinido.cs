using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public class ValorPredefinido
    {
        public ValorPredefinido()
        {
        }
        public int ValorPredefinidoID { get; set; }

        public string Valor { get; set; }

        public bool Activo { get; set; }

        public ValorPredefinido(string valor, bool p)
        {
            this.Valor = valor;
            this.Activo = p;
        }
    }
}

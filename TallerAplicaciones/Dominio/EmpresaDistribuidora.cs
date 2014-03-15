using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public class EmpresaDistribuidora
    {

        public EmpresaDistribuidora()
        {
            Activo = true;
            Ejecutivo = null;
        }

        public int EmpresaDistribuidoraID { get; set; }

        public string Nombre { get; set; }
   
        public EjecutivoDeCuenta Ejecutivo { get; set; }

        public bool Activo { get; set; }

    }
}

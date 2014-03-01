using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public class EmpresaDistribuidora
    {
        public int EmpresaDistribuidoraId { get; set; }
        public string Nombre { get; set; }

        public List<Distribuidor> Usuarios { get; set; }

        public EjecutivoDeCuenta Ejecutivo { get; set; }

    }
}

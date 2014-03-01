using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public class Producto
    {

        public int ProductoID { get; set; }

        public string Nombre { get; set; }
        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        public List<ValorAtributo> ValoresSeleccionados { get; set; }
        public List<Archivo> Archivos { get; set; }

    }
}

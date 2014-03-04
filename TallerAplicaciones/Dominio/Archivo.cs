using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public abstract class Archivo
    {

        public int ArchivoID { get; set; }

        public string PathFileSystem { get; set; }

        public string Nombre { get; set; }
        public string Url { get; set; }
       
        public bool Activo { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public class Producto
    {
        public Producto()
        {
            Activo = true;
        }

        public int ProductoID { get; set; }

        public string Nombre { get; set; }

        [Column(TypeName = "VARCHAR")]
        [StringLength(5)]
        public string Codigo { get; set; }
        public string Descripcion { get; set; }

        public List<ValorAtributo> ValoresSeleccionados { get; set; }
        public List<Archivo> Archivos { get; set; }

        public bool Activo { get; set; }

    }
}

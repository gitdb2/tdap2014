using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace uy.edu.ort.taller.aplicaciones.negocio
{
    public enum ActivoEnum
    {
        [Description("Activo")]
        Activo,
        [Description("Inactivo")]
        Inactivo,
        [Description("Activos e inactivos")]
        Todos
    }
}

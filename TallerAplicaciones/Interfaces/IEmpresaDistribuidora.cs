﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace uy.edu.ort.taller.aplicaciones.interfaces
{
    public interface IEmpresaDistribuidora
    {
        void AltaEmpresa(EmpresaDistribuidora empresa);
        List<EmpresaDistribuidora> ListarEmpresasDistribuidoras();

    }
}

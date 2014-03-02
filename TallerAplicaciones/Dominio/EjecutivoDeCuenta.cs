using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public class EjecutivoDeCuenta : PerfilUsuario
    {

        public EjecutivoDeCuenta()
            : base()
        {

        }

//        public List<EmpresaDistribuidora> Empresas { get; set; }
        public List<Pedido> Pedidos { get; set; }


        public void AsignarEmpresas(List<EmpresaDistribuidora> empresas)
        {
            throw new NotImplementedException();
        }
    }
}

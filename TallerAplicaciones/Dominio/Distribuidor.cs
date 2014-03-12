using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public class Distribuidor : PerfilUsuario
    {

        public Distribuidor()
            : base()
        {

        }
        public override int GetRol()
        {
            return (int)GetRolEnum();
        }

        public override UserRole GetRolEnum()
        {
            return UserRole.Distribuidor;
        }

        public EmpresaDistribuidora Empresa { get; set; }
        public List<Pedido> Pedidos { get; set; }

    }
}

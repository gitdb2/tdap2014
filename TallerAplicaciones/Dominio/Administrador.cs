using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public class Administrador : PerfilUsuario
    {

        public Administrador()
            : base()
        {

        }


        public override int GetRol()
        {
            return (int)GetRolEnum();
        }

        public override UserRole GetRolEnum()
        {
            return UserRole.Administrador;
        }
    }
}

﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio
{
    public class EjecutivoDeCuenta : PerfilUsuario
    {

        public EjecutivoDeCuenta()
            : base()
        {}

        public override int GetRol()
        {
            return (int)GetRolEnum();
        }

        public override UserRole GetRolEnum()
        {
            return UserRole.EjecutivoDeCuenta;
        }

        public List<Pedido> Pedidos { get; set; }
     
    }
}

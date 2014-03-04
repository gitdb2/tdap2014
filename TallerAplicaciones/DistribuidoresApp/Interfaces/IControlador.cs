﻿using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DistribuidoresApp.Temp;

// ReSharper disable once CheckNamespace
namespace DistribuidoresApp
{
    public interface IControlador
    {

        bool Login(string usuario, string password);

        void GuardarLoginActual(LoginUsuario loginActual);
        
        void CambiarEstadoPedido(int idPedido, bool aprobado);

        List<PedidoFake> ListarPedidos();

        List<ProductoFake> ListarProductos();

    }
}
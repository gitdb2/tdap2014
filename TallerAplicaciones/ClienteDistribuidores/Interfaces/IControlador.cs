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

// ReSharper disable once CheckNamespace
namespace uy.edu.ort.taller.aplicaciones.clientedistribuidores
{
    public interface IControlador
    {

        void CerrarSesion();

        bool HayUsuarioLogueado();

        void GuardarLoginActual(LoginUsuario loginActual);

        List<ValorAtributoFake> ObtenerAtributosProducto(int idProductoSeleccionado);

        List<string> ObtenerVideosProducto(int productoFakeId);

        List<string> ObtenerImagenesProducto(int productoFakeId);

    }
}

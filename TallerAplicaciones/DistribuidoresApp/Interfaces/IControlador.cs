using System;
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
namespace uy.edu.ort.taller.aplicaciones.distribuidoresapp
{
    public interface IControlador
    {

        bool Login(string usuario, string password);

    }
}

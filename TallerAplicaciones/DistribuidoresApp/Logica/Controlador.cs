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
    public class Controlador : IControlador
    {

        #region singleton
        private static Controlador instancia = new Controlador();

        private Controlador() { }

        public static Controlador GetInstance()
        {
            return instancia;
        }
        #endregion

        public bool Login(string usuario, string password)
        {
            return true;
        }

    }
}

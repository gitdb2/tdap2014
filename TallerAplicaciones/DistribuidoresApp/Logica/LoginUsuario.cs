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
namespace DistribuidoresApp
{
    public class LoginUsuario
    {

        private string _usuario;
        private string _password;

        public string Usuario
        {
            get { return _usuario; }
            set
            {
                if (value == null || value.Trim().Equals(""))
                {
                    throw new ArgumentException("Campo obligatorio");
                }
                _usuario = value;
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (value == null || value.Trim().Equals(""))
                {
                    throw new ArgumentException("Campo obligatorio");
                }
                _password = value;
            }
        }

        public LoginUsuario()
        {
            
        }

    }
}

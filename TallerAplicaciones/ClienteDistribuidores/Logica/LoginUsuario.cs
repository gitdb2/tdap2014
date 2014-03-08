using System;
using System.ComponentModel.DataAnnotations;
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
    public class LoginUsuario
    {

        private string _usuario;
        private string _password;

        public string Usuario
        {
            get { return _usuario; }
            set
            {
                if (_usuario != value)
                {
                    if (value == null || value.Trim().Equals(""))
                    {
                        throw new ValidationException("Campo obligatorio");
                    }
                    _usuario = value;
                }
            }
        }

        public string Password
        {
            get { return _password; }
            set
            {
                if (_password != value)
                {
                    if (value == null || value.Trim().Equals(""))
                    {
                        throw new ValidationException("Campo obligatorio");
                    }
                    _password = value;
                }
            }
        }

        public LoginUsuario()
        {
            
        }

    }
}

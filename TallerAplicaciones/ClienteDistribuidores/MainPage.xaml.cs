using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using uy.edu.ort.taller.aplicaciones.clientedistribuidores.ApiDistribuidores;

namespace uy.edu.ort.taller.aplicaciones.clientedistribuidores
{
    public partial class MainPage : Page
    {

        public LoginUsuario LoginActual { get; set; }

        public MainPage()
        {
            InitializeComponent();
            LoginActual = new LoginUsuario();
            LayoutRoot.DataContext = LoginActual;
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            BusyIndicatorMainPage.IsBusy = true;
            var api = new ApiDistribuidoresClient();
            api.LoginCompleted += new EventHandler<LoginCompletedEventArgs>(LoginCompleted);
            api.LoginAsync(TxtBoxUsuario.Text, TxtBoxPassword.Password);
        }

        private void LoginCompleted(object sender, LoginCompletedEventArgs e)
        {
            try
            {
                var mensaje = "";
                var error = false;
                if (e.Result != null)
                {
                    if (e.Result.LoginOk)
                    {
                        Controlador.GetInstance().GuardarLoginActual(LoginActual);
                        this.Content = new DataDistribuidorTabs();
                    }
                    else
                    {
                        mensaje = e.Result.Mensaje;
                        error = true;
                    }
                }
                else
                {
                    mensaje = "Error desdonocido, contacte al Administrador";
                    error = true;
                }
                if (error)
                {
                    ValidationSummaryLogin.Errors.Clear();
                    ValidationSummaryLogin.Errors.Add(new ValidationSummaryItem(mensaje));    
                }
            }
            catch (Exception err)
            {
                new ErrorWindow(err).Show();
            }
            finally
            {
                BusyIndicatorMainPage.IsBusy = false;
            }
        }

    }
}
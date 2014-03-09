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

        private void BtnLogin_Click2(object sender, RoutedEventArgs e)
        {
            IControlador iControlador = Controlador.GetInstance();
            var loginResult = iControlador.Login(LoginActual.Usuario, LoginActual.Password);
            if (loginResult)
            {
                iControlador.GuardarLoginActual(LoginActual);
                var proximaPagina = new DataDistribuidorTabs();
                this.Content = proximaPagina;
            }
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            BusyIndicatorMainPage.IsBusy = true;
            ApiDistribuidoresClient api = new ApiDistribuidoresClient();
            api.LoginCompleted += new EventHandler<LoginCompletedEventArgs>(LoginCompleted);
            api.LoginAsync(TxtBoxUsuario.Text, TxtBoxPassword.Password);
        }

        private void LoginCompleted(object sender, LoginCompletedEventArgs e)
        {
            try
            {
                IControlador iControlador = Controlador.GetInstance();
                if (e.Result)
                {
                    iControlador.GuardarLoginActual(LoginActual);
                    this.Content = new DataDistribuidorTabs();
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
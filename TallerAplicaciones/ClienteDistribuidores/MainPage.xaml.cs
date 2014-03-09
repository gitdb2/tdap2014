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
using uy.edu.ort.taller.aplicaciones.clientedistribuidores.ServiceReference1;

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
            /*
            ApiClient proxy = new ApiClient();
            proxy.DoWorkCompleted += new EventHandler<DoWorkCompletedEventArgs>(DoWorkCompleted);             proxy.DoWorkAsync(true);             */        }

        private void DoWorkCompleted(object sender, DoWorkCompletedEventArgs e)
        {
            /*
            try
            {
                TxtBoxUsuario.Text = e.Result.ToString();
            }
            catch (Exception err)
            {
                TxtBoxUsuario.Text = "Error contacting web service";
            }
             */
        }

        private void ProcesarResultadoLogin()
        {
            var loginResult = true;
            IControlador iControlador = Controlador.GetInstance();
            if (loginResult)
            {
                iControlador.GuardarLoginActual(LoginActual);
                var proximaPagina = new DataDistribuidorTabs();
                this.Content = proximaPagina;
            }
        }

    }
}
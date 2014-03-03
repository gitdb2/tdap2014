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

namespace DistribuidoresApp
{
    public partial class Home : Page
    {

        public LoginUsuario LoginActual { get; set; }

        public Home()
        {
            InitializeComponent();
            LoginActual = new LoginUsuario();
            LayoutRoot.DataContext = LoginActual;
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void BtnLogin_Click(object sender, RoutedEventArgs e)
        {
            IControlador iControlador = Controlador.GetInstance();
            var resLogin = iControlador.Login(LoginActual.Usuario, LoginActual.Password);
        }

    }
}
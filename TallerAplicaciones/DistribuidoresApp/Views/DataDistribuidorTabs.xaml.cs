using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using DistribuidoresApp.Temp;

namespace DistribuidoresApp.Views
{
    public partial class DataDistribuidorTabs : Page
    {
        public List<PedidoFake> Pedidos;
        public List<ProductoFake> Productos;
        public List<ValorAtributoFake> Atributos;

        public DataDistribuidorTabs()
        {
            InitializeComponent();
            RefrescarPedidos();
            RefrescarProductos();
            RefrescarArbolAtributos();
        }

        private void RefrescarPedidos()
        {
            IControlador iControlador = Controlador.GetInstance();
            Pedidos = iControlador.ListarPedidos();
            DataGridPedidos.ItemsSource = Pedidos;
        }

        private void RefrescarProductos()
        {
            IControlador iControlador = Controlador.GetInstance();
            Productos = iControlador.ListarProductos();
            DataGridProductos.ItemsSource = Productos;
            if (Productos.Any())
            {
                DataGridProductos.SelectedIndex = 0;
            }
        }

        private void RefrescarArbolAtributos()
        {
            var productoFakeSeleccionado = (ProductoFake) DataGridProductos.SelectedItem;
            if (productoFakeSeleccionado != null)
            {
                IControlador iControlador = Controlador.GetInstance();
                Atributos = iControlador.ObtenerAtributos(productoFakeSeleccionado.ProductoFakeId);
                TreeViewCamposVariables.ItemsSource = Atributos;
            }
        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        private void AprobadoCambiarEstado_Click(object sender, RoutedEventArgs e)
        {
            var chkBox = sender as CheckBox;
            var pedidoFakeSeleccionado = DataGridPedidos.SelectedItem as PedidoFake;
            if (chkBox != null && pedidoFakeSeleccionado != null)
            {
                var aprobado = chkBox.IsChecked != null && (bool)chkBox.IsChecked;
                IControlador iControlador = Controlador.GetInstance();
                iControlador.CambiarEstadoPedido(pedidoFakeSeleccionado.PedidoFakeId, aprobado);    
            }
        }

        private void DataGridProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefrescarArbolAtributos();
        }
    }
}

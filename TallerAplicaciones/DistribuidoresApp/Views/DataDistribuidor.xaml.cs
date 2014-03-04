﻿using System;
using System.Collections.Generic;
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
    public partial class DataDistribuidor : Page
    {
        public List<PedidoFake> Pedidos;
        public List<ProductoFake> Productos;

        public DataDistribuidor()
        {
            InitializeComponent();
            IControlador iControlador = Controlador.GetInstance();

            Pedidos = iControlador.ListarPedidos();
            DataGridPedidos.ItemsSource = Pedidos;

            Productos = iControlador.ListarProductos();
            DataGridProductos.ItemsSource = Productos;
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
    }
}

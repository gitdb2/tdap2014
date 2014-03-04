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
            Pedidos = new List<PedidoFake>
            {
                new PedidoFake() {PedidoFakeId = 1, Descripcion = "Pedido1", Aprobado = true},
                new PedidoFake() {PedidoFakeId = 2, Descripcion = "Pedido2", Aprobado = false},
                new PedidoFake() {PedidoFakeId = 3, Descripcion = "Pedido3", Aprobado = false},
                new PedidoFake() {PedidoFakeId = 4, Descripcion = "Pedido4", Aprobado = true}
            };
            DataGridPedidos.ItemsSource = Pedidos;

            Productos = new List<ProductoFake>
            {
                new ProductoFake() {ProductoFakeId = 1, Codigo = "AAAAA", Descripcion = "Producto1"},
                new ProductoFake() {ProductoFakeId = 2, Codigo = "BBBBB", Descripcion = "Producto2"},
                new ProductoFake() {ProductoFakeId = 3, Codigo = "CCCCC", Descripcion = "Producto3"},
                new ProductoFake() {ProductoFakeId = 4, Codigo = "DDDDD", Descripcion = "Producto4"}
            };
            DataGridProductos.ItemsSource = Productos;

        }

        // Executes when the user navigates to this page.
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

    }
}

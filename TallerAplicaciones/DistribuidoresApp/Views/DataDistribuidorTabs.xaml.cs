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
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;
using DistribuidoresApp.Temp;

namespace DistribuidoresApp.Views
{
    public partial class DataDistribuidorTabs : Page
    {
        public List<PedidoFake> Pedidos;

        public List<ProductoFake> Productos;
        public List<ValorAtributoFake> AtributosProducto;
        public Dictionary<string, int> PlayListVideosProducto { get; set; }
        public Dictionary<string, int> PlayListImagenesProducto { get; set; }

        public DataDistribuidorTabs()
        {
            InitializeComponent();
            RefrescarPedidos();
            RefrescarProductos();
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
            DataGridProductos.SelectedIndex = -1;
        }

        private void RefrescarArbolAtributos()
        {
            var productoFakeSeleccionado = (ProductoFake) DataGridProductos.SelectedItem;
            if (productoFakeSeleccionado != null)
            {
                IControlador iControlador = Controlador.GetInstance();
                AtributosProducto = iControlador.ObtenerAtributos(productoFakeSeleccionado.ProductoFakeId);
                TreeViewCamposVariables.ItemsSource = AtributosProducto;
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
            RefrescarVideosProducto();
            RefrescarImagenesProducto();
        }

        private void RefrescarImagenesProducto()
        {
            ImagenesProducto.Source = new BitmapImage(new Uri("http://upload.wikimedia.org/wikipedia/commons/1/1a/Bachalpseeflowers.jpg"));
        }

        private void RefrescarVideosProducto()
        {
            var productoFakeSeleccionado = (ProductoFake) DataGridProductos.SelectedItem;
            if (productoFakeSeleccionado != null)
            {
                IControlador iControlador = Controlador.GetInstance();
                var videosProducto = iControlador.ObtenerVideos(productoFakeSeleccionado.ProductoFakeId);
                VideosProducto.AutoPlay = true;
                PopularPlayList(videosProducto);
                SetearSiguienteVideo();
            }
        }

        private void SetearSiguienteVideo()
        {
            var siguienteVideo = VideoMenosMostrado();
            if (siguienteVideo != null)
            {
                PlayListVideosProducto[siguienteVideo]++;
                VideosProducto.Source = new Uri(siguienteVideo);    
            }
        }

        private string VideoMenosMostrado()
        {
            int minVeces = Int16.MaxValue;
            string videoMinVeces = null;
            foreach (var par in PlayListVideosProducto)
            {
                if (par.Value <= minVeces)
                {
                    minVeces = par.Value;
                    videoMinVeces = par.Key;
                }
            }
            return videoMinVeces;
        }

        private void PopularPlayList(List<string> videos)
        {
            PlayListVideosProducto = new Dictionary<string, int>();
            foreach (var video in videos)
            {
                PlayListVideosProducto.Add(video, 0);
            }        
        }

        private void VideosProducto_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            SetearSiguienteVideo();
        }

    }
}

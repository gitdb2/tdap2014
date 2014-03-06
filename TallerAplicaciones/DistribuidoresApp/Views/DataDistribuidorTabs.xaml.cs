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
        public Dictionary<string, int> PlayList { get; set; }

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
            RefrescarVideosProducto();
        }

        private void RefrescarVideosProducto()
        {
            var productoFakeSeleccionado = (ProductoFake) DataGridProductos.SelectedItem;
            if (productoFakeSeleccionado != null)
            {
                IControlador iControlador = Controlador.GetInstance();
                var videosProducto = iControlador.ObtenerVideos(productoFakeSeleccionado.ProductoFakeId);
                Media.AutoPlay = true;
                PopularPlayList(videosProducto);
                SetearSiguienteVideo();
            }
        }

        private void SetearSiguienteVideo()
        {
            var siguienteVideo = VideoMenosMostrado();
            if (siguienteVideo != null)
            {
                PlayList[siguienteVideo]++;
                Media.Source = new Uri(siguienteVideo);    
            }
        }

        private string VideoMenosMostrado()
        {
            int minVeces = Int16.MaxValue;
            string videoMinVeces = null;
            foreach (var par in PlayList)
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
            PlayList = new Dictionary<string, int>();
            foreach (var video in videos)
            {
                PlayList.Add(video, 0);
            }        
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Media.Stop();
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            Media.Pause();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Media.Play();
        }

        private void Media_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            SetearSiguienteVideo();
        }

    }
}

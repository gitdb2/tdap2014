using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Navigation;
using System.Windows.Threading;

namespace uy.edu.ort.taller.aplicaciones.clientedistribuidores
{
    public partial class DataDistribuidorTabs : Page
    {
        public List<PedidoFake> Pedidos;
        public List<ProductoFake> Productos;
        public List<ValorAtributoFake> AtributosProducto;
        public Dictionary<string, int> PlayListVideosProducto { get; set; }
        public Dictionary<string, int> PlayListImagenesProducto { get; set; }
        private DispatcherTimer _timer;

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
                AtributosProducto = iControlador.ObtenerAtributosProducto(productoFakeSeleccionado.ProductoFakeId);
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
            var productoFakeSeleccionado = (ProductoFake)DataGridProductos.SelectedItem;
            if (productoFakeSeleccionado != null)
            {
                IControlador iControlador = Controlador.GetInstance();
                var imagenesProducto = iControlador.ObtenerImagenesProducto(productoFakeSeleccionado.ProductoFakeId);
                PlayListImagenesProducto = GenerarPlayList(imagenesProducto);
                IniciarSlideShowImagenesProducto();
            }
        }

        private void RefrescarVideosProducto()
        {
            var productoFakeSeleccionado = (ProductoFake)DataGridProductos.SelectedItem;
            if (productoFakeSeleccionado != null)
            {
                IControlador iControlador = Controlador.GetInstance();
                var videosProducto = iControlador.ObtenerVideosProducto(productoFakeSeleccionado.ProductoFakeId);
                VideosProducto.AutoPlay = true;
                PlayListVideosProducto = GenerarPlayList(videosProducto);
                SetearSiguienteVideo();
            }
        }

        private Dictionary<string, int> GenerarPlayList(List<string> origen)
        {
            var playList = new Dictionary<string, int>();
            if (origen.Any())
            {
                foreach (var item in origen)
                {
                    playList.Add(item, 0);
                }
            }
            return playList;
        }

        private void IniciarSlideShowImagenesProducto()
        {
            SetearSiguienteImagen();
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(3000);
            _timer.Tick += (sender, e) => SetearSiguienteImagen();
            _timer.Start();
        }

        private void DetenerSlideShowImagenesProducto()
        {
            if (_timer != null) 
                _timer.Stop();
        }

        private void SetearSiguienteImagen()
        {
            var siguienteImagen = ElementoPlayListMenosMostrado(PlayListImagenesProducto);
            if (siguienteImagen != null)
            {
                PlayListImagenesProducto[siguienteImagen]++;
                ImagenesProducto.Source = new BitmapImage(new Uri(siguienteImagen));
            }
        }

        private void SetearSiguienteVideo()
        {
            var siguienteVideo = ElementoPlayListMenosMostrado(PlayListVideosProducto);
            if (siguienteVideo != null)
            {
                PlayListVideosProducto[siguienteVideo]++;
                VideosProducto.Source = new Uri(siguienteVideo);    
            }
        }

        private string ElementoPlayListMenosMostrado(Dictionary<string, int> playList)
        {
            int minVeces = Int16.MaxValue;
            string elementoMinVecesMostrado = null;
            if (playList != null)
            {
                foreach (var par in playList)
                {
                    if (par.Value <= minVeces)
                    {
                        minVeces = par.Value;
                        elementoMinVecesMostrado = par.Key;
                    }
                }
            }
            return elementoMinVecesMostrado;
        }

        private void VideosProducto_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            SetearSiguienteVideo();
        }

        private void BtnCerrarSesion_OnClick(object sender, RoutedEventArgs e)
        {
            DetenerSlideShowImagenesProducto();
            IControlador iControlador = Controlador.GetInstance();
            iControlador.CerrarSesion();
            Content = new MainPage();
        }

    }
}

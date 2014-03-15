using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Windows;
using System.Windows.Browser;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using System.Windows.Threading;
using uy.edu.ort.taller.aplicaciones.clientedistribuidores.ApiDistribuidores;
using uy.edu.ort.taller.aplicaciones.clientedistribuidores.Logica;

namespace uy.edu.ort.taller.aplicaciones.clientedistribuidores
{
    public partial class DataDistribuidorTabs : Page
    {

        public PlayList PlayListVideosProducto { get; set; }
        public PlayList PlayListImagenesProducto { get; set; }
        private DispatcherTimer _timer;

        public DataDistribuidorTabs()
        {
            InitializeComponent();
            RefrescarPedidosAsync();
            RefrescarProductosAsync();
        }

        #region pedidos
        private void RefrescarPedidosAsync()
        {
            BusyIndicatorPedidosTab.IsBusy = true;
            var api = new ApiDistribuidoresClient();
            api.ListarPedidosDistribuidorCompleted += new EventHandler<ListarPedidosDistribuidorCompletedEventArgs>(ListarPedidosDistribuidorCompleted);
            api.ListarPedidosDistribuidorAsync(Controlador.GetInstance().LoginActual.Usuario);
        }

        private void ListarPedidosDistribuidorCompleted(object sender, ListarPedidosDistribuidorCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null && e.Result.Any())
                {
                    DataGridPedidos.ItemsSource = e.Result;
                }
            }
            catch (Exception err)
            {
                new ErrorWindow(err).Show();
            }
            finally
            {
                BusyIndicatorPedidosTab.IsBusy = false;
            }
        }

        private void AprobadoCambiarEstado_Click(object sender, RoutedEventArgs e)
        {
            var chkBox = sender as CheckBox;
            var pedidoSeleccionado = DataGridPedidos.SelectedItem as PedidoDTO;
            if (chkBox != null && pedidoSeleccionado != null)
            {
                var aprobado = chkBox.IsChecked != null && (bool)chkBox.IsChecked;
                CambiarEstadoPedido(pedidoSeleccionado, aprobado);
            }
        }

        private void CambiarEstadoPedido(PedidoDTO pedidoSeleccionado, bool nuevoEstado)
        {
            BusyIndicatorPedidosTab.IsBusy = true;
            var api = new ApiDistribuidoresClient();
            api.CambiarEstadoPedidoCompleted += new EventHandler<CambiarEstadoPedidoCompletedEventArgs>(CambiarEstadoPedidoCompleted);
            api.CambiarEstadoPedidoAsync(pedidoSeleccionado.PedidoId, nuevoEstado);
        }

        private void CambiarEstadoPedidoCompleted(object sender, CambiarEstadoPedidoCompletedEventArgs e)
        {
            try
            {
                if (!e.Result)
                {
                    new ErrorWindow(new Exception("Ocurrio un error al cambiar el estado del Pedido")).Show();
                }
            }
            catch (Exception err)
            {
                new ErrorWindow(err).Show();
            }
            finally
            {
                BusyIndicatorPedidosTab.IsBusy = false;
            }
        }

        private void DataGridPedidos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            var pedidoSeleccionado = DataGridPedidos.SelectedItem as PedidoDTO;
            if (pedidoSeleccionado != null)
            {
                BusyIndicatorPedidosTab.IsBusy = true;
                var api = new ApiDistribuidoresClient();
                api.ListarProductosPedidoCompleted += new EventHandler<ListarProductosPedidoCompletedEventArgs>(ListarProductosPedidoCompleted);
                api.ListarProductosPedidoAsync(pedidoSeleccionado.PedidoId);
            }
        }

        private void ListarProductosPedidoCompleted(object sender, ListarProductosPedidoCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null && e.Result.Any())
                {
                    DataGridProductosPedido.ItemsSource = e.Result;
                }
            }
            catch (Exception err)
            {
                new ErrorWindow(err).Show();
            }
            finally
            {
                BusyIndicatorPedidosTab.IsBusy = false;
            }
        }
        #endregion

        #region productos
        private void RefrescarProductosAsync()
        {
            BusyIndicatorPedidosTab.IsBusy = true;
            var api = new ApiDistribuidoresClient();
            api.ListarProductosCompleted += new EventHandler<ListarProductosCompletedEventArgs>(ListarProductosCompleted);
            api.ListarProductosAsync();
        }

        private void ListarProductosCompleted(object sender, ListarProductosCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null && e.Result.Any())
                {
                    DataGridProductos.ItemsSource = e.Result;
                    DataGridProductos.SelectedIndex = -1;
                }
            }
            catch (Exception err)
            {
                new ErrorWindow(err).Show();
            }
            finally
            {
                BusyIndicatorPedidosTab.IsBusy = false;
            }
        }

        private void DataGridProductos_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            RefrescarArbolAtributosAsync();
            RefrescarImagenesProductoAsync();
            RefrescarVideosProductoAsync();
        }
        #endregion

        #region atributos producto
        private void RefrescarArbolAtributosAsync()
        {
            var productoSeleccionado = (ProductoDTO)DataGridProductos.SelectedItem;
            if (productoSeleccionado != null)
            {
                var api = new ApiDistribuidoresClient();
                api.ListarAtributosProductoCompleted += new EventHandler<ListarAtributosProductoCompletedEventArgs>(ListarAtributosProductoCompleted);
                api.ListarAtributosProductoAsync(productoSeleccionado.ProductoId);
                BusyIndicatorPedidosTab.IsBusy = true;
            }
        }

        private void ListarAtributosProductoCompleted(object sender, ListarAtributosProductoCompletedEventArgs e)
        {
            try
            {
                TreeViewCamposVariables.ItemsSource = null;
                if (e.Result != null && e.Result.Any())
                {
                    TreeViewCamposVariables.ItemsSource = e.Result;
                }
            }
            catch (Exception err)
            {
                new ErrorWindow(err).Show();
            }
            finally
            {
                BusyIndicatorPedidosTab.IsBusy = false;
            }
        }
        #endregion

        #region imagenes producto
        private void RefrescarImagenesProductoAsync()
        {
            DetenerSlideShowImagenesProducto();
            var productoSeleccionado = (ProductoDTO)DataGridProductos.SelectedItem;
            if (productoSeleccionado != null)
            {
                var api = new ApiDistribuidoresClient();
                api.ListarImagenesProductoCompleted += new EventHandler<ListarImagenesProductoCompletedEventArgs>(ListarImagenesProductoCompleted);
                api.ListarImagenesProductoAsync(productoSeleccionado.ProductoId);
                BusyIndicatorPedidosTab.IsBusy = true;
            }
        }

        private void ListarImagenesProductoCompleted(object sender, ListarImagenesProductoCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null && e.Result.Any())
                {
                    PlayListImagenesProducto = new PlayList();
                    PlayListImagenesProducto.CargarPlayList(e.Result);
                    IniciarSlideShowImagenesProducto();
                }
            }
            catch (Exception err)
            {
                new ErrorWindow(err).Show();
            }
            finally
            {
                BusyIndicatorPedidosTab.IsBusy = false;
            }
        }

        private void IniciarSlideShowImagenesProducto()
        {
            DetenerSlideShowImagenesProducto();
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
            ImagenesProducto.Source = null;
        }

        private void SetearSiguienteImagen()
        {
            var siguienteImagen = PlayListImagenesProducto.SiguienteElemento();
            if (siguienteImagen != null)
            {
                ImagenesProducto.Source = new BitmapImage(new Uri(siguienteImagen.Url));
            }
        }
        #endregion

        #region videos producto
        private void RefrescarVideosProductoAsync()
        {
            DetenerSlideShowVideosProducto();
            var productoSeleccionado = (ProductoDTO)DataGridProductos.SelectedItem;
            if (productoSeleccionado != null)
            {
                var api = new ApiDistribuidoresClient();
                api.ListarVideosProductoCompleted += new EventHandler<ListarVideosProductoCompletedEventArgs>(ListarVideosProductoCompleted);
                api.ListarVideosProductoAsync(productoSeleccionado.ProductoId);
                BusyIndicatorPedidosTab.IsBusy = true;
            }
        }

        private void ListarVideosProductoCompleted(object sender, ListarVideosProductoCompletedEventArgs e)
        {
            try
            {
                if (e.Result != null && e.Result.Any())
                {
                    PlayListVideosProducto = new PlayList();
                    PlayListVideosProducto.CargarPlayList(e.Result);
                    SetearSiguienteVideo();
                    VideosProducto.AutoPlay = true;
                }
            }
            catch (Exception err)
            {
                new ErrorWindow(err).Show();
            }
            finally
            {
                BusyIndicatorPedidosTab.IsBusy = false;
            }
        }

        private void DetenerSlideShowVideosProducto()
        {
            VideosProducto.Stop();
            VideosProducto.Source = null;
        }

        private void SetearSiguienteVideo()
        {
            var siguienteVideo = PlayListVideosProducto.SiguienteElemento();
            if (siguienteVideo != null)
            {
                VideosProducto.Source = new Uri(siguienteVideo.Url);    
            }
        }

        private void VideosProducto_OnMediaEnded(object sender, RoutedEventArgs e)
        {
            SetearSiguienteVideo();
        }
        #endregion

        #region sesion
        private void BtnCerrarSesion_OnClick(object sender, RoutedEventArgs e)
        {
            DetenerSlideShowImagenesProducto();
            IControlador iControlador = Controlador.GetInstance();
            iControlador.CerrarSesion();
            Content = new MainPage();
        }
        #endregion

    }
}




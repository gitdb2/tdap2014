using System;
using System.Collections.Generic;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using DistribuidoresApp.Temp;

// ReSharper disable once CheckNamespace
namespace DistribuidoresApp
{
    public class Controlador : IControlador
    {

        #region singleton
        private static Controlador instancia = new Controlador();

        private Controlador() { }

        public static Controlador GetInstance()
        {
            return instancia;
        }
        #endregion

        public LoginUsuario LoginActual { get; set; }

        //TODO
        public bool Login(string usuario, string password)
        {
            return true;
        }

        public void GuardarLoginActual(LoginUsuario loginActual)
        {
            this.LoginActual = loginActual;
        }

        //TODO
        public void CambiarEstadoPedido(int idPedido, bool aprobado)
        {
            
        }

        //TODO
        public List<PedidoFake> ListarPedidos()
        {
            var pedidos = new List<PedidoFake>
            {
                new PedidoFake() {PedidoFakeId = 1, Descripcion = "Pedido1", Aprobado = true},
                new PedidoFake() {PedidoFakeId = 2, Descripcion = "Pedido2", Aprobado = false},
                new PedidoFake() {PedidoFakeId = 3, Descripcion = "Pedido3", Aprobado = false},
                new PedidoFake() {PedidoFakeId = 4, Descripcion = "Pedido4", Aprobado = true}
            };
            return pedidos;
        }

        //TODO
        public List<Temp.ProductoFake> ListarProductos()
        {
            var productos = new List<ProductoFake>
            {
                new ProductoFake() {ProductoFakeId = 1, Codigo = "AAAAA", Descripcion = "Producto1"},
                new ProductoFake() {ProductoFakeId = 2, Codigo = "BBBBB", Descripcion = "Producto2"},
                new ProductoFake() {ProductoFakeId = 3, Codigo = "CCCCC", Descripcion = "Producto3"},
                new ProductoFake() {ProductoFakeId = 4, Codigo = "DDDDD", Descripcion = "Producto4"}
            };
            return productos;
        }

        //TODO
        public List<ValorAtributoFake> ObtenerAtributos(int idProductoSeleccionado)
        {
            var res = new List<ValorAtributoFake>();

            var valor1 = new ValorAtributoFake
            {
                Nombre = "valor1",
                Valores = new List<Valor>
                {
                    new Valor() {ValorString = "aaaaa"},
                    new Valor() {ValorString = "bbbbb"},
                    new Valor() {ValorString = "ccccc"}
                }
            };

            var valor2 = new ValorAtributoFake
            {
                Nombre = "valor2",
                Valores = new List<Valor>
                {
                    new Valor() {ValorString = "ddddd"}
                }
            };

            var valor3 = new ValorAtributoFake
            {
                Nombre = "valor3",
                Valores = new List<Valor>
                {
                    new Valor() {ValorString = "eeeee"},
                    new Valor() {ValorString = "fffff"}
                }
            };

            res.Add(valor1);
            res.Add(valor2);
            res.Add(valor3);

            return res;
        }

        //TODO
        public List<string> ObtenerVideos(int idProductoSeleccionado)
        {
            return new List<string>()
            {
                "http://download.wavetlan.com/SVV/Media/HTTP/WMV/ConvertedFiles/Blaze/Blaze_test1_WMV-WMV9MP_CBR_320x240_AR4to3_15fps_512kbps_WMA9.2L2_32kbps_44100Hz_Mono.wmv",
                "http://download.wavetlan.com/SVV/Media/HTTP/WMV/Blaze2/Blaze_test3_WMV9MP(VC1)_CBR_64kbps_480x320_30fps_WMA3_64kbps_Stereo_48000Hz.wmv",
                "http://download.wavetlan.com/SVV/Media/HTTP/WMV/Blaze2/Blaze_test5_WMV9MP(VC1)_CBR_256kbps_480x320_25fps_WMA2_48kbps_Stereo_32000Hz.wmv",
                "http://mschannel9.vo.msecnd.net/o9/mix/09/wmv/key01.wmv"
            };
        }

    }
}


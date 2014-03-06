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
        public List<ValorAtributoFake> ObtenerAtributosProducto(int idProductoSeleccionado)
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
        public List<string> ObtenerVideosProducto(int idProductoSeleccionado)
        {
            return new List<string>()
            {
                "http://www.2atoms.com/video/haha/fighting_cats4.wmv",
                "http://www.2atoms.com/video/haha/head_rush4.wmv",
                "http://www.2atoms.com/video/haha/smelly_monkey4.wmv"
            };
        }

        public List<string> ObtenerImagenesProducto(int productoFakeId)
        {
            return new List<string>()
            {
                "http://3.bp.blogspot.com/-5A5xpicPF5g/T8srguvp3TI/AAAAAAAAEPs/bLuFIK0gDss/s400/nature-wallpaper-23.jpg"
            };
        }

    }
}


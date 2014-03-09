using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using uy.edu.ort.taller.aplicaciones.dominio.Exceptions;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace uy.edu.ort.taller.aplicaciones.negocio
{
    public class ManejadorPedido : IPedido
    {
        #region singleton
        private static ManejadorPedido instance = new ManejadorPedido();

        private ManejadorPedido() { }

        public static ManejadorPedido GetInstance()
        {
            return instance;
        }
        #endregion



        public List<Pedido> ListarPedidos()
        {

            using (var db = new Persistencia())
            {




                return db.Pedidos
                    .Include(p => p.Ejecutivo)
                    .Include(p2 => p2.Ejecutivo.Usuario)
                    .Include(p3 => p3.Distribuidor)
                    .Include(p4 => p4.Distribuidor.Usuario)
                    .Include(p5 => p5.Distribuidor.Empresa)
                    .Include(p6 => p6.CantidadProductoPedidoList)
                    
                    //.Include("CantidadProductoPedidoList")
                    //.Include("Distribuidor")

                    //.Include("Empresa")


                    .ToList();
            }

        }

        public void Alta(Pedido pedido, int idDistribuidor, int idEjecutivo,
                                     List<int> productos, List<int> cantidades)
        {
            using (var db = new Persistencia())
            {
                //var distrib = ManejadorPerfilUsuario.GetInstance().FindDistribuidor(idDistribuidor);
                //db.PerfilesUsuario.Attach(distrib);

                //var ejecutivo = ManejadorPerfilUsuario.GetInstance().FindEjecutivo(idEjecutivo);
                //  db.PerfilesUsuario.Attach(ejecutivo);
                pedido.Distribuidor = db.PerfilesUsuario.OfType<Distribuidor>()
                    .Include(p => p.Empresa)
                    .Include(p2 => p2.Usuario)
                    .SingleOrDefault(p => p.PerfilUsuarioID == idDistribuidor);
                

                pedido.Ejecutivo = db.PerfilesUsuario.OfType<EjecutivoDeCuenta>()
                    .Include(p => p.Usuario)
                    .SingleOrDefault(p2 => p2.PerfilUsuarioID == idEjecutivo);

                
                pedido.CantidadProductoPedidoList = new List<CantidadProductoPedido>();

                for (int i = 0; i < productos.Count; i++)
                {
                    var prodId = productos[i];
                    var cant = cantidades[i];

                    var producto = ManejadorProducto.GetInstance().GetProducto(prodId);
                    db.Productos.Attach(producto);

                    var cantidadProducto = new CantidadProductoPedido()
                    {
                        Cantidad = cant,
                        Pedido = pedido,
                        Producto = producto
                    };
                    pedido.CantidadProductoPedidoList.Add(cantidadProducto);
                }

                db.Pedidos.Add(pedido);
                db.SaveChanges();
            }

        }

        public Pedido GetPedido(int idPedido)
        {
            using (var db = new Persistencia())
            {
                Pedido pedido = db.Pedidos
                    .Include(p => p.Ejecutivo)
                    .Include(p2 => p2.Distribuidor)
                    .Include(p3 => p3.Distribuidor.Empresa)
                    .Include(p4 => p4.CantidadProductoPedidoList.Select(t => t.Producto))
                    .SingleOrDefault(p => p.PedidoID == idPedido);

                

                return pedido;
            }
        }

        public void Baja(int idPedido)
        {
            using (var db = new Persistencia())
            {
                Pedido pedido = db.Pedidos.SingleOrDefault(p => p.PedidoID == idPedido);
                if (pedido != null)
                {
                    pedido.Activo = false;
                    db.SaveChanges();
                }
                else
                {
                    throw new CustomException("No se pudo elimianr el pedido"){Key = "PedidoID"};
                }
            }
        }
    }
}

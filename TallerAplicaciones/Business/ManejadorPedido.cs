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
                    .Include(p => p.Ejecutivo.Usuario)
                    .Include(p => p.Distribuidor)
                    .Include(p => p.Distribuidor.Usuario)
                    .Include(p => p.Distribuidor.Empresa)
                    .Include(p => p.CantidadProductoPedidoList)
                    
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
                    .Include(p => p.Usuario)
                    .SingleOrDefault(p => p.PerfilUsuarioID == idDistribuidor);
                

                pedido.Ejecutivo = db.PerfilesUsuario.OfType<EjecutivoDeCuenta>()
                    .Include(p => p.Usuario)
                    .SingleOrDefault(p => p.PerfilUsuarioID == idEjecutivo);

                
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
    }
}

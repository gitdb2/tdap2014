using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using uy.edu.ort.taller.aplicaciones.dominio.DTO;
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
                    .ToList();
            }

        }

        public void Alta(Pedido pedido, int idDistribuidor, int idEjecutivo,
                                     List<int> productos, List<int> cantidades)
        {
            using (var db = new Persistencia())
            {
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

        public List<PedidoDTO> ListarPedidosDTO(string loginDistribuidor)
        {
            var resultado = new List<PedidoDTO>();
            using (var db = new Persistencia())
            {
                List<Pedido> aDevolver = db.Pedidos
                .Where(p0 => p0.Distribuidor.Usuario.Login == loginDistribuidor && p0.Activo)
                .Include(p => p.Ejecutivo)
                .Include(p2 => p2.Ejecutivo.Usuario)
                .Include(p3 => p3.Distribuidor)
                .Include(p4 => p4.Distribuidor.Usuario)
                .ToList();
                if (aDevolver.Any())
                {
                    foreach (var pedido in aDevolver)
                    {
                        resultado.Add(new PedidoDTO()
                        {
                            PedidoId = pedido.PedidoID,
                            Aprobado = pedido.Aprobado,
                            Descripcion = pedido.Descripcion,
                            Ejecutivo = pedido.Ejecutivo.Nombre,
                            Fecha = pedido.Fecha
                        });
                    }
                }
            }
            return resultado;
        }

        public bool CambiarEstadoPedido(int idPedido, bool nuevoEstado)
        {
            bool pudeCambiar = false;
            using (var db = new Persistencia())
            {
                var aCambiar = db.Pedidos.Find(idPedido);
                if (aCambiar != null)
                {
                    aCambiar.Aprobado = nuevoEstado;
                    db.SaveChanges();
                    pudeCambiar = true;
                }
            }
            return pudeCambiar;
        }

        public bool BajaCantidadPedido(int idPedido, int idCantidadProductoPedido)
        {
            using (var db = new Persistencia())
            {

                var pepe = db.CantidadProductosPedido.ToList();

                int cantidadDeItems = db.Pedidos.Where(p => p.PedidoID == idPedido)
                    .Select(p => p.CantidadProductoPedidoList).Count();

                if (cantidadDeItems < 2)
                {
                    throw new CustomException("No se puede eliminar el item ya que el pedido debe tener al menos un elemento");
                }
                var cantPedidoProd = (from cpp in db.CantidadProductosPedido
                                        where cpp.Pedido.PedidoID == idPedido 
                                           && cpp.CantidadProductoPedidoID == idCantidadProductoPedido
                                        select cpp).SingleOrDefault();

                if (cantPedidoProd != null)
                {
                    //cantPedidoProd.Activo = false;
                  //  db.SaveChanges();
                  
                }
                else
                {
                    throw new CustomException("No se pudo encontrar cantidad producto pedido con id="+ idCantidadProductoPedido);
                }
                return true;

            }
        }

        public bool UpdateCantidadProductoPedido(int idPedido, int idCantidadProductoPedido, int cantidad)
        {
            if (cantidad <= 0) throw new CustomException("Cantidad debe ser mayor que cero");
            using (var db = new Persistencia())
            {

                var cantPedidoProd = db.CantidadProductosPedido.SingleOrDefault(p => p.CantidadProductoPedidoID == idCantidadProductoPedido);

                if (cantPedidoProd != null)
                {
                    cantPedidoProd.Cantidad = cantidad;
                    db.SaveChanges();
                }
                else
                {
                    throw new CustomException("No se pudo encontrar cantidad producto pedido con id=" + idCantidadProductoPedido);
                }
                return true;

            }
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Entity;
using System.Transactions;
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

        private ManejadorPedido()
        {
        }

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

                    var producto = db.Productos.SingleOrDefault(p => p.ProductoID == prodId);
        
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
                NotificarDistribuidores(pedido);
            }
        }

        private static void NotificarDistribuidores(Pedido nuevoPedido)
        {
            ManejadorEmail.GetInstance().NotificarDistribuidoresPorMail(nuevoPedido);
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
                    throw new CustomException("No se pudo elimianr el pedido") {Key = "PedidoID"};
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
                using (var scope = new TransactionScope())
                {
                    var pedido = db.Pedidos
                        .Include(p => p.CantidadProductoPedidoList)
                        .SingleOrDefault(p => p.PedidoID == idPedido && p.Activo == true);

                    if (pedido == null)
                    {
                        throw new CustomException("No se pudo encontrar el pedido con id=" + idPedido)
                        {
                            Key = "PedidoID"
                        };
                    }

                    int cantidadDeItems = pedido.CantidadProductoPedidoList.Count;

                    if (cantidadDeItems < 2)
                    {
                        throw new CustomException(
                            "No se puede eliminar el item ya que el pedido debe tener al menos un elemento");
                    }
       
                    var cantPedidoAEliminar =  db.CantidadProductosPedido
                                            .Include(p=>p.Pedido)
                                            .Include(p => p.Producto)
                                            .SingleOrDefault(p => p.CantidadProductoPedidoID == idCantidadProductoPedido);

                    if (cantPedidoAEliminar != null)
                    {

                        cantPedidoAEliminar.Pedido = null;
                        cantPedidoAEliminar.Producto = null;

                        pedido.CantidadProductoPedidoList.Remove(cantPedidoAEliminar);

                        db.CantidadProductosPedido.Remove(cantPedidoAEliminar);

                        db.SaveChanges();
                        scope.Complete();
                    }
                    else
                    {
                        throw new CustomException("No se pudo encontrar cantidad producto pedido con id=" +
                                                  idCantidadProductoPedido);
                    }
                }
                return true;
            }
        }

        public bool UpdateCantidadProductoPedido(int idPedido, int idCantidadProductoPedido, int cantidad)
        {
            if (cantidad <= 0) throw new CustomException("Cantidad debe ser mayor que cero");
            using (var db = new Persistencia())
            {
                var cantPedidoProd =
                    db.CantidadProductosPedido.SingleOrDefault(
                        p => p.CantidadProductoPedidoID == idCantidadProductoPedido);

                if (cantPedidoProd != null)
                {
                    cantPedidoProd.Cantidad = cantidad;
                    db.SaveChanges();
                }
                else
                {
                    throw new CustomException("No se pudo encontrar cantidad producto pedido con id=" +
                                              idCantidadProductoPedido);
                }
                return true;
            }
        }

        public List<CantidadProductoPedidoDTO> ListarProductosPedidoDTO(int idPedido)
        {
            var resultado = new List<CantidadProductoPedidoDTO>();
            using (var db = new Persistencia())
            {
                Pedido elPedido = db.Pedidos
                    .Where(p0 => p0.PedidoID == idPedido)
                    .Include(p1 => p1.CantidadProductoPedidoList.Select(p2 => p2.Producto))
                    .SingleOrDefault();
                if (elPedido != null
                    && elPedido.CantidadProductoPedidoList != null
                    && elPedido.CantidadProductoPedidoList.Any())
                {
                    foreach (var cantidadProductoPedido in elPedido.CantidadProductoPedidoList)
                    {
                        resultado.Add(new CantidadProductoPedidoDTO()
                        {
                            ProductoId = cantidadProductoPedido.Producto.ProductoID,
                            ProductoCodigo = cantidadProductoPedido.Producto.Codigo,
                            ProductoNombre = cantidadProductoPedido.Producto.Nombre,
                            CantidadPedida = cantidadProductoPedido.Cantidad
                        });
                    }
                }
            }
            return resultado;
        }

        public int AgregarCantidadPedido(int idPedido, int idProducto, int cantidad)
        {
            int ret = -1;

            if (cantidad <= 0) throw new CustomException("Cantidad debe ser mayor que cero");
            using (var db = new Persistencia())
            {

                try
                {

                    Producto producto = db.Productos.SingleOrDefault(p => p.ProductoID == idProducto && p.Activo == true);
                    //si el producto no existe o no esta activo error
                    if (producto == null)
                        throw new CustomException("El producto id=" + idProducto +
                                                  " no existe en la base de datos o no esta activo") {Key = "addItem"};

                    Pedido pedido =
                        db.Pedidos.Include(p => p.CantidadProductoPedidoList)
                            .SingleOrDefault(p => p.PedidoID == idPedido && p.Activo == true);
                    //si el pedido no existe o no esta activoerror
                    if (pedido == null)
                        throw new CustomException("El pedido id=" + idPedido +
                                                  " no existe en la base de datos o no esta activo") {Key = "addItem"};

                    //si el producto ya esta en algun item de la orden no dejo agregarlo de nuevo
                    bool yaAgregado = db.CantidadProductosPedido
                        .Any(c => c.Pedido.PedidoID == idPedido && c.Producto.ProductoID == idProducto);
                    if (yaAgregado)
                        throw new CustomException("El producto id=" + idProducto + " ya esta agregado en el pedido id =" +
                                                  idPedido) {Key = "addItem"};


                    CantidadProductoPedido newCantidadProductoPedido = new CantidadProductoPedido()
                    {
                        Activo = true,
                        Cantidad = cantidad,
                        Producto = producto,
                        Pedido = pedido
                    };
                    pedido.CantidadProductoPedidoList.Add(newCantidadProductoPedido);

                    db.SaveChanges();

                    ret = newCantidadProductoPedido.CantidadProductoPedidoID;
                }
                catch (CustomException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new CustomException("Error con AgregarCantidadPedido", e) {Key = "generalError"};
                }
            }
            return ret;
        }

        public void Modificar(Pedido newPedido)
        {
            using (var db = new Persistencia())
            {

                try
                {
                    Pedido pedido = db.Pedidos.SingleOrDefault(p => p.PedidoID == newPedido.PedidoID);
                    //si el pedido no existe o no esta activoerror
                    if (pedido == null)
                        throw new CustomException("El pedido id=" + newPedido.PedidoID +
                                                  " no existe en la base de datos") {Key = "PedidoID"};
                    pedido.Activo = newPedido.Activo;
                    pedido.Aprobado = newPedido.Aprobado;
                    pedido.Descripcion = newPedido.Descripcion;
                    pedido.Fecha = newPedido.Fecha;
                    db.SaveChanges();

                }
                catch (CustomException)
                {
                    throw;
                }
                catch (Exception e)
                {
                    throw new CustomException("Error en Modificar", e) {Key = "generalError"};
                }
            }
        }

    }
}

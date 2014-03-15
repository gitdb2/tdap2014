using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using uy.edu.ort.taller.aplicaciones.dominio.Exceptions;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace uy.edu.ort.taller.aplicaciones.negocio
{
    public class ManejadorReporte
    {

        #region singleton
        private static ManejadorReporte instance = new ManejadorReporte();

        private ManejadorReporte() { }

        public static ManejadorReporte GetInstance()
        {
            return instance;
        }
        #endregion


        public List<LogInfo> GetLogs(DateTime from, DateTime to)
        {
            using (var db = new Persistencia())
            {
                //db.Database.SqlQuery("select * from ");
                var data = db.Database.SqlQuery<LogInfo>
                    ("select [Date], [Login]"
                     + " from [TallerAplicaciones].[dbo].[Log]"
                     + " where [Message] ='Logueo Correcto' "
                     + " and [Date] >= {0}"
                     + " and [Date] <= {1}"
                     + " and [Login] is not null and [Login] <> '' "
                     + " order by 1 Desc", from, to);

                return data.ToList();

            }

        }


        public List<Pedido> GetPedidos(DateTime fromDate, DateTime toDate, int idDistribuidor, int idEjecuutivo)
        {
            return GetPedidos(fromDate, toDate, idDistribuidor, idEjecuutivo, Orderby.Fecha, OrdenDir.Desc);
        }


        public enum OrdenDir
        {
            Asc, Desc
        }
        public enum Orderby
        {
            Fecha, Distribuidor, Estado
        }

        public List<Pedido> GetPedidos(DateTime fromDate, DateTime toDate,
                            int idDistribuidor, int idEjecuutivo, Orderby orderby, OrdenDir dir)
        {
            using (var db = new Persistencia())
            {

                var query = db.Pedidos
                    .Include(p => p.Ejecutivo)
                    .Include(p2 => p2.Ejecutivo.Usuario)
                    .Include(p3 => p3.Distribuidor)
                    .Include(p4 => p4.Distribuidor.Usuario)
                    .Include(p5 => p5.Distribuidor.Empresa)
                    .Include(p6 => p6.CantidadProductoPedidoList)
                    .Include(p7 => p7.CantidadProductoPedidoList.Select(t => t.Producto))
                    .Where(p => p.Fecha >= fromDate && p.Fecha <= toDate);



                if (idDistribuidor > 0)
                {
                    query = query.Where(p => p.Distribuidor.PerfilUsuarioID == idDistribuidor);
                }
                if (idEjecuutivo > 0)
                {
                    query = query.Where(p => p.Ejecutivo.PerfilUsuarioID == idEjecuutivo);
                }

                switch (orderby)
                {
                    case Orderby.Distribuidor:
                        query = (dir == OrdenDir.Asc)
                            ? query.OrderBy(p => p.Distribuidor.Usuario.Login).ThenBy(p => p.PedidoID)
                            : query.OrderByDescending(p => p.Distribuidor.Usuario.Login).ThenBy(p => p.PedidoID);
                        break;
                    case Orderby.Estado:
                        query = (dir == OrdenDir.Asc)
                            ? query.OrderBy(p => p.Aprobado).ThenBy(p => p.PedidoID)
                            : query.OrderByDescending(p => p.Aprobado).ThenBy(p => p.PedidoID);
                        
                        break;
                    default:
                        query = (dir == OrdenDir.Asc)
                            ? query.OrderBy(p => p.Fecha).ThenBy(p => p.PedidoID)
                            : query.OrderByDescending(p => p.Fecha).ThenByDescending(p => p.PedidoID);
                        break;
            
                   
                }
                return query.ToList();
            }
        }

    }
}

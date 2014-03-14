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
                   

                return query.ToList();
            }
        }
    }
}

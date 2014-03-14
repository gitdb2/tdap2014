using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
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
    }
}

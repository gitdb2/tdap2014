using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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
            throw new NotImplementedException();
        }
    }
}

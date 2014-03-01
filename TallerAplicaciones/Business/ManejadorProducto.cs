using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.interfaces;

namespace uy.edu.ort.taller.aplicaciones.negocio
{
    public class ManejadorProducto : IProducto
    {
        #region singleton
        private static ManejadorProducto instance = new ManejadorProducto();

        private ManejadorProducto() { }

        public static ManejadorProducto GetInstance()
        {
            return instance;
        }
        #endregion

        public void AltaProducto(dominio.Producto producto)
        {
            throw new NotImplementedException();
        }

        public void BajaProducto(int idProducto)
        {
            throw new NotImplementedException();
        }

        public void ModificarProducto(dominio.Producto producto)
        {
            throw new NotImplementedException();
        }

        public List<IProducto> ListarProductos()
        {
            throw new NotImplementedException();
        }

        public void AgregarImagenProducto(int idProducto, dominio.Archivo imagen)
        {
            throw new NotImplementedException();
        }

        public void AgregarVideoProducto(int idProducto, dominio.Archivo video)
        {
            throw new NotImplementedException();
        }
    }
}

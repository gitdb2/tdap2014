using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace uy.edu.ort.taller.aplicaciones.interfaces
{
    public interface IProducto
    {

        void AltaProducto(Producto producto);

        void BajaProducto(int idProducto);

        void ModificarProducto(Producto producto);

        List<IProducto> ListarProductos();

        void AgregarImagenProducto(int idProducto, Archivo imagen);

        void AgregarVideoProducto(int idProducto, Archivo video);

    }
}

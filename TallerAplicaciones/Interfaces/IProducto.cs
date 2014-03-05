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

        bool BajaProducto(int idProducto);

        void ModificarProducto(Producto producto);

        List<Producto> ListarProductos();

        void AgregarImagenProducto(int idProducto, Foto imagen);

        void AgregarVideoProducto(int idProducto, Video video);

    }
}

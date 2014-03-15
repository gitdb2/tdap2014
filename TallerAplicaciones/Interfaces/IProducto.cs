using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.dominio.DTO;

namespace uy.edu.ort.taller.aplicaciones.interfaces
{
    public interface IProducto
    {

        void AltaProducto(Producto producto, List<int> idAtributoSimple, List<string> valorAtributoSimple,
            List<string> valorAtributoCombo, List<string> valorAtributoMulti);

        bool BajaProducto(int idProducto);

        void Modificar(Producto productoUpdate, List<int> filesToDelete);

        List<Producto> ListarProductos();

        void AgregarImagenProducto(int idProducto, Foto imagen);

        void AgregarVideoProducto(int idProducto, Video video);

        List<ProductoDTO> ListarProductosDTO();

        List<ValorAtributoDTO> ListarAtributosProductoDTO(int idProducto);

        List<ArchivoDTO> ListarImagenesProductoDTO(int idProducto);

        List<ArchivoDTO> ListarVideosProductoDTO(int idProducto);

    }
}

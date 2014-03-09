using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.dominio.DTO;

namespace uy.edu.ort.taller.aplicaciones.interfaces
{
    public interface IPedido
    {

        List<Pedido> ListarPedidos();

        List<PedidoDTO> ListarPedidos(string loginDistribuidor);

        void Alta(Pedido pedido, int idDistribuidor, int idEjecutivo,
    
        List<int> productos, List<int> cantidades);

        Pedido GetPedido(int idPedido);

        void Baja(int idPedido);

        bool CambiarEstadoPedido(int idPedido, bool nuevoEstado);

    }
}

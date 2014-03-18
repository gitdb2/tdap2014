using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Web;
using TallerAplicaciones.Filters;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.dominio.Constants;
using uy.edu.ort.taller.aplicaciones.dominio.DTO;
using uy.edu.ort.taller.aplicaciones.interfaces;
using uy.edu.ort.taller.aplicaciones.negocio;
using WebMatrix.WebData;

namespace TallerAplicaciones
{
    [ServiceContract(Namespace = "")]
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class ApiDistribuidores
    {
        private static log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        [OperationContract]
        public ResultadoLoginDTO Login(string login, string password)
        {
            IPerfilUsuario iPerfilUsuario = ManejadorPerfilUsuario.GetInstance();
            var resultadoLogin = new ResultadoLoginDTO() { LoginOk = false, Mensaje = "" };
            var usuarioDistribuidor = iPerfilUsuario.ObtenerUsuarioDistribuidor(login);
            if (usuarioDistribuidor != null && usuarioDistribuidor.Activo)
            {
                HttpContext.Current.Session[Constants.SESSION_LOGIN] = login;
                if (WebSecurity.Login(login, password))
                {
                    resultadoLogin.Mensaje = "Login OK";
                    resultadoLogin.LoginOk = true;
                    log.InfoFormat("Logueo correcto");
                }
                else
                {
                    resultadoLogin.Mensaje = "Usuario y/o Password incorrecto";
                }
            }
            else
            {
                resultadoLogin.Mensaje = "No existe tal Distribuidor";
            }
            return resultadoLogin;
        }

        [OperationContract]
        public List<PedidoDTO> ListarPedidosDistribuidor(string loginDistribuidor)
        {
            IPedido iPedido = ManejadorPedido.GetInstance();
            return iPedido.ListarPedidosDTO(loginDistribuidor);
        }

        [OperationContract]
        public bool CambiarEstadoPedido(int idPedido, bool nuevoEstado)
        {
            IPedido iPedido = ManejadorPedido.GetInstance();
            return iPedido.CambiarEstadoPedido(idPedido, nuevoEstado);
        }

        [OperationContract]
        public List<ProductoDTO> ListarProductos()
        {
            IProducto iProducto = ManejadorProducto.GetInstance();
            return iProducto.ListarProductosDTO();
        }

        [OperationContract]
        public List<ValorAtributoDTO> ListarAtributosProducto(int idProducto)
        {
            IProducto iProducto = ManejadorProducto.GetInstance();
            return iProducto.ListarAtributosProductoDTO(idProducto);
        }

        [OperationContract]
        public List<ArchivoDTO> ListarImagenesProducto(int idProducto)
        {
            IProducto iProducto = ManejadorProducto.GetInstance();
            return iProducto.ListarImagenesProductoDTO(idProducto);
        }

        [OperationContract]
        public List<ArchivoDTO> ListarVideosProducto(int idProducto)
        {
            IProducto iProducto = ManejadorProducto.GetInstance();
            return iProducto.ListarVideosProductoDTO(idProducto);
        }

        [OperationContract]
        public List<CantidadProductoPedidoDTO> ListarProductosPedido(int idPedido)
        {
            IPedido iPedido = ManejadorPedido.GetInstance();
            return iPedido.ListarProductosPedidoDTO(idPedido);
        }

    }
}

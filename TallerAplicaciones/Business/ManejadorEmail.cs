using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using uy.edu.ort.taller.aplicaciones.dominio;

namespace uy.edu.ort.taller.aplicaciones.negocio
{
    public class ManejadorEmail
    {
        #region singleton

        private static ManejadorEmail instance = new ManejadorEmail();

        private ManejadorEmail()
        {
        }

        public static ManejadorEmail GetInstance()
        {
            return instance;
        }

        #endregion

        private const string FormatoFecha = "yyyy-MM-dd H:mm";

        public void NotificarDistribuidoresPorMail(Pedido pedido)
        {
            List<Distribuidor> destinatarios = ManejadorPerfilUsuario
                .GetInstance()
                .ObtenerDistribuidoresDeEmpresa(pedido.Distribuidor.Empresa.EmpresaDistribuidoraID);
            EnviarMails(destinatarios, pedido);
        }

        private void EnviarMails(List<Distribuidor> destinatarios, Pedido pedido)
        {
            string cuerpoEmail = GenerarCuerpoEmail(pedido);
            foreach (var distribuidor in destinatarios)
            {
                EnviarMail(distribuidor, cuerpoEmail);
            }
        }

        private void EnviarMail(Distribuidor distribuidor, string cuerpoEmail)
        {
            var fromAddress = new MailAddress("tallerdisenoort2014@gmail.com", "Sistema de Alta de Pedidos");
            var toAddress = new MailAddress(distribuidor.Email, "Destinatario Mail Taller");
            const string fromPassword = "112233taller";
            const string subject = "Notificacion de Alta de Pedido";

            var bodyBuilder = new StringBuilder();
            bodyBuilder.Append("Sr Distribuidor ").Append(distribuidor.Nombre).Append("\n\n");
            bodyBuilder.Append("A continuacion detallamos el Pedido que acaba de realizarse.").Append("\n\n");
            bodyBuilder.Append(cuerpoEmail);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword),
                Timeout = 20000
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = subject,
                Body = bodyBuilder.ToString()
            })
            {
                smtp.Send(message);
            }
        }

        private string GenerarCuerpoEmail(Pedido p)
        {
            var stringBuilder = new StringBuilder();
            if (p.CantidadProductoPedidoList.Any())
            {
                stringBuilder.Append("Ejecutivo de Cuentas que realizo el Pedido:").Append(p.Ejecutivo.Nombre).Append("\n");
                stringBuilder.Append("Identificador del Pedido: ").Append(p.PedidoID).Append("\n");
                stringBuilder.Append("Descripcion del Pedido: ").Append(p.Descripcion).Append("\n");
                stringBuilder.Append("Fecha que se realizo el Pedido: ").Append(p.Fecha.ToString(FormatoFecha)).Append("\n");
                stringBuilder.Append("Estado del Pedido: ").Append(p.Aprobado ? "Aprobado" : "No Aprobado").Append("\n");
                stringBuilder.Append("\n").Append("Productos del Pedido: ").Append("\n");
                foreach (var cpp in p.CantidadProductoPedidoList)
                {
                    stringBuilder.Append("Producto: ").Append(cpp.Producto.Nombre);
                    stringBuilder.Append(", Cantidad: ").Append(cpp.Cantidad).Append("\n");
                }
            }
            else
            {
                stringBuilder.Append("No hay Productos en el Pedido.").Append("\n").Append("\n");
                stringBuilder.Append("Ocurrio un error al generar este email, contactese con el Administrador.");
            }
            return stringBuilder.ToString();
        }

    }
}

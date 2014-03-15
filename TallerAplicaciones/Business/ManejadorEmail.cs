using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using uy.edu.ort.taller.aplicaciones.dominio;
using uy.edu.ort.taller.aplicaciones.utiles;

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
            var emailRemitente = Settings.GetInstance().GetProperty("mail.remitente.direccion");
            var emailRemitentePassword = Settings.GetInstance().GetProperty("mail.remitente.password");
            var nombreRemitente = Settings.GetInstance().GetProperty("mail.remitente.nombre");
            var nombreServidor = Settings.GetInstance().GetProperty("mail.servidor.direccion");
            var puertoServidor = Settings.GetInstance().GetProperty("mail.servidor.puerto");
            var timeoutEnvioServidor = Settings.GetInstance().GetProperty("mail.servidor.envio.timeout", "20000");
            var emailAsunto = Settings.GetInstance().GetProperty("mail.asunto");

            var fromAddress = new MailAddress(emailRemitente, nombreRemitente);
            var toAddress = new MailAddress(distribuidor.Email, distribuidor.Nombre + " " + distribuidor.Apellido);

            var bodyBuilder = new StringBuilder();
            bodyBuilder.Append("Sr Distribuidor ").Append(distribuidor.Nombre).Append("\n\n");
            bodyBuilder.Append("A continuacion detallamos el Pedido que acaba de realizarse.").Append("\n\n");
            bodyBuilder.Append(cuerpoEmail);

            var smtp = new SmtpClient
            {
                Host = nombreServidor,
                Port = Int16.Parse(puertoServidor),
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, emailRemitentePassword),
                Timeout = Int16.Parse(timeoutEnvioServidor)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = emailAsunto,
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

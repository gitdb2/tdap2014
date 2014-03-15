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

        private string _emailRemitente;
        private string _emailRemitentePassword;
        private string _nombreRemitente;
        private string _nombreServidor;
        private string _emailAsunto;
        private int _puertoServidor;
        private int _timeoutEnvioServidor;

        public void NotificarDistribuidoresPorMail(Pedido pedido)
        {
            IntentarCargarVariablesEnvioMail();
            List<Distribuidor> destinatarios = ManejadorPerfilUsuario.GetInstance().ObtenerDistribuidoresDeEmpresa(pedido.Distribuidor.Empresa.EmpresaDistribuidoraID);
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
            var fromAddress = new MailAddress(_emailRemitente, _nombreRemitente);
            var toAddress = new MailAddress(distribuidor.Email, distribuidor.Nombre + " " + distribuidor.Apellido);

            var bodyBuilder = new StringBuilder();
            bodyBuilder.Append("Sr. Distribuidor ").Append(distribuidor.Nombre).Append(" ").Append(distribuidor.Apellido).Append("\n\n");
            bodyBuilder.Append("A continuacion detallamos el Pedido que acaba de realizarse.").Append("\n\n");
            bodyBuilder.Append(cuerpoEmail);

            var smtp = new SmtpClient
            {
                Host = _nombreServidor,
                Port = _puertoServidor,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                Credentials = new NetworkCredential(fromAddress.Address, _emailRemitentePassword),
                Timeout = _timeoutEnvioServidor
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = _emailAsunto,
                Body = bodyBuilder.ToString()
            })
            {
                smtp.Send(message);
            }
        }

        private void IntentarCargarVariablesEnvioMail()
        {
            _emailRemitente = CargarVariableString("mail.remitente.direccion");
            _emailRemitentePassword = CargarVariableString("mail.remitente.password");
            _nombreRemitente = CargarVariableString("mail.remitente.nombre");
            _nombreServidor = CargarVariableString("mail.servidor.direccion");
            _emailAsunto = CargarVariableString("mail.asunto");
            _puertoServidor = CargarVariableNumerica("mail.servidor.puerto");
            _timeoutEnvioServidor = CargarVariableNumerica("mail.servidor.envio.timeout");
        }

        private string CargarVariableString(string clave)
        {
            string valorVariable = null;
            valorVariable = Settings.GetInstance().GetProperty(clave);
            if (valorVariable == null || valorVariable.Trim().Equals(""))
            {
                throw new ArgumentException("El valor de la variable " + clave + " no puede ser vacia");
            }
            return valorVariable;
        }

        private int CargarVariableNumerica(string clave)
        {
            string valorVariable = CargarVariableString(clave);
            int resultado;
            if (!int.TryParse(valorVariable, out resultado))
            {
                throw new ArgumentException("El valor de la variable " + clave + " debe ser numerico");
            }
            return resultado;
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

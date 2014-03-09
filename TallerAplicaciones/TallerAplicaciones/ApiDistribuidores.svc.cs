using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using TallerAplicaciones.Filters;
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

        [OperationContract]
        public bool Login(string login, string password)
        {
            IPerfilUsuario iPerfilUsuario = ManejadorPerfilUsuario.GetInstance();
            var usuario = iPerfilUsuario.ObtenerUsuario(login);
            return usuario != null && usuario.Activo && WebSecurity.Login(login, password);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ApiDistribuidores
{

    [ServiceContract]
    public interface IApiDistribuidores
    {

        [OperationContract]
        Guid RealizarApuesta(int[] numeros);

        
        [OperationContract]
        double CorrespondePremio(Guid identificadorApuesta);

        
        [OperationContract]
        void EfectuarPago(Guid identificadorApuesta);

    }
}

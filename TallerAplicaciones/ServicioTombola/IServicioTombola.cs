using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ServicioTombola
{

    [ServiceContract]
    public interface IServicioTombola
    {

        [OperationContract]
        Guid RealizarApuesta(int[] numeros);

        
        [OperationContract]
        double CorrespondePremio(Guid identificadorApuesta);

        
        [OperationContract]
        void EfectuarPago(Guid identificadorApuesta);

    }
}

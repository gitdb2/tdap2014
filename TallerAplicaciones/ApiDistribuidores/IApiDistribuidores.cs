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
        bool Login(string login, string password);

    }
}

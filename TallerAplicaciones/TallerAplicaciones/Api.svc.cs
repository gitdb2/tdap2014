using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;

namespace TallerAplicaciones
{
    [ServiceContract(Namespace = "")]
    [SilverlightFaultBehavior]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Api
    {
        [OperationContract]
        public bool DoWork(bool param)
        {
            // Add your operation implementation here
            return !param;
        }

        // Add more operations here and mark them with [OperationContract]
    }
}

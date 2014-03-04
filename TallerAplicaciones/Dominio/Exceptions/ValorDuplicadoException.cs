using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace uy.edu.ort.taller.aplicaciones.dominio.Exceptions
{
    [Serializable]
    public class ValorDuplicadoException : Exception
    {

        public ValorDuplicadoException()
        {
        }

        public ValorDuplicadoException(string message)
            : base(message)
        {
        }

        public ValorDuplicadoException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected ValorDuplicadoException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }


    }
}

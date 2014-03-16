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

    [Serializable]
    public class EnvioMailException : Exception
    {
         public EnvioMailException()
        {
        }

        public EnvioMailException(string message)
            : base(message)
        {
        }

        public EnvioMailException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected EnvioMailException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }
    }

    [Serializable]
    public class AtributoYaEstaAsociadoException : Exception
    {

        public AtributoYaEstaAsociadoException()
        {
        }

        public AtributoYaEstaAsociadoException(string message)
            : base(message)
        {
        }

        public AtributoYaEstaAsociadoException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected AtributoYaEstaAsociadoException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }

    [Serializable]
    public class CustomException : Exception
    {

        public string Key { get; set; }

        public CustomException()
        {
        }

        public CustomException(string message)
            : base(message)
        {
        }

        public CustomException(string message, Exception inner)
            : base(message, inner)
        {
        }

        protected CustomException(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
        }

    }
}

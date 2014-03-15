using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using uy.edu.ort.taller.aplicaciones.dominio.Constants;
using WebMatrix.WebData;

namespace TallerAplicaciones.logs
{
    public class HttpContextUserNameProvider
    {
        public override string ToString()
        {
            HttpContext context = HttpContext.Current;
            if (context != null && context.Session != null && context.Session[Constants.SESSION_LOGIN] != null)
            {
                return (string)context.Session[Constants.SESSION_LOGIN];
            }

            return "-";
        }
    }
}
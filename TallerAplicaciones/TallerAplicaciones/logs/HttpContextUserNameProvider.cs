using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebMatrix.WebData;

namespace TallerAplicaciones.logs
{
    public class HttpContextUserNameProvider
    {
        public override string ToString()
        {
            HttpContext context = HttpContext.Current;
            if (context != null && context.Session != null && context.Session["login"] != null)
            {
                return (string)context.Session["login"];
            }

            //if (WebSecurity.CurrentUserId != null)
            //{
            //    return WebSecurity.CurrentUserId.ToString();
            //}

            //HttpContext context = HttpContext.Current;
            //if (context != null && context.User != null && context.User.Identity.IsAuthenticated)
            //{
                //return context.User.Identity.Name;
            //}
            return "-";
        }
    }
}
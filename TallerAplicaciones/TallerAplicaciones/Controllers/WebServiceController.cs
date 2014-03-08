using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace TallerAplicaciones.Controllers
{
    public class WebServiceController : ApiController
    {
        // GET api/webservice
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/webservice/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/webservice
        public void Post([FromBody]string value)
        {
        }

        // PUT api/webservice/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/webservice/5
        public void Delete(int id)
        {
        }
    }
}

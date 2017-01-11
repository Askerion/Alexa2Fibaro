using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace FibaroHomeCenter.Alexa
{
    public class AlexaController : ApiController
    {
        [Route("alexa/homecenter")]
        [HttpPost]
        public HttpResponseMessage SampleSession()
        {
            var speechlet = new HomeCenterSessionSpeechlet();
            return speechlet.GetResponse(Request);
        }
    }
}
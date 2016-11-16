using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace Microsoft.Translator.Web.Controllers
{
    [Route("api/[controller]")]
    public class TranslatorController : Controller
    {

        private readonly AdmAuthentication _admAuthentication;

        public TranslatorController(AdmAuthentication admAuthentication)
        {
            _admAuthentication = admAuthentication;
        }

        // GET api/Translator
        /*
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }
        */

        // GET api/Translator/5
        [HttpGet("{text}")]
        public string Get(string text, [FromQuery] string from, [FromQuery] string to)
        {
            using (var client = new TranslatorClient(_admAuthentication.GetAccessToken()))
            {
                return client.TranslateAction(text, from, to);
            }
        }

        /*
        // POST api/values
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/Translator/5
        [HttpPut("{text}")]
        public void Put(int text, [FromBody]string value)
        {
        }

        // DELETE api/Translator/5
        [HttpDelete("{text}")]
        public void Delete(string text)
        {
        }
        */
    }
}

using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Headers;

namespace Project.WebApi.Controllers
{ /// <summary>
  /// Web API as the name suggests, 
  /// is an API over the web which can be accessed using HTTP protocol.
  /// </summary>


    [Route("[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {

        public AuthenticationController()
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "hdkjehdhekfhd");



        }

        [HttpGet]
        public string Get()
        {
            return "this is a test from the Authentication Controller";
        }




    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.Models;
using AutoBuildApp.Managers;
using AutoBuildApp.Services.UserServices;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace AutoBuildApp.Api.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class RegistrationController : Controller
    {
        RegistrationManager _registrationManager = new RegistrationManager(ConnectionManager.connectionManager.GetConnectionStringByName(ControllerGlobals.DOCKER_CONNECTION));

        [HttpPost]
        public IActionResult RegisterUser(string username, string firstname, string lastname, string email, string password,
            string passwordCheck)
        {
            //username = "spiderman";
            //firstname = "Peter";
            //lastname = "Parker";
            //email = "spiderman@gmail.com";
            //password = "Password123";
            //passwordCheck = "Password123";

            return Ok(_registrationManager.RegisterUser(username, firstname, lastname, email, password, passwordCheck));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Managers;
using AutoBuildApp.Security.Enumerations;
using AutoBuildApp.Services.UserServices;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AutoBuildApp.Api.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("[controller]")]
    [ApiController]

    public class UserManagementController : Controller
    {
        private readonly UserManagementDAO _userManagementDAO = new UserManagementDAO(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));

        [HttpPut("password")]
        public IActionResult UpdatePassword(string password, string email)
        {
            //password = "123passwordD";
            //string userEmail = "ZeinabFarhat@gmail.com";
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService, ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));
            
            return Ok(userManagementManager.UpdatePassword(password, email));
        }

        [HttpPut("email")]
        public IActionResult UpdateEmail(string inputEmail, string email)
        {
            //inputEmail = "bobross@gmail.com";
            //string email = "ZeinabFarhat@gmail.com";
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService, ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));
            
            return Ok(userManagementManager.UpdateEmail(inputEmail, email));
        }

        [HttpPut("username")]
        public IActionResult UpdateUsername(string username, string email)
        {
            //username = "Charley";
            //string email = "ZeinabFarhat@gmail.com";
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService, ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));
            
            return Ok(userManagementManager.UpdateUsername(username, email));
        }


        [HttpGet]
        public IActionResult GetAllUserAccounts()
        {
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService, ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));

            var userAccounts = userManagementManager.GetUsersList();
            return Ok(userAccounts);
        }
       
        [HttpPut("permission")]
        public IActionResult UpdatePermission(IFormCollection formCollection)
        {
            var username = formCollection["username"];
            var permission = (formCollection["permission"]);
            //Console.WriteLine("Update permissions here");
            //username = "SERGE";
            //role = RoleEnumType.SYSTEM_ADMIN;
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService, ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));
            
            return Ok(userManagementManager.ChangePermissions(username, permission));
        }

        [HttpPut("lock")]
        public IActionResult UpdateLockState(IFormCollection formCollection)
        {
            var username = formCollection["username"];
            var lockstate = Convert.ToBoolean(formCollection["lockstate"]);
            //Console.WriteLine("Change lock state here");
            //username = "SERGE";
            //lockstate = false;
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService, ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));
            
            return Ok(userManagementManager.ChangeLockState(username, lockstate));
        }

        [HttpDelete("user")]
        public IActionResult DeleteUser(IFormCollection formCollection)
        {
            var username = formCollection["username"];
            //username = "kingPeni393";
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService, ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));

            return Ok(userManagementManager.DeleteUser(username));
        }
    }
}

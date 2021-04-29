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
        public IActionResult UpdatePassword(string password)
        {
            password = "123passwordD";
            string userEmail = "ZeinabFarhat@gmail.com";
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService, ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));
            
            return Ok(userManagementManager.UpdatePassword(password, userEmail));
        }

        [HttpPut("email")]
        public IActionResult UpdateEmail(string inputEmail)
        {
            inputEmail = "bobross@gmail.com";
            string email = "ZeinabFarhat@gmail.com";
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService, ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));
            
            return Ok(userManagementManager.UpdateEmail(inputEmail, email));
        }

        [HttpPut("username")]
        public IActionResult UpdateUsername(string username)
        {
            username = "Charley";
            string email = "ZeinabFarhat@gmail.com";
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
        public IActionResult UpdatePermission(string username, string role)
        {
            Console.WriteLine("Update permissions here");
            username = "SERGE";
            role = RoleEnumType.BASIC_ROLE;
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService, ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));
            
            return Ok(userManagementManager.ChangePermissions(username, role));
        }

        [HttpPut("lock")]
        public IActionResult UpdateLockState(string username, string lockstate)
        {
            Console.WriteLine("Change lock state here");
            username = "SERGE";
            lockstate = RoleEnumType.BASIC_ROLE;
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService, ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));
            
            return Ok(userManagementManager.ChangeLockState(username, lockstate));
        }

        [HttpDelete("user")]
        public IActionResult DeleteUser(string email)
        {
            email = "spiderman@gmail.com";
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService, ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));

            return Ok(userManagementManager.DeleteUser(email));
        }
    }
}

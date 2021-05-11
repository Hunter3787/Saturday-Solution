using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoBuildApp.Api.HelperFunctions;
using AutoBuildApp.DataAccess;
using AutoBuildApp.Logging;
using AutoBuildApp.Managers;
using AutoBuildApp.Security;
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
        private List<string> _adminRoles; //specify roles
        private readonly LoggingProducerService _logger = LoggingProducerService.GetInstance;

        // UMDAO has the connection string
        private readonly UserManagementDAO _userManagementDAO = new UserManagementDAO(ConnectionManager.connectionManager.GetConnectionStringByName("MyConnection"));

        public UserManagementController()
        {
            _adminRoles = new List<string>()
            { RoleEnumType.SystemAdmin,
              RoleEnumType.DelegateAdmin};
        }

        [HttpPut("password")]
        public IActionResult UpdatePassword(IFormCollection formCollection)
        {
            _logger.LogInformation("Update password called.");
            // pass in from front end form data
            var password = formCollection["password"];
            var passwordCheck = formCollection["passwordCheck"];
            var activeEmail = formCollection["activeEmail"];

            // connection string is in DAO, pass through UMservice to UMmanager
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // calls DeleteUser from UMmanager
            // decrypts password and password check passed from the front end with key
            return Ok(userManagementManager.UpdatePassword(userManagementManager.DecryptByAES(password, "12345678900000001234567890000000"),
                userManagementManager.DecryptByAES(passwordCheck, "12345678900000001234567890000000"), activeEmail));
        }

        [HttpPut("email")]
        public IActionResult UpdateEmail(IFormCollection formCollection)
        {
            _logger.LogInformation("Update email called.");
            // pass in from front end form data
            var inputEmail = formCollection["inputEmail"];
            var activeEmail = formCollection["activeEmail"];

            // connection string is in DAO, pass through UMservice to UMmanager
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);
            
            // calls updateemail from UMmanager
            return Ok(userManagementManager.UpdateEmail(inputEmail, activeEmail));
        }

        [HttpPut("username")]
        public IActionResult UpdateUsername(IFormCollection formCollection)
        {
            _logger.LogInformation("Update usernamed called.");
            // pass in from front end form data
            var username = formCollection["username"];
            var activeEmail = formCollection["activeEmail"];

            // connection string is in DAO, pass through UMservice to UMmanager
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // calls updateusername from UMmanager
            return Ok(userManagementManager.UpdateUsername(username, activeEmail));
        }


        [HttpGet]
        public IActionResult GetAllUserAccounts()
        {
            if (!AuthorizationCheck.IsAuthorized(_adminRoles))
            {
                _logger.LogWarning("Unauthorized Access!!!");

                _logger.LogWarning("StatusCodes.Status403Forbidden");

                return new StatusCodeResult(StatusCodes.Status403Forbidden);
            }

            _logger.LogInformation("Get registered users called.");
            // connection string is in DAO, pass through UMservice to UMmanager
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // calls getuserslist from UMmanager
            var userAccounts = userManagementManager.GetUsersList();
            return Ok(userAccounts);
        }
       
        [HttpPut("permission")]
        public IActionResult UpdatePermission(IFormCollection formCollection)
        {
            _logger.LogInformation("Change permissions called.");
            // pass in from front end form data
            var username = formCollection["username"];
            var permission = (formCollection["permission"]);

            // connection string is in DAO, pass through UMservice to UMmanager
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // calls changepermissions from UMmanager
            return Ok(userManagementManager.ChangePermissions(username, permission));
        }

        [HttpPut("lock")]
        public IActionResult UpdateLockState(IFormCollection formCollection)
        {
            _logger.LogInformation("Lock user called.");
            // pass in from front end form data
            var username = formCollection["username"];
            var lockstate = Convert.ToBoolean(formCollection["lockstate"]);

            // connection string is in DAO, pass through UMservice to UMmanager
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // calls changelockstate from UMmanager
            return Ok(userManagementManager.ChangeLockState(username, lockstate));
        }

        [HttpDelete("user")]
        public IActionResult DeleteUser(IFormCollection formCollection)
        {
            _logger.LogInformation("Delete user called.");
            // pass in from front end form data
            var username = formCollection["username"];

            // connection string is in DAO, pass through UMservice to UMmanager
            UserManagementService userManagementService = new UserManagementService(_userManagementDAO);
            UserManagementManager userManagementManager = new UserManagementManager(userManagementService);

            // calls DeleteUser from UMmanager
            return Ok(userManagementManager.DeleteUser(username));
        }
    }
}

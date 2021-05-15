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
using Microsoft.AspNetCore.Identity;
using AutoBuildApp.Models.Users;
using AutoMapper;
using AutoBuildApp.Services.EmailService;

namespace AutoBuildApp.Api.Controllers
{
    [EnableCors("CorsPolicy")]
    [Route("[controller]")]
    [ApiController]
    public class RegistrationController : Controller
    {
        //private readonly IMapper _mapper;
        //private readonly UserManager<User> _userManager;
        //private readonly SignInManager<User> _signInManager;
        //private readonly IEmailSender _emailSender;

        //public RegistrationController(IMapper mapper, UserManager<User> userManager, SignInManager<User> signInManager, IEmailSender emailSender)
        //{
        //    _mapper = mapper;
        //    _userManager = userManager;
        //    _signInManager = signInManager;
        //    _emailSender = emailSender;
        //}
        RegistrationManager _registrationManager = new RegistrationManager(ConnectionManager.connectionManager.GetConnectionStringByName(ControllerGlobals.ADMIN_CREDENTIALS_CONNECTION));

        [HttpPost]  
        //public async Task<IActionResult> RegisterUser(string username, string firstname, string lastname, string email, string password,
        //    string passwordCheck)
        public IActionResult RegisterUser(string username, string firstname, string lastname, string email, string password,
            string passwordCheck)
        {
            //RegisterUser registerUser = new RegisterUser();
            //registerUser.Email = email;
            //var user = _mapper.Map<User>(registerUser);

            //var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            //var confirmationLink = Url.Action(nameof(ConfirmEmail), "Account", new { token, email = user.Email }, Request.Scheme);

            //var message = new Message(new string[] { user.Email }, "Confirmation email link", confirmationLink, null);
            //await _emailSender.SendEmailAsync(message);

            //await _userManager.AddToRoleAsync(user, "Visitor");


            return Ok(_registrationManager.RegisterUser(username, firstname, lastname, email, password, passwordCheck));
        }

        //[HttpGet]
        //public async Task<IActionResult> ConfirmEmail(string token, string email)
        //{
        //    var user = await _userManager.FindByEmailAsync(email);
        //    if (user == null)
        //        return View("Error");

        //    var result = await _userManager.ConfirmEmailAsync(user, token);
        //    return View(result.Succeeded ? nameof(ConfirmEmail) : "Error");
        //}
    }
}

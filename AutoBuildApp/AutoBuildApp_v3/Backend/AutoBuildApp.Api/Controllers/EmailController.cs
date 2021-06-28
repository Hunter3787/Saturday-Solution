//using MailKit.Net.Smtp;
//using Microsoft.AspNetCore.Cors;
//using Microsoft.AspNetCore.Mvc;
//using MimeKit;
//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Net.Mail;
//using System.Net;
//using System.Threading.Tasks;

//namespace AutoBuildApp.Api.Controllers
//{
//    [EnableCors("CorsPolicy")]
//    [Route("[controller]")]
//    [ApiController]
//    public class EmailController : Controller
//    {
//        [HttpPost]
//        public IActionResult SendEmail()
//        {
//            //var email = new MailMessage();
//            string to = "crkobel@verizon.net";
//            string from = "autobuild1337@gmail.com";
//            MailMessage mail = new MailMessage(from, to);
//            mail.Subject = "Test using SMTP";
//            mail.Body = @"Blah Blah Blach";
//            //SmtpClient client = new SmtpClient(server)
//            //email.From.("from_address@example.com"));
//            //email.To.Add(MailboxAddress.Parse("crkobel@verizon.net"));
//            //email.Subject = "Test Email Subject";
//            //email.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = "Example Plain Text Message Body" };

//            using (System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient())
//            {
//                smtp.DeliveryMethod = SmtpDeliveryMethod.Network;
//                smtp.UseDefaultCredentials = false;
//                smtp.EnableSsl = true;
//                smtp.Host = "smtp.gmail.com";
//                smtp.Port = 587;
//                smtp.Credentials = new NetworkCredential("autobuild1337@gmail.com", "Passwordispassword123");
//                smtp.Send(mail);
//            }

//            //using var smtp = new SmtpClient();
//            //smtp.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
//            //smtp.Authenticate("autobuild1337@gmail.com", "Passwordispassword123");
//            //smtp.Send(email);
//            //smtp.Disconnect(true);
//            return Ok();
//        }
//    }
//}

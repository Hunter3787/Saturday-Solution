using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using System.Threading;

namespace AutoBuildSecure.ConsoleApp
{
    public class CP
    {


        public void testing()
        {

            Console.WriteLine($"in the cp class:  ");

            // get the current principle that is on the thread:
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            // to recieve the "username"/ "userEmail" on the thread do the following:
            String mynameis = _threadPrinciple.Identity.Name;

            Console.WriteLine($"\n\nCurrent Thread Priciple: { JsonSerializer.Serialize(Thread.CurrentPrincipal)} \n" +
                $"\n Whats my name?  name  - { mynameis}");


            foreach (var clm in _threadPrinciple.Claims)
            {
                Console.WriteLine($"claim type: { clm.Type } claim value: {clm.Value} ");
            }

        }
        public void conors()
        {

            Console.WriteLine($"in the connor method:  ");

            // get the current principle that is on the thread:
            ClaimsPrincipal _threadPrinciple 
                = (ClaimsPrincipal)Thread.CurrentPrincipal;
            // to recieve the "username"/ "userEmail" on the thread do the following:
            String mynameis = _threadPrinciple.Identity.Name;

            Console.WriteLine($"\n\nCurrent Thread Priciple: { JsonSerializer.Serialize(Thread.CurrentPrincipal)} \n" +
                $"\n Whats my name?  name  - { mynameis}");


            foreach (var clm in _threadPrinciple.Claims)
            {
                Console.WriteLine($"claim type: { clm.Type } claim value: {clm.Value} ");
            }
            Console.WriteLine($"I AM HER: \n" +
                $"{_threadPrinciple.FindFirst(ClaimTypes.Email).Value}\n");

            //https://stackoverflow.com/questions/24587414/how-to-update-a-claim-in-asp-net-identity 

            var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
            
            Console.WriteLine($" HERE {identity.FindFirst(ClaimTypes.Email)} \n"); 
           identity.RemoveClaim(identity.FindFirst(ClaimTypes.Email));
           string returnedVal = "this is my new email";
            identity.AddClaim(new Claim(ClaimTypes.Email, returnedVal));
            _threadPrinciple = new ClaimsPrincipal(identity);
            Thread.CurrentPrincipal = _threadPrinciple;
        }




    }
}

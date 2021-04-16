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

        public void updateEmailThread()
        {
            Console.WriteLine($"----------------");
            ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;
            String mynameis = _threadPrinciple.Identity.Name;

            Console.WriteLine($"\n\nCurrent Thread Priciple: { JsonSerializer.Serialize(Thread.CurrentPrincipal)} \n" +
                $"\n Whats my name?  name  - { mynameis}");

            foreach (var clm in _threadPrinciple.Claims)
            {
                Console.WriteLine($"claim type: {clm.Type} claim value: {clm.Value}");
            }

            Console.WriteLine($"I AM HERE: \n" +
                $"{_threadPrinciple.FindFirst(ClaimTypes.Email).Value}\n");

            var identity = Thread.CurrentPrincipal.Identity as ClaimsIdentity;
            Console.WriteLine($"HERE {identity.FindFirst(ClaimTypes.Email)} \n");
            identity.RemoveClaim(identity.FindFirst(ClaimTypes.Email));
            //string returnedVal1 = (_threadPrinciple.FindFirst(ClaimTypes.Email).Value).ToString();
            string returnedVal = "bob222@gmail.com";
            identity.AddClaim(new Claim(ClaimTypes.Email, returnedVal));
            _threadPrinciple = new ClaimsPrincipal(identity);
            Thread.CurrentPrincipal = _threadPrinciple;
        }


    }
}

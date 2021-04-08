using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Text;
using System.Threading;

namespace AutoBuildSecure.ConsoleApp
{
    public class CP
    {

        ClaimsPrincipal _threadPrinciple = (ClaimsPrincipal)Thread.CurrentPrincipal;

        public void testing()
        {
            Console.WriteLine($"in the cp class:  ");
            foreach (var clm in _threadPrinciple.Claims)
            {
                Console.WriteLine($"claim type: { clm.Type } claim value: {clm.Value} ");
            }

        }




    }

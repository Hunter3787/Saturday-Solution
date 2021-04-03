using Project.Security.Models;
using System;
using System.Collections.Generic;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace Project.Manager
{
    public class testing
    {

        public testing()
        {

            //CheckCompatibility();
        }

        private static void CheckCompatibility()
        {
            IPrincipal currentPrincipal = Thread.CurrentPrincipal;
            Console.WriteLine(currentPrincipal.Identity.Name);
            Console.WriteLine(currentPrincipal.IsInRole("IT"));

        }

        public void checkcustomPrinciple()
        {
            UserPrinciple customPrinciple = (UserPrinciple)Thread.CurrentPrincipal;
            Thread.CurrentPrincipal = customPrinciple;
            Console.WriteLine(customPrinciple.getPermissions());

        }

    }
}

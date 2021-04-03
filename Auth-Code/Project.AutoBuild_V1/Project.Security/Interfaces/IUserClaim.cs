using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Project.Security.Models
{
    public interface IUserClaim
    {

        public string Permission { get; set; }

        public string scopeOfPermissions { get; set; }


    }
}

﻿using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Project.Security.Models
{
    /// <summary>
    /// defining a claims interface for Autobuilds user
    /// holding the persmissions and the scope/target of the permisions
    /// </summary>
    public interface IUserClaim
    {
       string Permission { get; set; }
       string scopeOfPermissions { get; set; }
    }
}

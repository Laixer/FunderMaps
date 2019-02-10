﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace FunderMaps
{
    public class Constants
    {
        public static readonly PasswordOptions PasswordPolicy = new PasswordOptions
        {
            RequireDigit = false,
            RequireLowercase = false,
            RequireNonAlphanumeric = false,
            RequireUppercase = false,
            RequiredLength = 6,
            RequiredUniqueChars = 1,
        };
    }
}

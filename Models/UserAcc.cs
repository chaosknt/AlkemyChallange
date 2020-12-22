﻿using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace AlkemyChallange.Models
{
    public class UserAcc : IdentityUser<Guid>
    {
        
        public string Name { get; set; }
       
        public string LastName { get; set; }
        
        public string DNI { get; set; }

        public string Docket { get; set; }


    }
}
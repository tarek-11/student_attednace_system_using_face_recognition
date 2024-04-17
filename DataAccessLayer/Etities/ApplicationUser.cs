using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Etities
{
    public class ApplicationUser : IdentityUser
    {
        public bool Isagree { get; set; }
        public string UserRoleName { get; set; }
    }
    
}

using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Identity.Models
{
    public class AppUser : IdentityUser<Guid>
    {
        public string AccessToken { get; set; } = string.Empty;
        public DateTime AccessTokenExpireDate { get; set; }
    }
}

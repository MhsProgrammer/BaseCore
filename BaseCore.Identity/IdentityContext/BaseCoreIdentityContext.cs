using BaseCore.Identity.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Identity.IdentityContext
{
    public class BaseCoreIdentityContext : IdentityDbContext<AppUser , AppRole , Guid>
    {
        public BaseCoreIdentityContext(DbContextOptions<BaseCoreIdentityContext> options) : base(options)
        {
        }
    }
}

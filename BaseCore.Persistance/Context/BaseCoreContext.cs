using BaseCore.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BaseCore.Persistance.Context
{
    public class BaseCoreContext : DbContext
    {
        public BaseCoreContext(DbContextOptions<BaseCoreContext> options) : base(options)
        {
            
        }


        public virtual DbSet<Product> Products { get; set; }    
    }
}

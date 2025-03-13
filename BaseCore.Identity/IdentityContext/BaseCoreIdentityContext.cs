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

        public DbSet<BlacklistToken> BlackListTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<BlacklistToken>()
                .HasKey(t => t.Id);

            modelBuilder.Entity<BlacklistToken>()
                .Property(t => t.Token)
                .IsRequired();

            modelBuilder.Entity<BlacklistToken>()
                .Property(t => t.ExpiryDate)
                .IsRequired();
        }
    }
}

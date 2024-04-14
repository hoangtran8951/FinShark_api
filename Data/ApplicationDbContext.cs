using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using apiSTockapi.Models;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace apiSTockapi.Data
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<StockModel> Stocks {get; set; }
        public DbSet<CommentModel> Comments {get; set; }
        public DbSet<PortfolioModel> Portfolios {get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<PortfolioModel>(x => x.HasKey(p => new {p.AppUserId, p.StockId}));
            builder.Entity<PortfolioModel>()
                   .HasOne(u => u.AppUser)
                   .WithMany(u => u.Portfolios)
                   .HasForeignKey(p => p.AppUserId);

            builder.Entity<PortfolioModel>()
                   .HasOne(s => s.Stock)
                   .WithMany(u => u.Portfolios)
                   .HasForeignKey(p => p.StockId);

            List<IdentityRole> roles = new List<IdentityRole>{
                new IdentityRole
                {
                    Name = "Admin",
                    NormalizedName = "ADMIN"
                },
                new IdentityRole
                {
                    Name = "User",
                    NormalizedName = "USER"
                }
            };
            builder.Entity<IdentityRole>().HasData(roles);

            // builder.Entity<AppUser>( b => b.Property(u => u.Email).RequireUniqueEmail = true);
        }
    }
}
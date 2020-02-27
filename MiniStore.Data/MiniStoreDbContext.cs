using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using MiniStore.Data.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace MiniStore.Data
{
    public class MiniStoreDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, long,
        IdentityUserClaim<long>, UserRole, IdentityUserLogin<long>,
        IdentityRoleClaim<long>, IdentityUserToken<long>>
    {
        public MiniStoreDbContext(DbContextOptions<MiniStoreDbContext> options) : base(options)
        {

        }

        public DbSet<ApplicationUser> ApplicationUser { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionBuilder)
        {
            base.OnConfiguring(optionBuilder);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<ApplicationUser>().ToTable("User");
            builder.Entity<ApplicationRole>().ToTable("Role");

            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.RoleId)
                .IsRequired();

                userRole.HasOne(ur => ur.User)
                .WithMany(r => r.UserRoles)
                .HasForeignKey(ur => ur.UserId)
                .IsRequired();
            });

        }
    }
}

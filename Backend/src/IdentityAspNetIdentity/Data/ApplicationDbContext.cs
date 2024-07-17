using IdentityAspNetIdentity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;

namespace IdentityAspNetIdentity.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // Identity Data Seed

            // Création des rôles
            var defaultRole = new IdentityRole
            {
                Id = "bd22a9e5-8fbd-46be-8258-1e273b910520",
                Name = "default",
                NormalizedName = "DEFAULT"
            };

            var adminRole = new IdentityRole
            {
                Id = "42effada-1b94-4c16-aa6f-382ac9778de5",
                Name = "admin",
                NormalizedName = "ADMIN"
            };

            var managerRole = new IdentityRole
            {
                Id = "7902b0ce-e716-441f-8fae-78eec8c29388",
                Name = "manager",
                NormalizedName = "MANAGER"
            };

            // Création des utilisateurs admin et manager
            var admin = new ApplicationUser
            {
                Id = "522c7a37-296d-45b2-a3d1-8c826731619e",
                UserName = "admin",
                Email = "admin@cegeplimoilou.ca",
                NormalizedUserName = "ADMIN",
                NormalizedEmail = "ADMIN@CEGEPLIMOILOU.CA",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            var manager = new ApplicationUser
            {
                Id = "b2fa2f6d-94f0-460d-b044-16ada7742e11",
                UserName = "manager",
                Email = "manager@email.com",
                NormalizedUserName = "MANAGER",
                NormalizedEmail = "MANAGER@EMAIL.COM",
                SecurityStamp = Guid.NewGuid().ToString()
            };

            // Hasher les mots de passe
            var hasher = new PasswordHasher<ApplicationUser>();
            admin.PasswordHash = hasher.HashPassword(admin, "Pass-123");
            manager.PasswordHash = hasher.HashPassword(manager, "Pass-123");

            // Persister les données dans la BD
            builder.Entity<IdentityRole>().HasData(defaultRole);
            builder.Entity<IdentityRole>().HasData(adminRole);
            builder.Entity<IdentityRole>().HasData(managerRole);

            builder.Entity<ApplicationUser>().HasData(admin);
            builder.Entity<ApplicationUser>().HasData(manager);

            // Associer les rôles aux usagers (admin et manager)
            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = admin.Id, RoleId = adminRole.Id });

            builder.Entity<IdentityUserRole<string>>().HasData(
                new IdentityUserRole<string> { UserId = manager.Id, RoleId = managerRole.Id });

            base.OnModelCreating(builder);
            // Customize the ASP.NET Identity model and override the defaults if needed.
            // For example, you can rename the ASP.NET Identity table names and more.
            // Add your customizations after calling base.OnModelCreating(builder);
        }
    }
}

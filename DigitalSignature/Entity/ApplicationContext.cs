using Entity.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Entity
{
    public class ApplicationContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public ApplicationContext(DbContextOptions<ApplicationContext> options)
            : base(options)
        {
            Database.EnsureDeleted();
            //Thread.Sleep(1000);
            //Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            string[] roles = new string[] { "write", "read" };
            Role[] Roles = new Role[roles.Length];
            for (int i = 0; i < Roles.Length; i++)
            {
                Roles[i] = new Role { Id = i + 1, Name = roles[i] };
            }

            string adminEmail = "admin";
            string adminPassword = "123456!";


            User adminUser = new User { Id = 1, Email = adminEmail, Password = adminPassword, RoleId = 1 };
            User userRead = new User { Id = 2, Email = "user", Password = "123!", RoleId = 2 };

            modelBuilder.Entity<Role>().HasData(Roles);
            modelBuilder.Entity<User>().HasData(new User[] { adminUser, userRead });
            base.OnModelCreating(modelBuilder);
        }
    }
}

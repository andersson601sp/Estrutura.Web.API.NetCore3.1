using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Estrutura.Web.API.Entities;

namespace Estrutura.Web.API.Data
{
    public class EstruturaContext : DbContext
    {
        public EstruturaContext(DbContextOptions<EstruturaContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            // carga inicial
            builder.Entity<User>()
                  .HasData(new List<User>(){
                    new User(1,"Anderson", "Oliveira", "aos", "123", Role.Admin, ""),
                    new User(2,"keli", "Silva", "keli", "123", Role.User, ""),
                    new User(3,"Samuel", "Claro", "samuel","123", Role.User, ""),
                  });

        }
    }
}
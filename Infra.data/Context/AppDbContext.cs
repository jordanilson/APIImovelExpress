using Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace Infra.data.Context
{
    public class AppDbContext : IdentityDbContext<ApplicationUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<Owners> Owners { get; set; }
        public DbSet<Properties> Properties { get; set; }
        public DbSet<Clients> Clients { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder mb)
        {

            mb.Entity<Owners>()
                .HasMany(u => u.Properties)
                .WithOne(p => p.Owners)
                .HasForeignKey(p => p.OwnersId);


            mb.Entity<Clients>()
               .HasMany(u => u.Reservations)
               .WithOne(p => p.Clients)
               .HasForeignKey(p => p.ClientsId);

            mb.Entity<Properties>().HasKey(p => p.Id);
            mb.Entity<Properties>().Property(p => p.Name).HasMaxLength(100).IsRequired();
            mb.Entity<Properties>().Property(p => p.Price).HasColumnType("decimal(12,2)").IsRequired();
            mb.Entity<Properties>().Property(p => p.Description).HasMaxLength(150).IsRequired();
            mb.Entity<Properties>().Property(p => p.Amenities).HasMaxLength(100).IsRequired();
            mb.Entity<Properties>().Property(p => p.State).HasMaxLength(100).IsRequired();
            mb.Entity<Properties>().Property(p => p.City).HasMaxLength(100).IsRequired();
            mb.Entity<Properties>().Property(p => p.ImgUrl).HasMaxLength(150).IsRequired();
            mb.Entity<Properties>().Property(p => p.Availability).IsRequired();

            mb.Entity<Reservation>().HasKey(p => p.Id);
            mb.Entity<Reservation>().Property(p => p.ReservationProperties).IsRequired();

            base.OnModelCreating(mb);
        }
    }
}

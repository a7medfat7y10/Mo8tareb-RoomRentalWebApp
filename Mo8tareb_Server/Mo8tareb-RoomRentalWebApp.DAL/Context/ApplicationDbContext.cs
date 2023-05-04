using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Mo8tareb_RoomRentalWebApp.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Mo8tareb_RoomRentalWebApp.DAL.Context
{
    public class ApplicationDbContext : IdentityDbContext<AppUser>
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Room>()
           .Property(r => r.Price)
           .HasColumnType("decimal(18,2)");
            base.OnModelCreating(builder);
        }


        public virtual DbSet<AppUser> AppUsers { get; set; }
        public virtual DbSet<Owner> Owners { get; set; }
        public virtual DbSet<Image> Images { get; set; }
        public virtual DbSet<Reservation> Reservations { get; set; }
        public virtual DbSet<Review> Reviews { get; set; }
        public virtual DbSet<Room> Rooms { get; set; }
        public virtual DbSet<Service> Services { get; set; }
        public virtual DbSet<Payment> payments { get; set; }
    }
}
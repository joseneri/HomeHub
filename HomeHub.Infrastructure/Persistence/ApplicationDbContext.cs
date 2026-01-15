using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HomeHub.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace HomeHub.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Community> Communities => Set<Community>();
        public DbSet<Home> Homes => Set<Home>();
        public DbSet<Amenity> Amenities => Set<Amenity>();
        public DbSet<HomeAmenity> HomeAmenities => Set<HomeAmenity>();
        public DbSet<Lead> Leads => Set<Lead>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Many-to-many Home <-> Amenity
            modelBuilder.Entity<HomeAmenity>()
                .HasKey(ha => new { ha.HomeId, ha.AmenityId });

            modelBuilder.Entity<HomeAmenity>()
                .HasOne(ha => ha.Home)
                .WithMany(h => h.HomeAmenities)
                .HasForeignKey(ha => ha.HomeId);

            modelBuilder.Entity<HomeAmenity>()
                .HasOne(ha => ha.Amenity)
                .WithMany(a => a.HomeAmenities)
                .HasForeignKey(ha => ha.AmenityId);

            // Community 1 - N Homes
            modelBuilder.Entity<Community>()
                .HasMany(c => c.Homes)
                .WithOne(h => h.Community)
                .HasForeignKey(h => h.CommunityId);

            // Community 1 - N Leads (optional)
            modelBuilder.Entity<Community>()
                .HasMany(c => c.Leads)
                .WithOne(l => l.Community)
                .HasForeignKey(l => l.CommunityId);

            // Home 1 - N Leads (optional)
            modelBuilder.Entity<Home>()
                .HasMany(h => h.Leads)
                .WithOne(l => l.Home)
                .HasForeignKey(l => l.HomeId);
        }
    }
}

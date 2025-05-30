using Microsoft.EntityFrameworkCore;
using PedalPricerServer.Models;

namespace PedalPricerServer.Db
{
    public class PedalPricerDbContext : DbContext
    {
        public DbSet<Pedal> Pedals { get; set; }
        public DbSet<Pedalboard> Pedalboards { get; set; }
        public DbSet<PowerSupply> PowerSupplies { get; set; }

        public PedalPricerDbContext(DbContextOptions<PedalPricerDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Pedal>()
                .HasKey(p => p.ID);

            modelBuilder.Entity<Pedalboard>()
                .HasKey(pb => pb.ID);

            modelBuilder.Entity<PowerSupply>()
                .HasKey(ps => ps.ID);
        }
    }
}

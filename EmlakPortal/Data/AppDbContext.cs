using EmlakPortal.Models;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace EmlakPortal.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Ev> Evler { get; set; }
        public DbSet<IlanTur> IlanTurleri { get; set; }
        public DbSet<IlanDurum> IlanDurumlari { get; set; }
        // Bu metod, EF Core'a veritabanını nasıl oluşturacağını söyler.
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Ev>()
                .Property(e => e.Fiyat)
                .HasPrecision(18, 2);
            base.OnModelCreating(modelBuilder);
        }
    }
}

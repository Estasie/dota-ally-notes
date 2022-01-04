using DotaNotes.DTO.Models;
using Microsoft.EntityFrameworkCore;

namespace DotaNotes.DTO.Context
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext() 
        {
            ChangeTracker.LazyLoadingEnabled = false;
        }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer("Server=DESKTOP-1H8EHBJ;Database=DotaNotes;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<DotaPlayer> DotaPlayers { get; set; }
    }
}
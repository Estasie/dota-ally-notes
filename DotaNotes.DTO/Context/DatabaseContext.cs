using System;
using DotaNotes.DTO.Models;
using Microsoft.EntityFrameworkCore;

namespace DotaNotes.DTO.Context
{
    public class DatabaseContext: DbContext
    {
        public DatabaseContext() 
        {
            ChangeTracker.LazyLoadingEnabled = false;
            Database.EnsureCreated();
        }

        
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var path = AppDomain.CurrentDomain.BaseDirectory;
            var DbPath = $"{path}dotanotes.db";
            optionsBuilder.UseSqlite($"Data Source={DbPath}");
            
            //optionsBuilder.UseSqlServer("Server=DESKTOP-1H8EHBJ;Database=DotaNotes;Trusted_Connection=True;MultipleActiveResultSets=true");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<DotaPlayer> DotaPlayers { get; set; }
    }
}
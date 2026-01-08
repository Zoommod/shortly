using System;
using Microsoft.EntityFrameworkCore;
using Shorty.Web.Models;

namespace Shorty.Web.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
        
    }
    public DbSet<UrlMappings> UrlMappings { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    { 
            modelBuilder.Entity<UrlMappings>()
            .HasIndex(u => u.Code)
            .IsUnique();
    }
}

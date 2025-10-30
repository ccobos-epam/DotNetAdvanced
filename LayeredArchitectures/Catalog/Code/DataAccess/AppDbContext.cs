using Microsoft.EntityFrameworkCore;
using Domain = DomainEntities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAccess;

public class AppDbContext : DbContext
{
    public DbSet<Domain.CategoryEntity> Categories { get; set; }
    public DbSet<Domain.ProductEntity> Products { get; set; }

    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (optionsBuilder.IsConfigured)
            return;
        //optionsBuilder.UseSqlServer(_connectionString);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AssemblyMarker).Assembly);
    }
}

using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TP_CAISSE.Models;

namespace TP_CAISSE.Context;

public partial class MyDbContext : DbContext
{
    public MyDbContext()
    {
    }

    public MyDbContext(DbContextOptions<MyDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Categorie> Categories { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        => optionsBuilder.UseSqlServer("Name=MyDbConnection");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categorie>(entity =>
        {
            entity.HasKey(e => e.Primarikey);

            entity.ToTable("CATEGORIE");

            entity.Property(e => e.Primarikey)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("PRIMARIKEY");
            entity.Property(e => e.Name).HasColumnName("NAME");
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasKey(e => e.Primarikey);

            entity.ToTable("PRODUCT");

            entity.Property(e => e.Primarikey)
                .HasDefaultValueSql("(newid())")
                .HasColumnName("PRIMARIKEY");
            entity.Property(e => e.Description).HasColumnName("DESCRIPTION");
            entity.Property(e => e.Image).HasColumnName("IMAGE");
            entity.Property(e => e.Name).HasColumnName("NAME");
            entity.Property(e => e.Price).HasColumnName("PRICE");
            entity.Property(e => e.Stock).HasColumnName("STOCK");

            entity.HasMany(d => d.FkCategories).WithMany(p => p.FkProducts)
                .UsingEntity<Dictionary<string, object>>(
                    "RProductCategorie",
                    r => r.HasOne<Categorie>().WithMany()
                        .HasForeignKey("FkCategorie")
                        .HasConstraintName("FK_R_PRODUCT_CATEGORIE_CATEGORIE"),
                    l => l.HasOne<Product>().WithMany()
                        .HasForeignKey("FkProduct")
                        .HasConstraintName("FK_R_PRODUCT_CATEGORIE_PRODUCT"),
                    j =>
                    {
                        j.HasKey("FkProduct", "FkCategorie");
                        j.ToTable("R_PRODUCT_CATEGORIE");
                        j.IndexerProperty<Guid>("FkProduct").HasColumnName("FK_PRODUCT");
                        j.IndexerProperty<Guid>("FkCategorie").HasColumnName("FK_CATEGORIE");
                    });
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

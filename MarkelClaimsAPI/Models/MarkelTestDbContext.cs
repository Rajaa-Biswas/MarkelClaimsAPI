using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace MarkelClaimsAPI.Models;

public partial class MarkelTestDbContext : DbContext
{
    public MarkelTestDbContext()
    {
    }

    public MarkelTestDbContext(DbContextOptions<MarkelTestDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Claim> Claims { get; set; }

    public virtual DbSet<ClaimType> ClaimTypes { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
            optionsBuilder.UseSqlServer("Data Source=LAPTOP-LK5IILQE;Initial Catalog=MarkelTestDB;Integrated Security=True; TrustServerCertificate=True");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Claim>(entity =>
        {
            entity.HasKey(e => e.Ucr);

            entity.Property(e => e.Ucr)
                .HasMaxLength(20)
                .IsUnicode(false)
                .HasColumnName("UCR");
            entity.Property(e => e.AssuredName)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("Assured Name");
            entity.Property(e => e.ClaimDate).HasColumnType("datetime");
            entity.Property(e => e.IncurredLoss)
                .HasColumnType("decimal(15, 2)")
                .HasColumnName("Incurred Loss");
            entity.Property(e => e.LossDate).HasColumnType("datetime");

            entity.HasOne(d => d.Company).WithMany(p => p.Claims)
                .HasForeignKey(d => d.CompanyId)
                .HasConstraintName("FK_Claims_Company");
        });

        modelBuilder.Entity<ClaimType>(entity =>
        {
            entity.ToTable("ClaimType");

            entity.Property(e => e.Name)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.ToTable("Company");

            entity.Property(e => e.Address1)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address2)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Address3)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Country)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.InsuranceEndDate).HasColumnType("datetime");
            entity.Property(e => e.Name)
                .HasMaxLength(200)
                .IsUnicode(false);
            entity.Property(e => e.Postcode)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

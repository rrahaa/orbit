using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using SmartRestaurant.Api.Models;

namespace SmartRestaurant.Api.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Artikel> Artikel { get; set; }

    public virtual DbSet<ArtikelKategorie> ArtikelKategorie { get; set; }

    public virtual DbSet<BestellStatus> BestellStatus { get; set; }

    public virtual DbSet<Bestellposition> Bestellposition { get; set; }

    public virtual DbSet<Bestellung> Bestellung { get; set; }

    public virtual DbSet<Mitarbeiter> Mitarbeiter { get; set; }

    public virtual DbSet<Rolle> Rolle { get; set; }

    public virtual DbSet<Tisch> Tisch { get; set; }

    public virtual DbSet<TischStatus> TischStatus { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("utf8mb4_0900_ai_ci")
            .HasCharSet("utf8mb4");

        modelBuilder.Entity<Artikel>(entity =>
        {
            entity.HasKey(e => e.ArtikelId).HasName("PRIMARY");

            entity
                .ToTable("artikel")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.KategorieId, "kategorie_id");

            entity.Property(e => e.ArtikelId).HasColumnName("artikel_id");
            entity.Property(e => e.KategorieId).HasColumnName("kategorie_id");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.Preis)
                .HasPrecision(10, 2)
                .HasColumnName("preis");

            entity.HasOne(d => d.Kategorie).WithMany(p => p.Artikel)
                .HasForeignKey(d => d.KategorieId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("artikel_ibfk_1");
        });

        modelBuilder.Entity<ArtikelKategorie>(entity =>
        {
            entity.HasKey(e => e.KategorieId).HasName("PRIMARY");

            entity
                .ToTable("artikel_kategorie")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.Kategoriename, "kategoriename").IsUnique();

            entity.Property(e => e.KategorieId).HasColumnName("kategorie_id");
            entity.Property(e => e.Kategoriename)
                .HasMaxLength(50)
                .HasColumnName("kategoriename");
        });

        modelBuilder.Entity<BestellStatus>(entity =>
        {
            entity.HasKey(e => e.BestellStatusId).HasName("PRIMARY");

            entity
                .ToTable("bestell_status")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.Statusname, "statusname").IsUnique();

            entity.Property(e => e.BestellStatusId).HasColumnName("bestell_status_id");
            entity.Property(e => e.Statusname)
                .HasMaxLength(30)
                .HasColumnName("statusname");
        });

        modelBuilder.Entity<Bestellposition>(entity =>
        {
            entity.HasKey(e => e.BestellpositionId).HasName("PRIMARY");

            entity
                .ToTable("bestellposition")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.ArtikelId, "artikel_id");

            entity.HasIndex(e => new { e.BestellungId, e.ArtikelId }, "bestellung_id").IsUnique();

            entity.Property(e => e.BestellpositionId).HasColumnName("bestellposition_id");
            entity.Property(e => e.ArtikelId).HasColumnName("artikel_id");
            entity.Property(e => e.BestellungId).HasColumnName("bestellung_id");
            entity.Property(e => e.Einzelpreis)
                .HasPrecision(10, 2)
                .HasColumnName("einzelpreis");
            entity.Property(e => e.Menge).HasColumnName("menge");

            entity.HasOne(d => d.Artikel).WithMany(p => p.Bestellposition)
                .HasForeignKey(d => d.ArtikelId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bestellposition_ibfk_2");

            entity.HasOne(d => d.Bestellung).WithMany(p => p.Bestellposition)
                .HasForeignKey(d => d.BestellungId)
                .HasConstraintName("bestellposition_ibfk_1");
        });

        modelBuilder.Entity<Bestellung>(entity =>
        {
            entity.HasKey(e => e.BestellungId).HasName("PRIMARY");

            entity
                .ToTable("bestellung")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.BestellStatusId, "bestell_status_id");

            entity.HasIndex(e => e.MitarbeiterId, "mitarbeiter_id");

            entity.HasIndex(e => e.TischId, "tisch_id");

            entity.Property(e => e.BestellungId).HasColumnName("bestellung_id");
            entity.Property(e => e.Abschlusszeitpunkt)
                .HasColumnType("datetime")
                .HasColumnName("abschlusszeitpunkt");
            entity.Property(e => e.BestellStatusId).HasColumnName("bestell_status_id");
            entity.Property(e => e.Bestelldatum)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("datetime")
                .HasColumnName("bestelldatum");
            entity.Property(e => e.MitarbeiterId).HasColumnName("mitarbeiter_id");
            entity.Property(e => e.TischId).HasColumnName("tisch_id");

            entity.HasOne(d => d.BestellStatus).WithMany(p => p.Bestellung)
                .HasForeignKey(d => d.BestellStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bestellung_ibfk_3");

            entity.HasOne(d => d.Mitarbeiter).WithMany(p => p.Bestellung)
                .HasForeignKey(d => d.MitarbeiterId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bestellung_ibfk_2");

            entity.HasOne(d => d.Tisch).WithMany(p => p.Bestellung)
                .HasForeignKey(d => d.TischId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("bestellung_ibfk_1");
        });

        modelBuilder.Entity<Mitarbeiter>(entity =>
        {
            entity.HasKey(e => e.MitarbeiterId).HasName("PRIMARY");

            entity
                .ToTable("mitarbeiter")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.Benutzername, "benutzername").IsUnique();

            entity.HasIndex(e => e.RolleId, "rolle_id");

            entity.Property(e => e.MitarbeiterId).HasColumnName("mitarbeiter_id");
            entity.Property(e => e.Benutzername)
                .HasMaxLength(50)
                .HasColumnName("benutzername");
            entity.Property(e => e.IstAktiv)
                .IsRequired()
                .HasDefaultValueSql("'1'")
                .HasColumnName("ist_aktiv");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
            entity.Property(e => e.PasswortHash)
                .HasMaxLength(255)
                .HasColumnName("passwort_hash");
            entity.Property(e => e.RolleId).HasColumnName("rolle_id");

            entity.HasOne(d => d.Rolle).WithMany(p => p.Mitarbeiter)
                .HasForeignKey(d => d.RolleId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("mitarbeiter_ibfk_1");
        });

        modelBuilder.Entity<Rolle>(entity =>
        {
            entity.HasKey(e => e.RolleId).HasName("PRIMARY");

            entity
                .ToTable("rolle")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.Rollenname, "rollenname").IsUnique();

            entity.Property(e => e.RolleId).HasColumnName("rolle_id");
            entity.Property(e => e.Rollenname)
                .HasMaxLength(50)
                .HasColumnName("rollenname");
        });

        modelBuilder.Entity<Tisch>(entity =>
        {
            entity.HasKey(e => e.TischId).HasName("PRIMARY");

            entity
                .ToTable("tisch")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.TischStatusId, "tisch_status_id");

            entity.HasIndex(e => e.Tischnummer, "tischnummer").IsUnique();

            entity.Property(e => e.TischId).HasColumnName("tisch_id");
            entity.Property(e => e.Kapazitaet).HasColumnName("kapazitaet");
            entity.Property(e => e.TischStatusId).HasColumnName("tisch_status_id");
            entity.Property(e => e.Tischnummer).HasColumnName("tischnummer");

            entity.HasOne(d => d.TischStatus).WithMany(p => p.Tisch)
                .HasForeignKey(d => d.TischStatusId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("tisch_ibfk_1");
        });

        modelBuilder.Entity<TischStatus>(entity =>
        {
            entity.HasKey(e => e.TischStatusId).HasName("PRIMARY");

            entity
                .ToTable("tisch_status")
                .UseCollation("utf8mb4_unicode_ci");

            entity.HasIndex(e => e.Statusname, "statusname").IsUnique();

            entity.Property(e => e.TischStatusId).HasColumnName("tisch_status_id");
            entity.Property(e => e.Statusname)
                .HasMaxLength(30)
                .HasColumnName("statusname");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

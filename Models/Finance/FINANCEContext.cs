using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace OSIET_Finance.Models.Finance;

public partial class FINANCEContext : DbContext
{

    public FINANCEContext(DbContextOptions<FINANCEContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Compte> Comptes { get; set; }

    public virtual DbSet<Imputation> Imputations { get; set; }

    public virtual DbSet<Journal> Journals { get; set; }

    public virtual DbSet<Solde> Soldes { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Compte>(entity =>
        {
            entity.HasKey(e => e.CompteId)
            .HasName("PK_COMPTE");

            entity.ToTable("FN_COMPTE");

            entity.Property(e => e.CompteId)
                .ValueGeneratedOnAdd()
                .HasColumnName("COMPTE_ID");
            entity.Property(e => e.NomCompte)
                .HasMaxLength(100)
                .HasColumnName("NOM_COMPTE");
            entity.Property(e => e.NumCompte)
                .HasMaxLength(100)
                .HasColumnName("NUM_COMPTE");
        });

        modelBuilder.Entity<Imputation>(entity =>
        {
            entity.HasKey(e => e.ImputationId).HasName("PK_IMPUTATION");

            entity.ToTable("FN_IMPUTATION");

            entity.Property(e => e.ImputationId)
                .ValueGeneratedOnAdd()
                .HasColumnName("IMPUTATION_ID");
            entity.Property(e => e.Description)
                .HasMaxLength(255)
                .HasColumnName("DESCRIPTION");
            entity.Property(e => e.Nom)
                .HasMaxLength(100)
                .HasColumnName("NOM");
            entity.Property(e => e.NumeroCompte).HasColumnName("NUMERO_COMPTE");
            entity.Property(e => e.Type)
                .HasMaxLength(100)
                .HasColumnName("TYPE");
        });

        modelBuilder.Entity<Journal>(entity =>
        {
            entity.HasKey(e => e.JournalId).HasName("PK_JOURNAL");

            entity.ToTable("FN_JOURNAL");

            entity.Property(e => e.JournalId)
                .ValueGeneratedOnAdd()
                .HasColumnName("JOURNAL_ID");
            entity.Property(e => e.CompteId).HasColumnName("COMPTE_ID");
            entity.Property(e => e.Concernee)
                .HasMaxLength(100)
                .IsFixedLength()
                .HasColumnName("CONCERNEE");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("DATE");
            entity.Property(e => e.IdSoldeApres).HasColumnName("ID_SOLDE_APRES");
            entity.Property(e => e.IdSoldeAvant).HasColumnName("ID_SOLDE_AVANT");
            entity.Property(e => e.ImputationId).HasColumnName("IMPUTATION_ID");
            entity.Property(e => e.Libelle)
                .HasMaxLength(100)
                .HasColumnName("LIBELLE");
            entity.Property(e => e.LocationFichier)
                .HasMaxLength(255)
                .IsFixedLength()
                .HasColumnName("LOCATION_FICHIER");
            entity.Property(e => e.Montant)
                .HasColumnType("double")
                .HasColumnName("MONTANT");
            entity.Property(e => e.PayerPar)
                .HasMaxLength(100)
                .HasColumnName("PAYER_PAR");
            entity.Property(e => e.Type)
                .HasMaxLength(50)
                .HasColumnName("TYPE");
            entity.Property(e => e.RefPiece)
                .HasMaxLength(100)
                .HasColumnName("REF_PIECE");
        });

        modelBuilder.Entity<Solde>(entity =>
        {
            entity.HasKey(e => e.SoldeId).HasName("PK_SOLDE");

            entity.ToTable("FN_SOLDE");

            entity.Property(e => e.SoldeId)
                .ValueGeneratedOnAdd()
                .HasColumnName("SOLDE_ID");
            entity.Property(e => e.CompteId).HasColumnName("COMPTE_ID");
            entity.Property(e => e.Date)
                .HasColumnType("datetime")
                .HasColumnName("DATE");
            entity.Property(e => e.Montant)
                .HasColumnType("double")
                .HasColumnName("MONTANT");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

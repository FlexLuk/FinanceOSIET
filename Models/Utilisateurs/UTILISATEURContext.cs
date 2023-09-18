using Microsoft.EntityFrameworkCore;

namespace OSIET_Finance.Models.Administration
{
    public partial class UTILISATEURContext : DbContext
    {

        public UTILISATEURContext(DbContextOptions<UTILISATEURContext> options)
            : base(options)
        {
        }

        public virtual DbSet<AvoirRole> AvoirRoles { get; set; } = null!;
        public virtual DbSet<Urole> Uroles { get; set; } = null!;
        public virtual DbSet<Utilisateur> Utilisateurs { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AvoirRole>(entity =>
            {
                entity.ToTable("avoir_role");

                entity.Property(e => e.Avoirroleid)
                     .HasColumnName("AVOIRROLEID");

                entity.Property(e => e.Roleid).HasColumnName("ROLEID");

                entity.Property(e => e.Utilisateurid).HasColumnName("UTILISATEURID");
            });

            modelBuilder.Entity<Urole>(entity =>
            {
                entity.HasKey(e => e.Roleid)
                    .HasName("PK_ROLE");

                entity.ToTable("urole");

                entity.Property(e => e.Roleid)
                    .HasColumnName("ROLEID");

                entity.Property(e => e.Rolename)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("ROLENAME")
                    .IsFixedLength();
            });

            modelBuilder.Entity<Utilisateur>(entity =>
            {
                entity.ToTable("utilisateur");

                entity.Property(e => e.Utilisateurid)
                    .HasColumnName("UTILISATEURID");

                entity.Property(e => e.Email)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("EMAIL")
                    .IsFixedLength();

                entity.Property(e => e.Fonction)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("FONCTION")
                    .IsFixedLength();

                entity.Property(e => e.Nom)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("NOM")
                    .IsFixedLength();

                entity.Property(e => e.Passwordhash)
                    .HasMaxLength(128)
                    .IsUnicode(false)
                    .HasColumnName("PASSWORDHASH");

                entity.Property(e => e.Prenom)
                    .HasMaxLength(255)
                    .IsUnicode(false)
                    .HasColumnName("PRENOM")
                    .IsFixedLength();

                entity.Property(e => e.Status)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("STATUS")
                    .IsFixedLength();

                entity.Property(e => e.Type)
                    .HasMaxLength(32)
                    .IsUnicode(false)
                    .HasColumnName("TYPE")
                    .IsFixedLength();
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

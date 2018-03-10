using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace ProfessionalPartnerships.Data.Models
{
    public partial class PartnershipsContext : DbContext
    {
        public virtual DbSet<Certifications> Certifications { get; set; }
        public virtual DbSet<CertificationTypes> CertificationTypes { get; set; }
        public virtual DbSet<Companies> Companies { get; set; }
        public virtual DbSet<Enrollments> Enrollments { get; set; }
        public virtual DbSet<EnrollmentStatuses> EnrollmentStatuses { get; set; }
        public virtual DbSet<Interests> Interests { get; set; }
        public virtual DbSet<ProfessionalReviews> ProfessionalReviews { get; set; }
        public virtual DbSet<Professionals> Professionals { get; set; }
        public virtual DbSet<ProfessionalSkills> ProfessionalSkills { get; set; }
        public virtual DbSet<Programs> Programs { get; set; }
        public virtual DbSet<ProgramTypes> ProgramTypes { get; set; }
        public virtual DbSet<Semesters> Semesters { get; set; }
        public virtual DbSet<Skills> Skills { get; set; }
        public virtual DbSet<StudentInterests> StudentInterests { get; set; }
        public virtual DbSet<StudentReviews> StudentReviews { get; set; }
        public virtual DbSet<Students> Students { get; set; }

        public PartnershipsContext(DbContextOptions<PartnershipsContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Certifications>(entity =>
            {
                entity.HasKey(e => e.CertificationId);

                entity.Property(e => e.Note)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.CertificationType)
                    .WithMany(p => p.Certifications)
                    .HasForeignKey(d => d.CertificationTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Certifications_CertificationTypes");

                entity.HasOne(d => d.Professional)
                    .WithMany(p => p.Certifications)
                    .HasForeignKey(d => d.ProfessionalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Certifications_Professionals");
            });

            modelBuilder.Entity<CertificationTypes>(entity =>
            {
                entity.HasKey(e => e.CertificationTypeId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Companies>(entity =>
            {
                entity.HasKey(e => e.CompanyId);

                entity.Property(e => e.Address1)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Address2)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.City)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.State)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Zip)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.HasOne(d => d.PrimaryProfessional)
                    .WithMany(p => p.Companies)
                    .HasForeignKey(d => d.PrimaryProfessionalId)
                    .HasConstraintName("FK_Companies_Companies");
            });

            modelBuilder.Entity<Enrollments>(entity =>
            {
                entity.HasKey(e => e.EnrollmentId);

                entity.Property(e => e.Note)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.EnrollmentStatus)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.EnrollmentStatusId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrollments_EnrollmentStatuses");

                entity.HasOne(d => d.Program)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.ProgramId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrollments_Programs");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.Enrollments)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Enrollments_Students");
            });

            modelBuilder.Entity<EnrollmentStatuses>(entity =>
            {
                entity.HasKey(e => e.EnrollmentStatusId);

                entity.Property(e => e.IsDisregardedInEnrollmentCount).HasDefaultValueSql("((1))");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Interests>(entity =>
            {
                entity.HasKey(e => e.InterestId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<ProfessionalReviews>(entity =>
            {
                entity.HasKey(e => e.ProfessionalReviewId);

                entity.Property(e => e.Note)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.Professional)
                    .WithMany(p => p.ProfessionalReviews)
                    .HasForeignKey(d => d.ProfessionalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProfessionalReviews_Professionals");

                entity.HasOne(d => d.Program)
                    .WithMany(p => p.ProfessionalReviews)
                    .HasForeignKey(d => d.ProgramId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProfessionalReviews_Programs");
            });

            modelBuilder.Entity<Professionals>(entity =>
            {
                entity.HasKey(e => e.ProfessionalId);

                entity.Property(e => e.EmailAddress)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.Phone)
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AspNetUserId)
                      .IsRequired()
                      .HasMaxLength(450)
                      .IsUnicode(true);

                entity.HasOne(d => d.Company)
                    .WithMany(p => p.Professionals)
                    .HasForeignKey(d => d.CompanyId)
                    .HasConstraintName("FK_Professionals_Companies");
            });

            modelBuilder.Entity<ProfessionalSkills>(entity =>
            {
                entity.HasKey(e => new { e.ProfessionalId, e.SkillId });

                entity.HasOne(d => d.Professional)
                    .WithMany(p => p.ProfessionalSkills)
                    .HasForeignKey(d => d.ProfessionalId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProfessionalSkills_Professionals");

                entity.HasOne(d => d.Skill)
                    .WithMany(p => p.ProfessionalSkills)
                    .HasForeignKey(d => d.SkillId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_ProfessionalSkills_Skills");
            });

            modelBuilder.Entity<Programs>(entity =>
            {
                entity.HasKey(e => e.ProgramId);

                //entity.Property(e => e.ProgramId).ValueGeneratedNever();

                entity.Property(e => e.Description)
                    .IsRequired()
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.HasOne(d => d.PointOfContactProfessional)
                    .WithMany(p => p.Programs)
                    .HasForeignKey(d => d.PointOfContactProfessionalId)
                    .HasConstraintName("FK_Programs_Professionals");

                entity.HasOne(d => d.ProgramType)
                    .WithMany(p => p.Programs)
                    .HasForeignKey(d => d.ProgramTypeId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Programs_ProgramTypes");

                entity.HasOne(d => d.Semester)
                    .WithMany(p => p.Programs)
                    .HasForeignKey(d => d.SemesterId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_Programs_Semesters");
            });

            modelBuilder.Entity<ProgramTypes>(entity =>
            {
                entity.HasKey(e => e.ProgramTypeId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Semesters>(entity =>
            {
                entity.HasKey(e => e.SemesterId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<Skills>(entity =>
            {
                entity.HasKey(e => e.SkillId);

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);
            });

            modelBuilder.Entity<StudentInterests>(entity =>
            {
                entity.HasKey(e => new { e.StudentId, e.InterestId });

                entity.HasOne(d => d.Interest)
                    .WithMany(p => p.StudentInterests)
                    .HasForeignKey(d => d.InterestId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentInterests_Interests");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentInterests)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentInterests_Students");
            });

            modelBuilder.Entity<StudentReviews>(entity =>
            {
                entity.HasKey(e => e.StudentReviewId);

                entity.Property(e => e.Note)
                    .HasMaxLength(500)
                    .IsUnicode(false);

                entity.Property(e => e.Stars).HasColumnType("decimal(18, 0)");

                entity.HasOne(d => d.Program)
                    .WithMany(p => p.StudentReviews)
                    .HasForeignKey(d => d.ProgramId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentReviews_Programs");

                entity.HasOne(d => d.Student)
                    .WithMany(p => p.StudentReviews)
                    .HasForeignKey(d => d.StudentId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("FK_StudentReviews_Students");
            });

            modelBuilder.Entity<Students>(entity =>
            {
                entity.HasKey(e => e.StudentId);

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(50)
                    .IsUnicode(false);

                entity.Property(e => e.AspNetUserId)
                   .HasMaxLength(450)
                   .IsUnicode(true);

            });
        }
    }
}

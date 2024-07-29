using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace StajYerApp_API.Models;

public partial class Db6761Context : DbContext
{
    public Db6761Context()
    {
    }

    public Db6761Context(DbContextOptions<Db6761Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Advertisement> Advertisements { get; set; }

    public virtual DbSet<Application> Applications { get; set; }

    public virtual DbSet<Certificate> Certificates { get; set; }

    public virtual DbSet<Company> Companies { get; set; }

    public virtual DbSet<Education> Educations { get; set; }

    public virtual DbSet<Experience> Experiences { get; set; }

    public virtual DbSet<Program> Programs { get; set; }

    public virtual DbSet<Project> Projects { get; set; }

    public virtual DbSet<Univercity> Univercities { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserForgotPassword> UserForgotPasswords { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=db6761.public.databaseasp.net; Database=db6761; User Id=db6761; Password=Nz3_9#aF@B5y; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;Connection Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Advertisement>(entity =>
        {
            entity.HasKey(e => e.AdvId);

            entity.Property(e => e.AdvAdress)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.AdvDepartment)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AdvDesc).IsUnicode(false);
            entity.Property(e => e.AdvExpirationDate)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.AdvTitle)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.AdvWorkType)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.Comp).WithMany(p => p.Advertisements)
                .HasForeignKey(d => d.CompId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Advertisements_Companies");
        });

        modelBuilder.Entity<Application>(entity =>
        {
            entity.HasKey(e => e.AppId);

            entity.Property(e => e.AppDate)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.Adv).WithMany(p => p.Applications)
                .HasForeignKey(d => d.AdvId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Applications_Advertisements");

            entity.HasOne(d => d.User).WithMany(p => p.Applications)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Applications_Users");
        });

        modelBuilder.Entity<Certificate>(entity =>
        {
            entity.HasKey(e => e.CertId);

            entity.Property(e => e.CerCompanyName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CertDate)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.CertDesc)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CertName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Certificates)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Certificates_Users");
        });

        modelBuilder.Entity<Company>(entity =>
        {
            entity.HasKey(e => e.CompId);

            entity.Property(e => e.ComLinkedin)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CompAdress)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CompContactMail)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CompDesc)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.CompFoundationYear)
                .HasMaxLength(4)
                .IsUnicode(false)
                .IsFixedLength();
            entity.Property(e => e.CompLogo)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CompName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.CompSektor)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.CompWebSite)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Education>(entity =>
        {
            entity.HasKey(e => e.EduId);

            entity.Property(e => e.EduDesc)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.EduFinishDate)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.EduSituation)
                .HasMaxLength(50)
                .IsUnicode(false);
            entity.Property(e => e.EduStartDate)
                .HasMaxLength(25)
                .IsUnicode(false);

            entity.HasOne(d => d.Prog).WithMany(p => p.Educations)
                .HasForeignKey(d => d.ProgId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Educations_Programs");

            entity.HasOne(d => d.Uni).WithMany(p => p.Educations)
                .HasForeignKey(d => d.UniId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Educations_Educations");

            entity.HasOne(d => d.User).WithMany(p => p.Educations)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Educations_Users");
        });

        modelBuilder.Entity<Experience>(entity =>
        {
            entity.HasKey(e => e.ExpId);

            entity.Property(e => e.ExpCity)
                .HasMaxLength(20)
                .IsUnicode(false);
            entity.Property(e => e.ExpCompanyName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ExpDesc)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ExpFinishDate)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ExpPosition)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ExpStartDate)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.ExpWorkType)
                .HasMaxLength(20)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Experiences)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Experiences_Users");
        });

        modelBuilder.Entity<Program>(entity =>
        {
            entity.HasKey(e => e.ProgId);

            entity.Property(e => e.ProgName)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProgType)
                .HasMaxLength(20)
                .IsUnicode(false);
        });

        modelBuilder.Entity<Project>(entity =>
        {
            entity.HasKey(e => e.ProId);

            entity.Property(e => e.ProDesc)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.ProGithub)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.ProName)
                .HasMaxLength(255)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.Projects)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_Projects_Users");
        });

        modelBuilder.Entity<Univercity>(entity =>
        {
            entity.HasKey(e => e.UniId);

            entity.Property(e => e.UniName)
                .HasMaxLength(255)
                .IsUnicode(false);
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Ubirthdate)
                .HasMaxLength(25)
                .IsUnicode(false);
            entity.Property(e => e.Ucv)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Udesc)
                .HasMaxLength(500)
                .IsUnicode(false);
            entity.Property(e => e.Uemail)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Ugithub)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Ulinkedin)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Uname)
                .HasMaxLength(100)
                .IsUnicode(false);
            entity.Property(e => e.Upassword).HasMaxLength(255);
            entity.Property(e => e.Uphone)
                .HasMaxLength(15)
                .IsUnicode(false);
            entity.Property(e => e.Uprofilephoto)
                .HasMaxLength(255)
                .IsUnicode(false);
            entity.Property(e => e.Usurname)
                .HasMaxLength(100)
                .IsUnicode(false);
        });

        modelBuilder.Entity<UserForgotPassword>(entity =>
        {
            entity.HasKey(e => e.ForgotId).HasName("PK__UserForgotPasswords");

            entity.Property(e => e.ExpirationTime).HasColumnType("datetime");
            entity.Property(e => e.VerifyCode)
                .HasMaxLength(6)
                .IsUnicode(false);

            entity.HasOne(d => d.User).WithMany(p => p.UserForgotPasswords)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_UserForgotPasswords_Users");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

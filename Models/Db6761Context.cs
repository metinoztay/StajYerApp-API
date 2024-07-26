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

    public virtual DbSet<TblTest> TblTests { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=db6761.public.databaseasp.net; Database=db6761; User Id=db6761; Password=Nz3_9#aF@B5y; Encrypt=True; TrustServerCertificate=True; MultipleActiveResultSets=True;Connection Timeout=30");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TblTest>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK__tbl_test");

            entity.ToTable("tbl_test");

            entity.Property(e => e.Id).ValueGeneratedNever();
            entity.Property(e => e.TestKolon)
                .HasMaxLength(50)
                .HasColumnName("testKolon");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

using System;
using System.Collections.Generic;
using BookMoney.ModelsTemp;
using Microsoft.EntityFrameworkCore;

namespace BookMoney.Data;

public partial class AppDbContextTemp : DbContext
{
    public AppDbContextTemp()
    {
    }

    public AppDbContextTemp(DbContextOptions<AppDbContextTemp> options)
        : base(options)
    {
    }

    public virtual DbSet<ConfirmSm> ConfirmSms { get; set; }

    public virtual DbSet<Login> Logins { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=BookMoney;Username=postgres;Password=123");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConfirmSm>(entity =>
        {
            entity.Property(e => e.DateCreate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.Login).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("confirm_sms_login_fk");
        });

        modelBuilder.Entity<Login>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("login_pk");

            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.IsActive).HasDefaultValue(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

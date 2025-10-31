using BookMoney.Models;
using Microsoft.EntityFrameworkCore;

namespace BookMoney.Data;

public partial class AppDbContext : DbContext
{
    public AppDbContext()
    {
    }

    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<LoginDBModel> Logins { get; set; }
    public virtual DbSet<ConfirmSmsDBModel> ConfirmSms { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConfirmSmsDBModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("confirm_sms_pk");
            entity.Property(e => e.DateCreate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

            entity.HasOne(d => d.Login).WithMany()
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("confirm_sms_login_fk");
        });

        modelBuilder.Entity<LoginDBModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("login_pk");
            entity.Property(e => e.DateCreate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.IsActive).HasDefaultValue(false);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

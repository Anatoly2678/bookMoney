using BookMoney.Models;
using BookMoney.Models.View;
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

    public virtual DbSet<ClientInfoDBModel> ClientInfos { get; set; }
    public virtual DbSet<ClientProfile> ClientProfiles { get; set; }
    public virtual DbSet<PendingClient> PendingClients { get; set; }


    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<ConfirmSmsDBModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("confirm_sms_pk");
            entity.Property(e => e.DateCreate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");

            //entity.HasOne(d => d.Login).WithMany()
            //    .OnDelete(DeleteBehavior.ClientSetNull)
            //    .HasConstraintName("confirm_sms_login_fk");
        });

        modelBuilder.Entity<LoginDBModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("login_pk");
            entity.Property(e => e.DateCreate).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.Id).HasDefaultValueSql("gen_random_uuid()");
            entity.Property(e => e.IsActive).HasDefaultValue(false);
        });

        modelBuilder.Entity<ClientInfoDBModel>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("info_pkey");

            entity.ToTable("info", "client", tb => tb.HasComment("Таблица для хранения информации о клиентах"));

            entity.Property(e => e.Id)
                .HasDefaultValueSql("gen_random_uuid()")
                .HasComment("UUID идентификатор клиента");
            entity.Property(e => e.AgreementAccepted).HasDefaultValue(false);
            entity.Property(e => e.CreatedAt).HasDefaultValueSql("CURRENT_TIMESTAMP");
            entity.Property(e => e.EmailVerified).HasDefaultValue(false);
            entity.Property(e => e.FirstName).HasComment("Имя клиента");
            entity.Property(e => e.LastName).HasComment("Фамилия клиента");
            entity.Property(e => e.MiddleName).HasComment("Отчество клиента");
            entity.Property(e => e.PassportNumber).IsFixedLength();
            entity.Property(e => e.PassportSeries).IsFixedLength();
            entity.Property(e => e.Status).HasDefaultValueSql("'pending'::character varying");

            entity.HasOne(d => d.Login).WithMany(p => p.Infos)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("info_login_fk");
        });

        // View Models
        modelBuilder.Entity<ClientProfile>(entity =>
        {
            entity.ToView("client_profiles", "client");

            entity.Property(e => e.PassportNumber).IsFixedLength();
            entity.Property(e => e.PassportSeries).IsFixedLength();
        });

        modelBuilder.Entity<PendingClient>(entity =>
        {
            entity.ToView("pending_clients", "client");
        });
        // View Models

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

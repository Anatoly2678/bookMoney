using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMoney.Models;

/// <summary>
/// Таблица для хранения информации о клиентах
/// </summary>
[Table("info", Schema = "client")]
[Index("CreatedAt", Name = "idx_clients_created_at")]
[Index("Email", Name = "idx_clients_email")]
[Index("LastName", "FirstName", "MiddleName", Name = "idx_clients_names")]
[Index("Snils", Name = "idx_clients_snils")]
[Index("Status", Name = "idx_clients_status")]
[Index("Email", Name = "info_email_key", IsUnique = true)]
[Index("PassportSeries", "PassportNumber", Name = "unique_passport", IsUnique = true)]
public partial class ClientInfoDBModel
{
    /// <summary>
    /// UUID идентификатор клиента
    /// </summary>
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    /// <summary>
    /// Фамилия клиента
    /// </summary>
    [Column("last_name")]
    [StringLength(100)]
    public string LastName { get; set; } = null!;

    /// <summary>
    /// Имя клиента
    /// </summary>
    [Column("first_name")]
    [StringLength(100)]
    public string FirstName { get; set; } = null!;

    /// <summary>
    /// Отчество клиента
    /// </summary>
    [Column("middle_name")]
    [StringLength(100)]
    public string MiddleName { get; set; } = null!;

    [Column("birth_date")]
    public DateOnly BirthDate { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string Email { get; set; } = null!;

    [Column("password_hash")]
    [StringLength(255)]
    public string PasswordHash { get; set; } = null!;

    [Column("photo_page_image")]
    public byte[]? PhotoPageImage { get; set; }

    [Column("selfie_with_passport_image")]
    public byte[]? SelfieWithPassportImage { get; set; }

    [Column("registration_page_image")]
    public byte[]? RegistrationPageImage { get; set; }

    [Column("passport_series")]
    [StringLength(4)]
    public string PassportSeries { get; set; } = null!;

    [Column("passport_number")]
    [StringLength(6)]
    public string PassportNumber { get; set; } = null!;

    [Column("issued_by")]
    public string IssuedBy { get; set; } = null!;

    [Column("issue_date")]
    public DateTime IssueDate { get; set; }

    [Column("department_code")]
    [StringLength(10)]
    public string DepartmentCode { get; set; } = null!;

    [Column("birth_place")]
    public string BirthPlace { get; set; } = null!;

    [Column("snils")]
    [StringLength(14)]
    public string Snils { get; set; } = null!;

    [Column("agreement_accepted")]
    public bool AgreementAccepted { get; set; }

    [Column("email_verified")]
    public bool EmailVerified { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string Status { get; set; } = null!;

    [Column("created_at")]
    public DateTime CreatedAt { get; set; }

    [Column("updated_at")]
    public DateTime UpdatedAt { get; set; }

    [Column("verified_at")]
    public DateTime? VerifiedAt { get; set; }

    [Column("login_id")]
    public Guid LoginId { get; set; }

    [ForeignKey("LoginId")]
    [InverseProperty("Infos")]
    public virtual LoginDBModel Login { get; set; } = null!;
}

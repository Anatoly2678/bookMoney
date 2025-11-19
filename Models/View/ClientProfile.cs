using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMoney.Models.View;

[Keyless]
public partial class ClientProfile
{
    [Column("id")]
    public Guid? Id { get; set; }

    [Column("last_name")]
    [StringLength(100)]
    public string? LastName { get; set; }

    [Column("first_name")]
    [StringLength(100)]
    public string? FirstName { get; set; }

    [Column("middle_name")]
    [StringLength(100)]
    public string? MiddleName { get; set; }

    [Column("birth_date")]
    public DateOnly? BirthDate { get; set; }

    [Column("email")]
    [StringLength(255)]
    public string? Email { get; set; }

    [Column("passport_series")]
    [StringLength(4)]
    public string? PassportSeries { get; set; }

    [Column("passport_number")]
    [StringLength(6)]
    public string? PassportNumber { get; set; }

    [Column("snils")]
    [StringLength(14)]
    public string? Snils { get; set; }

    [Column("status")]
    [StringLength(20)]
    public string? Status { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }

    [Column("verified_at")]
    public DateTime? VerifiedAt { get; set; }
}

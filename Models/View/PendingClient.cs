using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMoney.Models.View;

[Keyless]
public partial class PendingClient
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

    [Column("email")]
    [StringLength(255)]
    public string? Email { get; set; }

    [Column("created_at")]
    public DateTime? CreatedAt { get; set; }
}

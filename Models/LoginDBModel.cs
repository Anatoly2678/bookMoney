using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMoney.Models;

[Table("login", Schema = "client")]
[Index("Login", Name = "login_unique", IsUnique = true)]
public partial class LoginDBModel
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("login", TypeName = "character varying")]
    public string Login { get; set; } = null!;

    [Column("password", TypeName = "character varying")]
    public string? Password { get; set; }

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("date_create")]
    public DateTime DateCreate { get; set; }

    [InverseProperty("Login")]
    public virtual ICollection<ConfirmSmsDBModel> ConfirmSms { get; set; } = new List<ConfirmSmsDBModel>();

    [InverseProperty("Login")]
    public virtual ICollection<ClientInfoDBModel> Infos { get; set; } = new List<ClientInfoDBModel>();
}
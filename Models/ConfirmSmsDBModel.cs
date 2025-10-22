using BookMoney.ModelsTemp;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace BookMoney.Models;

[Keyless]
[Table("confirm_sms", Schema = "client")]
public partial class ConfirmSmsDBModel
{
    [Column("id")]
    public Guid Id { get; set; }

    [Column("sms_code", TypeName = "character varying")]
    public string SmsCode { get; set; } = null!;

    [Column("date_create", TypeName = "timestamp without time zone")]
    public DateTime DateCreate { get; set; }

    [Column("date_confirm", TypeName = "timestamp without time zone")]
    public DateTime? DateConfirm { get; set; }

    [Column("login_id")]
    public Guid LoginId { get; set; }

    [ForeignKey("LoginId")]
    public virtual Login Login { get; set; } = null!;
}
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace BookMoney.ModelsTemp;

[Table("login", Schema = "client")]
[Index("Login1", Name = "login_unique", IsUnique = true)]
public partial class Login
{
    [Key]
    [Column("id")]
    public Guid Id { get; set; }

    [Column("login", TypeName = "character varying")]
    public string Login1 { get; set; } = null!;

    [Column("password", TypeName = "character varying")]
    public string Password { get; set; } = null!;

    [Column("is_active")]
    public bool IsActive { get; set; }

    [Column("date_create")]
    public DateTime DateCreate { get; set; }
}

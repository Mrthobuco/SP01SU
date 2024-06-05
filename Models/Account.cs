using System;
using System.Collections.Generic;

namespace SP01.Models;

public partial class Account 
{
    public int Id { get; set; }

    public string Username { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Address { get; set; } = null!;

    public DateOnly BirthDay { get; set; }

    public DateOnly CreateAt { get; set; }

    public DateOnly UpdateAt { get; set; }

    public int RoleId { get; set; }
}

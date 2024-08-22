using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class CompanyUser
{
    public int CompUserId { get; set; }

    public string NameSurname { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string TaxNumber { get; set; } = null!;

    public int TaxCityId { get; set; }

    public int TaxOfficeId { get; set; }

    public bool IsVerified { get; set; }

    public bool HasSetPassword { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<CompanyUserForgotPassword> CompanyUserForgotPasswords { get; set; } = new List<CompanyUserForgotPassword>();
}

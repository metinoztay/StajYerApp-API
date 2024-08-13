using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class Company
{
    public int CompId { get; set; }

    public string CompName { get; set; } = null!;

    public string CompFoundationYear { get; set; } = null!;

    public string CompWebSite { get; set; } = null!;

    public string CompContactMail { get; set; } = null!;

    public string CompAdress { get; set; } = null!;

    public string CompAddressTitle { get; set; } = null!;

    public string CompSektor { get; set; } = null!;

    public string CompDesc { get; set; } = null!;

    public string CompLogo { get; set; } = null!;

    public string ComLinkedin { get; set; } = null!;

    public int CompEmployeeCount { get; set; }

    public int CompUserId { get; set; }

    public virtual ICollection<Advertisement> Advertisements { get; set; } = new List<Advertisement>();

    public virtual CompanyUser CompUser { get; set; } = null!;

    public virtual ICollection<UserFollowedCompany> UserFollowedCompanies { get; set; } = new List<UserFollowedCompany>();
}

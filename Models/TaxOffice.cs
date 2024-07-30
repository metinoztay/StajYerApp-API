using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class TaxOffice
{
    public int TaxOfficeId { get; set; }

    public int CityId { get; set; }

    public string City { get; set; } = null!;

    public string District { get; set; } = null!;

    public string Office { get; set; } = null!;

    public virtual City CityNavigation { get; set; } = null!;

    public virtual ICollection<CompanyUser> CompanyUsers { get; set; } = new List<CompanyUser>();
}

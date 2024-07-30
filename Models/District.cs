using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class District
{
    public int DistId { get; set; }

    public string? DistName { get; set; }

    public int CityId { get; set; }

    public virtual City City { get; set; } = null!;
}

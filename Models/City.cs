﻿using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class City
{
    public int CityId { get; set; }

    public string? CityName { get; set; }

    public virtual ICollection<District> Districts { get; set; } = new List<District>();

    public virtual ICollection<Experience> Experiences { get; set; } = new List<Experience>();

    public virtual ICollection<TaxOffice> TaxOffices { get; set; } = new List<TaxOffice>();
}

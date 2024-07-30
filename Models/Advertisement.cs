using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class Advertisement
{
    public int AdvertId { get; set; }

    public int CompId { get; set; }

    public string AdvTitle { get; set; } = null!;

    public string AdvAdress { get; set; } = null!;

    public string AdvWorkType { get; set; } = null!;

    public string AdvDepartment { get; set; } = null!;

    public string AdvDesc { get; set; } = null!;

    public DateTime AdvExpirationDate { get; set; }

    public bool AdvIsActive { get; set; }

    public string? AdvPhoto { get; set; }

    public string AdvAdressTitle { get; set; } = null!;

    public bool? AdvPaymentInfo { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual Company Comp { get; set; } = null!;

    public virtual ICollection<UsersSavedAdvert> UsersSavedAdverts { get; set; } = new List<UsersSavedAdvert>();
}

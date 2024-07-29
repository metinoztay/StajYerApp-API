using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class UsersSavedAdvert
{
    public int SavedAdvertId { get; set; }

    public int UserId { get; set; }

    public int AdvertId { get; set; }

    public virtual Advertisement Advert { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

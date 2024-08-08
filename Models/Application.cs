using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class Application
{
    public int AppId { get; set; }

    public int UserId { get; set; }

    public int AdvertId { get; set; }

    public string AppDate { get; set; } = null!;

    public string AppLetter { get; set; } = null!;

    public virtual Advertisement Advert { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

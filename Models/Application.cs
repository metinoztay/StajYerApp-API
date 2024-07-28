using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class Application
{
    public int AppId { get; set; }

    public int UserId { get; set; }

    public int AdvId { get; set; }

    public DateTime AppDate { get; set; }

    public virtual Advertisement Adv { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class Project
{
    public int ProId { get; set; }

    public int UserId { get; set; }

    public string ProName { get; set; } = null!;

    public string? ProDesc { get; set; }

    public string ProGithub { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class UserFollowedCompany
{
    public int FollowId { get; set; }

    public int UserId { get; set; }

    public int CompId { get; set; }

    public virtual Company Comp { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

﻿using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class Admin
{
    public int AdminId { get; set; }

    public string AdminEmail { get; set; } = null!;

    public string AdminPassword { get; set; } = null!;
}

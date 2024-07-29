using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class Experience
{
    public int ExpId { get; set; }

    public int UserId { get; set; }

    public string ExpPosition { get; set; } = null!;

    public string ExpCompanyName { get; set; } = null!;

    public string ExpCity { get; set; } = null!;

    public string ExpStartDate { get; set; } = null!;

    public string? ExpFinishDate { get; set; }

    public string ExpWorkType { get; set; } = null!;

    public string? ExpDesc { get; set; }

    public virtual User User { get; set; } = null!;
}

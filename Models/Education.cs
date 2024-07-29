using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class Education
{
    public int EduId { get; set; }

    public int UserId { get; set; }

    public int UniId { get; set; }

    public int ProgId { get; set; }

    public string EduStartDate { get; set; } = null!;

    public string? EduFinishDate { get; set; }

    public double EduGano { get; set; }

    public string EduSituation { get; set; } = null!;

    public string? EduDesc { get; set; }

    public virtual Program Prog { get; set; } = null!;

    public virtual Univercity Uni { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}

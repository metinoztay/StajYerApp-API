using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class Univercity
{
    public int UniId { get; set; }

    public string UniName { get; set; } = null!;

    public virtual ICollection<Education> Educations { get; set; } = new List<Education>();
}

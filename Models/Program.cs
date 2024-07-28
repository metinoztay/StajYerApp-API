using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class Program
{
    public int ProgId { get; set; }

    public string ProgName { get; set; } = null!;

    public string ProgType { get; set; } = null!;

    public virtual ICollection<Education> Educations { get; set; } = new List<Education>();
}

using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class Certificate
{
    public int CertId { get; set; }

    public int UserId { get; set; }

    public string CertName { get; set; } = null!;

    public string CerCompanyName { get; set; } = null!;

    public string? CertDesc { get; set; }

    public DateTime CertDate { get; set; }

    public virtual User User { get; set; } = null!;
}

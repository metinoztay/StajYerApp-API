using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class VergiDaireleri
{
    public int Id { get; set; }

    public string? Plaka { get; set; }

    public string? Il { get; set; }

    public string? Ilce { get; set; }

    public string? Daire { get; set; }
}

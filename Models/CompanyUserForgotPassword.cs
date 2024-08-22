using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class CompanyUserForgotPassword
{
    public int ForgotId { get; set; }

    public int CompUserId { get; set; }

    public string VerifyCode { get; set; } = null!;

    public DateTime ExpirationTime { get; set; }

    public virtual CompanyUser CompUser { get; set; } = null!;
}

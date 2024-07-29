using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class UserForgotPassword
{
    public int ForgotId { get; set; }

    public int UserId { get; set; }

    public string VerifyCode { get; set; } = null!;

    public DateTime ExpirationTime { get; set; }

    public virtual User User { get; set; } = null!;
}

using System;
using System.Collections.Generic;

namespace StajYerApp_API.Models;

public partial class User
{
    public int UserId { get; set; }

    public string Uname { get; set; } = null!;

    public string Usurname { get; set; } = null!;

    public string Uemail { get; set; } = null!;

    public string Upassword { get; set; } = null!;

    public string Uphone { get; set; } = null!;

    public string Ubirthdate { get; set; } = null!;

    public bool Ugender { get; set; }

    public string? Ulinkedin { get; set; }

    public string? Ucv { get; set; }

    public string? Ugithub { get; set; }

    public string? Udesc { get; set; }

    public string Uprofilephoto { get; set; } = null!;

    public bool Uisactive { get; set; }

    public bool UisEmailVerified { get; set; }

    public bool UisPhoneVerified { get; set; }

    public virtual ICollection<Application> Applications { get; set; } = new List<Application>();

    public virtual ICollection<Certificate> Certificates { get; set; } = new List<Certificate>();

    public virtual ICollection<Education> Educations { get; set; } = new List<Education>();

    public virtual ICollection<Experience> Experiences { get; set; } = new List<Experience>();

    public virtual ICollection<Project> Projects { get; set; } = new List<Project>();

    public virtual ICollection<UserForgotPassword> UserForgotPasswords { get; set; } = new List<UserForgotPassword>();
}

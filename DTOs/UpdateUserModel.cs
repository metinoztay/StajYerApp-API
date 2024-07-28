namespace StajYerApp_API.DTOs
{
    public class UpdateUserModel
    {
        public int UserId { get; set; }
        public string Uname { get; set; } = null!;

        public string Usurname { get; set; } = null!;

        public string Uemail { get; set; } = null!;

        public string Upassword { get; set; } = null!;

        public string Uphone { get; set; } = null!;

        public DateTime Ubirthdate { get; set; }

        public string Ugender { get; set; } = null!;

        public string? Ulinkedin { get; set; }

        public string? Ucv { get; set; }

        public string? Ugithub { get; set; }

        public string? Udesc { get; set; }

        public string Uprofilephoto { get; set; } = null!;

        public bool Uisactive {get; set;}
    }
}

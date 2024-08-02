using StajYerApp_API.Models;

namespace StajYerApp_API.DTOs
{
    public class NewUserModel
    {
        public string Uname { get; set; } = null!;

        public string Usurname { get; set; } = null!;

        public string? Uemail { get; set; }

        public string Upassword { get; set; } = null!;

        public string? Uphone { get; set; }

        public string Ubirthdate { get; set; } = null!;

        public bool Ugender { get; set; }

    }
}

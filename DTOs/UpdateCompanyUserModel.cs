using StajYerApp_API.Models;

namespace StajYerApp_API.DTOs
{
    public class UpdateCompanyUserModel
    {
        public int CompUserId { get; set; }

        public string NameSurname { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
    }
}

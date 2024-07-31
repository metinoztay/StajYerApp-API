using StajYerApp_API.Models;

namespace StajYerApp_API.DTOs
{
    public class NewCompanyUserModel
    {
        public string NameSurname { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string TaxNumber { get; set; } = null!;

        public int TaxCityId { get; set; }

        public int TaxOfficeId { get; set; }
    }
}

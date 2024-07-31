using StajYerApp_API.Models;

namespace StajYerApp_API.DTOs
{
    public class NewAdvertModel
    {
        public int CompId { get; set; }

        public string AdvTitle { get; set; } = null!;

        public string AdvAdressTitle { get; set; } = null!;

        public string AdvAdress { get; set; } = null!;

        public string AdvWorkType { get; set; } = null!;

        public string AdvDepartment { get; set; } = null!;

        public string AdvJobDesc { get; set; } = null!;

        public string AdvQualifications { get; set; } = null!;

        public string AdvAddInformation { get; set; } = null!;

        public DateTime AdvExpirationDate { get; set; }

        public string? AdvPhoto { get; set; }        

        public bool? AdvPaymentInfo { get; set; }

    }
}

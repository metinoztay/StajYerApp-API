using StajYerApp_API.Models;

namespace StajYerApp_API.DTOs
{
    public class NewExperienceModel
    {
        public int UserId { get; set; }

        public string ExpPosition { get; set; } = null!;

        public string ExpCompanyName { get; set; } = null!;

        public int ExpCityId { get; set; }

        public string ExpStartDate { get; set; } = null!;

        public string? ExpFinishDate { get; set; }

        public string ExpWorkType { get; set; } = null!;

        public string? ExpDesc { get; set; }

    }
}

using StajYerApp_API.Models;

namespace StajYerApp_API.DTOs
{
    public class NewEducationModel
    {
        public int UserId { get; set; }

        public int UniId { get; set; }

        public int ProgId { get; set; }

        public string EduStartDate { get; set; } = null!;

        public string? EduFinishDate { get; set; }

        public double EduGano { get; set; }

        public string EduSituation { get; set; } = null!;

        public string? EduDesc { get; set; }

    }
}

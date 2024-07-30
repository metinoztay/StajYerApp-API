namespace StajYerApp_API.DTOs
{
    public class ProjectsModel
    {
        public int ProId { get; set; }

        public int UserId { get; set; }

        public string ProName { get; set; } = null!;

        public string? ProDesc { get; set; }

        public string ProGithub { get; set; } = null!;
    }
}

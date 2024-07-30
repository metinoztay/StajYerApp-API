namespace StajYerApp_API.DTOs
{
    public class CertificatesModel
    {
        public int CertId { get; set; }
        public int UserId { get; set; }
        public string CertName { get; set; } = null!;
        public string CerCompanyName { get; set; } = null!;
        public string? CertDesc { get; set; }
        public string CertDate { get; set; } = null!;
    }
}

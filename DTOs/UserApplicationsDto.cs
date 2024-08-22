
namespace StajYerApp_API.DTOs
{    
	//listelere ait dtoları ayırmadım. isteğe bağlı olarak ayırabilirim.
	public class UserApplicationsDto
	{
		public int UserId { get; set; }
		public string Uname { get; set; } = null!;
		public string Usurname { get; set; } = null!;
		public string Uemail { get; set; } = null!;
		public string? Uphone { get; set; }
		public string Ubirthdate { get; set; } = null!;
		public bool Ugender { get; set; }
		public string? Ulinkedin { get; set; }
		public string? Ucv { get; set; }
		public string? Ugithub { get; set; }
		public string? Udesc { get; set; }
		public string Uprofilephoto { get; set; } = null!;
		public List<CertificateDto> Certificates { get; set; } = new List<CertificateDto>();
		public List<EducationDto> Educations { get; set; } = new List<EducationDto>();
		public List<ExperienceDto> Experiences { get; set; } = new List<ExperienceDto>();
		public List<ProjectDto> Projects { get; set; } = new List<ProjectDto>();
		public List<ApplicationDto> Applications { get; set; } = new List<ApplicationDto>();
	}

	public class CertificateDto
	{
		public int CertId { get; set; }
		public string CertName { get; set; } = null!;
		public string CerCompanyName { get; set; } = null!;
		public string? CertDesc { get; set; }
		public string CertDate { get; set; } = null!;
	}

	public class EducationDto
	{
		public int EduId { get; set; }
		public int UniId { get; set; }
		public int ProgId { get; set; }
		public string EduStartDate { get; set; } = null!;
		public string? EduFinishDate { get; set; }
		public double EduGano { get; set; }
		public string EduSituation { get; set; } = null!;
		public string? EduDesc { get; set; }
	}

	public class ExperienceDto
	{
		public int ExpId { get; set; }
		public string ExpPosition { get; set; } = null!;
		public string ExpCompanyName { get; set; } = null!;
		public int ExpCityId { get; set; }
		public string ExpStartDate { get; set; } = null!;
		public string? ExpFinishDate { get; set; }
		public string ExpWorkType { get; set; } = null!;
		public string? ExpDesc { get; set; }
	}

	public class ProjectDto
	{
		public int ProId { get; set; }
		public string ProName { get; set; } = null!;
		public string? ProDesc { get; set; }
		public string ProGithub { get; set; } = null!;
	}

	public class ApplicationDto
	{
		public int AppId { get; set; }
		public int AdvertId { get; set; }
		public string AppDate { get; set; } = null!;
		public string AppLetter { get; set; } = null!;
		public string AdvertName { get; set; } = null!;
		public string CompName { get; set; } = null!;
	}
}

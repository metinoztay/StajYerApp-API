namespace StajYerApp_API.DTOs
{
    public class compUserLoginModel
    {
        public string Email { get; set; } = null!;

        public string Password { get; set; } = null!;
		public bool IsVerified { get; set; }

	}
}

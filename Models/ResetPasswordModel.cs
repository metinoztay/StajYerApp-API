namespace StajYerApp_API.Models
{
    public class ResetPasswordModel
    {
        public int UserId { get; set; }
        public string Code { get; set; }
        public string NewPassword { get; set; }
    }
}

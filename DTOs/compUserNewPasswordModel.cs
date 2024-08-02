namespace StajYerApp_API.DTOs
{
    public class compUserNewPasswordModel
    {
        public int CompUserId { get; set; }
        public string OldPassword { get; set; } = null!;
        public string NewPassword { get; set; } = null!;
    }
}

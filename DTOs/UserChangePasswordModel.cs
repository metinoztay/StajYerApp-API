namespace StajYerApp_API.DTOs
{
    public class UserChangePasswordModel
    {
        public int UserId { get; set; }
        public string oldPassword { get; set; } = null!;
        public string newPassword { get; set; } = null!;
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;
using StajYerApp_API.Models;

namespace StajYerApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        private readonly Db6761Context _context;
        private readonly IWebHostEnvironment _env;

        public PhotoController(Db6761Context context,IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        [HttpPost("UploadUserPhoto/{userId}")]
        public async Task<IActionResult> UploadUserPhoto(int userId, [FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "UserProfilePhotos");

            // Klasör var mı kontrol et ve oluştur
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }
            var user = await _context.Users.FindAsync(userId);
            string fileName = file.FileName;
            string extension = Path.GetExtension(fileName);
            var filePath = Path.Combine(uploadsFolderPath, user.UserId.ToString()+extension);

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            
            var fileUrl = $"{Request.Scheme}://{Request.Host}/UserProfilePhotos/{user.UserId + extension}";

            
            user.Uprofilephoto = fileUrl;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { fileUrl });
        }
    }
}

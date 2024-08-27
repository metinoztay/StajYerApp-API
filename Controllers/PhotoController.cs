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

        #region User Photo Upload
        [HttpPost("UploadUserPhoto/{userId}")]
        public async Task<IActionResult> UploadUserPhoto(int userId, [FromForm] IFormFile file)
        {
            //Var olan photo silme eklenebilir..

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Photos", "UserProfilePhotos");

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


            var fileUrl = $"{Request.Scheme}://{Request.Host}/Photos/UserProfilePhotos/{user.UserId + extension}";


            user.Uprofilephoto = fileUrl;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { fileUrl });
        }
        #endregion

        #region Company Logo Upload
        [HttpPost("UploadCompanyLogo/{CompId}")]
        public async Task<IActionResult> UploadCompanyLogo(int CompId, [FromForm] IFormFile file)
        { 
            //Var olan photo silme eklenebilir..

            if (file == null || file.Length == 0)
                return BadRequest("No file uploaded");

            var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(),"Photos", "CompanyLogos");

            // Klasör var mı kontrol et ve oluştur
            if (!Directory.Exists(uploadsFolderPath))
            {
                Directory.CreateDirectory(uploadsFolderPath);
            }
            var comp = await _context.Companies.FindAsync(CompId);
            string fileName = file.FileName;
            string extension = Path.GetExtension(fileName);
            var filePath = Path.Combine(uploadsFolderPath, comp.CompId.ToString() + extension);           

            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }
            var fileUrl = $"{Request.Scheme}://{Request.Host}/Photos/CompanyLogos/{comp.CompId + extension}";


            comp.CompLogo = fileUrl;

            _context.Entry(comp).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return Ok(new { fileUrl });
        }
		#endregion

		#region Advert Photo Upload
		[HttpPost("UploadAdvertPhoto")]
		public async Task<IActionResult> UploadAdvertPhoto([FromForm] IFormFile file)
		{
			try
			{
				if (file == null || file.Length == 0)
					return BadRequest("No file uploaded");

				var uploadsFolderPath = Path.Combine(Directory.GetCurrentDirectory(), "Photos", "AdvertPhotos");

				if (!Directory.Exists(uploadsFolderPath))
				{
					Directory.CreateDirectory(uploadsFolderPath);
				}

				int id = await _context.Advertisements.AnyAsync()
					? await _context.Advertisements.MaxAsync(x => x.AdvertId) + 1
					: 1; // Eğer hiç kayıt yoksa, ID'yi 1 olarak ayarlayın

				string fileName = file.FileName;
				string extension = Path.GetExtension(fileName);
				var filePath = Path.Combine(uploadsFolderPath, id.ToString() + fileName.Substring(0, 5) + extension);

				using (var stream = new FileStream(filePath, FileMode.Create))
				{
					await file.CopyToAsync(stream);
				}

				var fileUrl = $"{Request.Scheme}://{Request.Host}/Photos/AdvertPhotos/{id.ToString() + fileName.Substring(0, 5) + extension}";
				return Ok(new { fileUrl });
			}
			catch (Exception ex)
			{
				// Hata mesajını döndürün
				return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
			}
		}

		#endregion

	}
}

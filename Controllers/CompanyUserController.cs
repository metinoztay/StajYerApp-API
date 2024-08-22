using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajYerApp_API.DTOs;
using StajYerApp_API.Models;
using StajYerApp_API.Services;

namespace StajYerApp_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class CompanyUserController : ControllerBase
	{
		private readonly Db6761Context _context;
		private readonly IEmailService _emailService;

		/// <summary>
		/// UserController Db6761Context ile başlatır
		/// </summary>
		/// <param name="context">Uygulamanın veritabanı bağlantısı yapılıyor</param>
		public CompanyUserController(Db6761Context context, IEmailService emailService)
		{
			_context = context;
			_emailService = emailService;
		}

		#region Şirket Kullanıcısı Kayıt
		/// <summary>
		/// Kullanıcı kaydeder
		/// </summary>
		/// <param name="newUser">Kaydedilecek kullanıcı</param>
		/// <returns>Oluşturulan kullanıcıyı döndürür</returns>
		[HttpPost("Register")]
		public async Task<ActionResult<CompanyUser>> Register([FromBody] NewCompanyUserModel newUser)
		{

			var existingUser = await _context.CompanyUsers
				.FirstOrDefaultAsync(u => u.Email == newUser.Email.ToLower() || u.Phone == newUser.Phone);

			if (existingUser != null)
			{
				return BadRequest("Bu email veya telefon numarası ile daha önce kayıt oluşturulmuş.");
			}

			var addedUser = new CompanyUser
			{
				NameSurname = newUser.NameSurname,
				Email = newUser.Email.ToLower(),
				Phone = newUser.Phone,
				Password = Utilities.GenerateRandomPassword(8),
				TaxNumber = newUser.TaxNumber,
				TaxCityId = newUser.TaxCityId,
				TaxOfficeId = newUser.TaxOfficeId,
				IsVerified = false,
				HasSetPassword = false
			};

			_context.CompanyUsers.Add(addedUser);
			await _context.SaveChangesAsync();
			return Ok(addedUser);
		}
		#endregion

		#region Şirket Kullanıcısı Giriş
		/// <summary>
		/// Kullanıcı girişi
		/// </summary>
		/// <param name="loginUser">Kullanıcı adı ve şifre içeren giriş modeli</param>
		/// <returns>Kimliği doğrulanmış kullanıcıyı döndürür</returns>
		[HttpPost("Login")]
		public async Task<ActionResult<CompanyUser>> Login([FromBody] compUserLoginModel loginUser)
		{
			var user = await _context.CompanyUsers
				.FirstOrDefaultAsync(u => u.Email == loginUser.Email && u.Password == loginUser.Password);

			if (user == null)
			{
				return Unauthorized();
			}

			return Ok(user);
		}
		#endregion

		#region Yeni Şifre Belirleme
		/// <summary>
		/// Kullanıcının yeni şifre belirlemesini sağlar
		/// </summary>
		/// <param name="changeItem">Şifre değiştirme bilgilerini içeren model</param>
		/// <returns>Kullanıcı bilgilerini döndürür</returns>
		/// <response code="200">Kullanıcının yeni şifresi başarıyla belirlenmiştir</response>
		/// <response code="400">Eski şifre yanlış veya şifre zaten belirlenmiş</response>
		/// <response code="404">Kullanıcı bulunamadı</response>
		[HttpPut("NewPassword")]
		public async Task<IActionResult> NewPassword([FromBody] compUserNewPasswordModel changeItem)
		{
			var user = await _context.CompanyUsers.FindAsync(changeItem.CompUserId);

			if (user == null)
			{
				return NotFound();
			}

			if (user.Password != changeItem.OldPassword)
			{
				return BadRequest("Eski şifre yanlış.");
			}

			if (user.HasSetPassword)
			{
				return BadRequest("Şifre zaten belirlenmiş.");
			}

			user.Password = changeItem.NewPassword;
			user.HasSetPassword = true;
			await _context.SaveChangesAsync();
			return Ok(user);
		}
		#endregion



		#region Şirket Kullanıcı ile şirket bilgilerini getirme
		/// <summary>
		/// Şirket kullanıcı kimliği ile şirket bilgilerini getirir.
		/// </summary>
		/// <param name="compUserId">Şirket kullanıcısının kimliği</param>
		/// <returns>Şirket bilgilerini içeren bir nesne döner</returns>
		/// <response code="200">Şirket bilgileri başarıyla getirildi</response>
		/// <response code="404">Şirket kullanıcısı bulunamadı</response>
		[HttpGet("GetCompanyInformations/{compUserId}")]
		public async Task<ActionResult> GetCompanyInformations(int compUserId)
		{
			var user = await _context.Companies.Where(c => c.CompUserId == compUserId).FirstOrDefaultAsync();

			if (user == null)
			{
				return NotFound();
			}

			return Ok(user);
		}
		#endregion


		#region Şifre Değiştirme
		/// <summary>
		/// Kullanıcı şifre değiştirme
		/// </summary>
		/// <param name="passModel">Eski ve yeni şifreyi içeren model</param>
		/// <returns>başarılı veya başarısız sonucunu döndürür</returns>
		[HttpPut("ChangePassword")]
		public async Task<IActionResult> ChangePassword([FromBody] compUserChangePassworModel passModel)
		{
			var user = await _context.CompanyUsers
				.FirstOrDefaultAsync(u => u.CompUserId == passModel.UserId && u.Password == passModel.oldPassword);

			if (user == null)
			{
				return Unauthorized();
			}


			user.Password = passModel.newPassword;

			_context.Entry(user).State = EntityState.Modified;
			await _context.SaveChangesAsync();
			return NoContent();
		}
		#endregion

		#region Şirket Kullanıcısı Bilgilerini Güncelleme
		/// <summary>
		/// Kullanıcı profili güncelleme
		/// </summary>
		/// <param name="updateUser">Güncellenmiş kullanıcı bilgilerini tutar</param>
		/// <returns>başarılı veya başarısız sonucunu döndürür</returns>
		[HttpPut("UpdateCompanyUser")]
		public async Task<IActionResult> UpdateCompanyUser([FromBody] UpdateCompanyUserModel updateUser)
		{
			var user = await _context.CompanyUsers.FindAsync(updateUser.CompUserId);
			if (user == null)
			{
				return NotFound();
			}

			user.NameSurname = updateUser.NameSurname;
			user.Email = updateUser.Email.ToLower();
			user.Phone = updateUser.Phone;
			user.Password = updateUser.Password; // burada yine hashlenme gerekebilir.

			await _context.SaveChangesAsync();

			return NoContent();
		}
		#endregion

		#region Kullanıcı şifremi unuttum
		/// <summary>
		/// Kullanıcı şifremi unuttum
		/// </summary>
		/// <param name="email">hesaba aşt email</param>
		/// <returns></returns>
		[HttpPost("ForgotPassword")]
		public async Task<IActionResult> ForgotPassword([FromBody] ForgotPasswordModel email)
		{
			var user = await _context.CompanyUsers.FirstOrDefaultAsync(u => u.Email == email.Email);

			if (user == null)
			{
				return NotFound("User not found");
			}

			var verificationCode = new Random().Next(100000, 999999).ToString();

			var userForgotPassword = new CompanyUserForgotPassword
			{
				CompUserId = user.CompUserId,
				VerifyCode = verificationCode,
				ExpirationTime = DateTime.UtcNow.AddMinutes(5) // kodun geçerlilik süresi 5 dakika
			};

			_context.CompanyUserForgotPasswords.Add(userForgotPassword);
			await _context.SaveChangesAsync();

			await _emailService.SendVerificationCodeAsync(user.Email, verificationCode);

			return Ok("Verification code sent to your email");
		}
		#endregion


		#region reset password
		/// <summary>
		/// Kullanıcı şifre sıfırlama
		/// </summary>
		/// <param name="model">UserId - VerificationCode - NewPassword değerlerini paramatere alır</param>
		/// <returns></returns>
		[HttpPost("ResetPassword")]
		public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordModel model)
		{

			int id = await _context.CompanyUsers.Where(u => u.Email == model.Email).Select(u => u.CompUserId).FirstOrDefaultAsync();
			var record = await _context.CompanyUserForgotPasswords
				.FirstOrDefaultAsync(u => u.CompUserId == id && u.VerifyCode == model.Code && u.ExpirationTime > DateTime.UtcNow);

			if (record == null)
			{
				return BadRequest("Invalid or expired verification code");
			}

			var user = await _context.CompanyUsers.FindAsync(id);
			if (user == null)
			{
				return NotFound("User not found");
			}
			var passwordHasher = new PasswordHasher<CompanyUser>();


			user.Password = passwordHasher.HashPassword(null, model.NewPassword);
			_context.Entry(user).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			return Ok("Password reset successful");
		}
		#endregion


	}
}

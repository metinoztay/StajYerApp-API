﻿using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajYerApp_API.DTOs;
using StajYerApp_API.Models;
using StajYerApp_API.Services;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace StajYerApp_API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ApplicationController : ControllerBase
	{
		private readonly Db6761Context _context;
		private readonly IEmailService _emailService;

		/// <summary>
		/// ApplicationController'ı Db6761Context ve IEmailService ile başlatır.
		/// </summary>
		/// <param name="context">Uygulamanın veritabanı bağlantısı</param>
		/// <param name="emailService">E-posta gönderme servisi</param>
		public ApplicationController(Db6761Context context, IEmailService emailService)
		{
			_context = context;
			_emailService = emailService;
		}

		#region Kullanıcının Tüm Başvurularını Listeleme
		/// <summary>
		/// Kullanıcının tüm başvurularını ve ilgili ilanları listeler.
		/// </summary>
		/// <param name="userId">Kullanıcı kimliği</param>
		/// <returns>Kullanıcının başvurduğu ilanların listesini döner</returns>
		/// <response code="200">Başvurular başarıyla getirildi</response>
		/// <response code="404">Kullanıcıya ait başvuru bulunamadı</response>
		[HttpGet("ListUsersAllApplications/{userId}")]
		public async Task<ActionResult<UserApplicationsDto>> ListUsersAllApplications(int userId)
		{
			var user = await _context.Users
				.Include(u => u.Applications)
				.ThenInclude(a => a.Advert)
				.ThenInclude(ad => ad.Comp)
				.Include(u => u.Certificates)
				.Include(u => u.Educations)
				.Include(u => u.Experiences)
				.Include(u => u.Projects)
				.FirstOrDefaultAsync(u => u.UserId == userId);

			if (user == null)
			{
				return NotFound();
			}

			var userApplicationsDto = new UserApplicationsDto
			{
				UserId = user.UserId,
				Uname = user.Uname,
				Usurname = user.Usurname,
				Uemail = user.Uemail,
				Uphone = user.Uphone,
				Ubirthdate = user.Ubirthdate,
				Ugender = user.Ugender,
				Ulinkedin = user.Ulinkedin,
				Ucv = user.Ucv,
				Ugithub = user.Ugithub,
				Udesc = user.Udesc,
				Uprofilephoto = user.Uprofilephoto,
				Certificates = user.Certificates.Select(c => new CertificateDto
				{
					CertId = c.CertId,
					CertName = c.CertName,
					CerCompanyName = c.CerCompanyName,
					CertDesc = c.CertDesc,
					CertDate = c.CertDate
				}).ToList(),
				Educations = user.Educations.Select(e => new EducationDto
				{
					EduId = e.EduId,
					UniId = e.UniId,
					ProgId = e.ProgId,
					EduStartDate = e.EduStartDate,
					EduFinishDate = e.EduFinishDate,
					EduGano = e.EduGano,
					EduSituation = e.EduSituation,
					EduDesc = e.EduDesc
				}).ToList(),
				Experiences = user.Experiences.Select(ex => new ExperienceDto
				{
					ExpId = ex.ExpId,
					ExpPosition = ex.ExpPosition,
					ExpCompanyName = ex.ExpCompanyName,
					ExpCityId = ex.ExpCityId,
					ExpStartDate = ex.ExpStartDate,
					ExpFinishDate = ex.ExpFinishDate,
					ExpWorkType = ex.ExpWorkType,
					ExpDesc = ex.ExpDesc
				}).ToList(),
				Projects = user.Projects.Select(p => new ProjectDto
				{
					ProId = p.ProId,
					ProName = p.ProName,
					ProDesc = p.ProDesc,
					ProGithub = p.ProGithub
				}).ToList(),
				Applications = user.Applications.Select(app => new ApplicationDto
				{
					AppId = app.AppId,
					AdvertId = app.AdvertId,
					AppDate = app.AppDate,
					AppLetter = app.AppLetter,
					CompName = app.Advert.Comp.CompName
				}).ToList()
			};

			return Ok(userApplicationsDto);
		}

		#endregion

		#region İlana Başvuranları Listeleme
		/// <summary>
		/// Bir ilana başvuran kullanıcıları listeler.
		/// </summary>
		/// <param name="advertId">İlan kimliği</param>
		/// <returns>İlana başvuran kullanıcıların listesini döner</returns>
		/// <response code="200">Başvuran kullanıcılar başarıyla getirildi</response>
		/// <response code="404">İlana ait başvuru bulunamadı</response>
		[HttpGet("ListAdvertsApplications/{advertId}")]
		public async Task<ActionResult> ListAdvertsApplications(int advertId)
		{
			var apps = await _context.Applications.Where(a => a.AdvertId == advertId).ToListAsync();

			if (apps == null)
			{
				return NotFound();
			}

			var appliedUsers = await _context.Users
				.Where(u => apps.Select(app => app.UserId).Contains(u.UserId) && u.Uisactive)
				.ToListAsync();

			return Ok(appliedUsers);
		}
		#endregion

		#region Kullanıcı ilana başvuru yapar
		/// <summary>
		/// Kullanıcının ilana başvuru yapmasını sağlar.
		/// </summary>
		/// <param name="appAdvert">Başvuru bilgilerini içeren model</param>
		/// <returns>Başvuru işleminin sonucunu döner</returns>
		/// <response code="200">Başvuru başarıyla yapıldı</response>
		/// <response code="400">Email onayı gerekli veya başka bir hata oluştu</response>
		/// <response code="404">Kullanıcı bulunamadı</response>
		[HttpPost("UserApplyAdvert")]
		public async Task<ActionResult> UserApplyAdvert([FromBody] UserApplyModel appAdvert)
		{
			var user = await _context.Users.FindAsync(appAdvert.UserId);

			if (user == null)
			{
				return NotFound("Kullanıcı bulunamadı");
			}

			if (!user.UisEmailVerified)
			{
				return BadRequest(new { message = "Email onayı gerekli. Lütfen email doğrulama kodunu gönderin.", isEmailVerified = user.UisEmailVerified });
			}

			var application = new Application
			{
				UserId = appAdvert.UserId,
				AdvertId = appAdvert.AdvertId,
				AppDate = DateTime.Now.ToString(),
				AppLetter = appAdvert.AppLetter,
			};

			_context.Applications.Add(application);
			await _context.SaveChangesAsync();
			return Ok(new { message = "Başvuru başarıyla yapıldı", isEmailVerified = user.UisEmailVerified });
		}
		#endregion


		#region Email Doğrulama Kodu Gönder
		/// <summary>
		/// Kullanıcının email adresine doğrulama kodu gönderir.
		/// </summary>
		/// <param name="userId">Kullanıcı kimliği</param>
		/// <returns>Doğrulama kodu gönderme işleminin sonucunu döner</returns>
		/// <response code="200">Doğrulama kodu başarıyla gönderildi</response>
		/// <response code="404">Kullanıcı bulunamadı</response>
		[HttpPost("SendVerificationCode")]
		public async Task<ActionResult> SendVerificationCode([FromBody] UserSendVerificationCodeModel model)
		{
			var user = await _context.Users.FindAsync(model.UserId);

			if (user == null)
			{
				return NotFound("Kullanıcı bulunamadı");
			}

			var verificationCode = new Random().Next(100000, 999999).ToString();
			var userForgotPassword = new UserForgotPassword
			{
				UserId = user.UserId,
				VerifyCode = verificationCode,
				ExpirationTime = DateTime.UtcNow.AddMinutes(5)
			};

			_context.UserForgotPasswords.Add(userForgotPassword);
			await _context.SaveChangesAsync();

			await _emailService.SendVerificationCodeAsync(user.Uemail, verificationCode);

			return Ok("Doğrulama kodu e-posta adresinize gönderildi.");
		}

		#endregion

		#region E-posta Doğrulama
		/// <summary>
		/// Kullanıcının e-posta adresini doğrular.
		/// </summary>
		/// <param name="model">Doğrulama kodunu içeren model</param>
		/// <returns>E-posta doğrulama işleminin sonucunu döner</returns>
		/// <response code="200">E-posta doğrulaması başarılı</response>
		/// <response code="400">Geçersiz veya süresi dolmuş doğrulama kodu</response>
		/// <response code="404">Kullanıcı bulunamadı</response>
		[HttpPost("VerifyEmail")]
		public async Task<IActionResult> VerifyEmail([FromBody] VerifyCodeModel model)
		{
			var record = await _context.UserForgotPasswords
				.FirstOrDefaultAsync(u => u.UserId == model.UserId && u.VerifyCode == model.Code && u.ExpirationTime > DateTime.UtcNow);

			if (record == null)
			{
				return BadRequest("Geçersiz veya süresi dolmuş doğrulama kodu");
			}

			var user = await _context.Users.FindAsync(model.UserId);
			if (user == null)
			{
				return NotFound("Kullanıcı bulunamadı");
			}

			user.UisEmailVerified = true;

			_context.Entry(user).State = EntityState.Modified;
			await _context.SaveChangesAsync();

			// Kodu veritabanından siliyorum
			_context.UserForgotPasswords.Remove(record);
			await _context.SaveChangesAsync();

			return Ok("E-posta doğrulaması başarılı");
		}
		#endregion
	}
}

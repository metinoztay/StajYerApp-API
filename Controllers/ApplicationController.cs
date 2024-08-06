using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajYerApp_API.DTOs;
using StajYerApp_API.Models;
using StajYerApp_API.Services;

namespace StajYerApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly Db6761Context _context;
        private readonly IEmailService _emailService;

        /// <summary>
        /// UserController Db6761Context ile başlatır
        /// </summary>
        /// <param name="context">Uygulamanın veritabanı bağlantısı yapılıyor</param>
        public ApplicationController(Db6761Context context, IEmailService emailService)
        {
            _context = context;
            _emailService = emailService;
        }


        #region Kullanıcnın Tüm Başvurularını Listeleme
        [HttpGet("ListUsersAllApplications/{userId}")]
        public async Task<ActionResult> ListUsersAllApplications(int userId)
        {
            var apps = await _context.Applications.Where(a => a.UserId == userId).ToListAsync();

            List<Advertisement> advertList = new List<Advertisement>();
            foreach (var app in apps) {
                var advert = await _context.Advertisements.FindAsync(app.AdvertId);
                if (advert != null)
                    advertList.Add(advert);
            }

            if (apps == null)
            {
                return NotFound();
            }

            return Ok(advertList);
        }
        #endregion

        

        #region İlana Başvuranları Listeleme
        [HttpGet("ListAdvertsApplications/{advertId}")]
        public async Task<ActionResult> ListAdvertsApplications(int advertId)
        {
            List<User> appliedUsers = new List<User>();
            var apps = await _context.Applications.Where(a => a.AdvertId == advertId).ToListAsync();
            if (apps == null)
            {
                return NotFound();
            }
            
            foreach (var app in apps) {
                var user = await _context.Users.FindAsync(app.UserId);
                if (user != null && user.Uisactive == true)
                    appliedUsers.Add(user);                
            }
            return Ok(appliedUsers);
        }
        #endregion

        #region Kullanıcı ilana başvuru yapar
        [HttpPost("UserApplyAdvert")]
        public async Task<ActionResult> UserApplyAdvert([FromBody] UserApplyModel appAdvert)
        {
            var user = await _context.Users.FindAsync(appAdvert.UserId);

            if (user == null)
            {
                return NotFound("Kullanıcı bulunamadı");
            }

            if (user.UisEmailVerified != true)
            {
                var verificationCode = new Random().Next(100000, 999999).ToString();
                var userForgotPassword = new UserForgotPassword
                {
                    UserId = user.UserId,
                    VerifyCode = verificationCode,
                    ExpirationTime = DateTime.UtcNow.AddMinutes(10) // Kodun geçerlilik süresi 10 dakika
                };

                _context.UserForgotPasswords.Add(userForgotPassword);
                await _context.SaveChangesAsync();

                await _emailService.SendVerificationCodeAsync(user.Uemail, verificationCode);

                return BadRequest("Email Onayı Gerekli. Onay kodu e-posta adresinize gönderildi.");
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
            return Ok();
        }
        #endregion

        [HttpPost("VerifyEmail")]
        public async Task<IActionResult> VerifyEmail([FromBody] VerifyCodeModel model)
        {
            var record = await _context.UserForgotPasswords //kodları bu modelde saklıyoruz. daha sonra değiştirilebilir.
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

    }
}

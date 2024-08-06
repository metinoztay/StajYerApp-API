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
                IsVerified = false
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
            if (user.IsVerified==false)
            {

            }

            return Ok(user);
        }
        #endregion

        //şirket mailden gelen password ile giriş yaptıktan sonra direkt şifre değiştirme ekranına gönderilmeli. 
        //şifre değiştirme endpointi yazılacak. 

        #region yeni şifre 
        [HttpPut("NewPassword")]
        public async Task<IActionResult> NewPassword([FromBody] compUserNewPasswordModel changeItem)
        {
            var user=await _context.CompanyUsers.FindAsync(changeItem.CompUserId);

            if (user == null)
            {
                return NotFound();
            }
                                                            //şifre hashleme bakılabilir.
            if (user.Password!= changeItem.OldPassword)
            {
                return BadRequest("Eski şifre yanlış.");
            }
            user.Password = changeItem.NewPassword;
            await _context.SaveChangesAsync();
            return Ok(user);
            
        }
        #endregion

        #region Şirket Kullanıcı ile şirket bilgilerini getirme
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
            user.Password = updateUser.Password; //burada yine hashlenme gerekebilir.

            
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion


    }
}

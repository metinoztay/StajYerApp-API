using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajYerApp_API.DTOs;
using StajYerApp_API.Models;
using static System.Net.Mime.MediaTypeNames;
using static System.Net.WebRequestMethods;

namespace StajYerApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly Db6761Context _context;

        /// <summary>
        /// UserController Db6761Context ile başlatır
        /// </summary>
        /// <param name="context">Uygulamanın veritabanı bağlantısı yapılıyor</param>
        public UserController(Db6761Context context)
        {
            _context = context;
        }

        #region Kullanıcı Kayıt
        /// <summary>
        /// Kullanıcı kaydeder
        /// </summary>
        /// <param name="newUser">Kaydedilecek kullanıcı</param>
        /// <returns>Oluşturulan kullanıcıyı döndürür</returns>
        [HttpPost("Register")]
        public async Task<ActionResult<User>> Register([FromBody] NewUserModel newUser)
        {
            var addedUser = new User
            {
                Uname = Utilities.CapitalizeFirstLetter(newUser.Uname),
                Usurname = newUser.Usurname.ToUpper(),
                Uemail = newUser.Uemail.ToLower(),
                Upassword = newUser.Upassword,
                Uphone = newUser.Uphone,
                Ubirthdate = newUser.Ubirthdate,
                Ugender = newUser.Ugender,
                Uprofilephoto = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png",
                Uisactive = true
            };

            _context.Users.Add(addedUser);
            await _context.SaveChangesAsync();
            return Ok(addedUser);
        }
        #endregion

        #region Kullanıcı Giriş
        /// <summary>
        /// Kullanıcı girişi
        /// </summary>
        /// <param name="loginUser">Kullanıcı adı ve şifre içeren giriş modeli</param>
        /// <returns>Kimliği doğrulanmış kullanıcıyı döndürür</returns>
        [HttpPost("Login")]
        public async Task<ActionResult<User>> Login([FromBody] UserLoginModel loginUser)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.Uemail == loginUser.Uemail && u.Upassword == loginUser.Upassword);

            if (user == null)
            {
                return Unauthorized();
            }

            return Ok(user);
        }
        #endregion

        #region Kullanıcı Silme
        /// <summary>
        /// belirtilen id'ye sahip kullanıcıyı siler
        /// </summary>
        /// <param name="deleteUserId">Silinecek kullanıcının id'si</param>
        /// <returns>başarılı veya başarısız sonucunu döndürür</returns>
        [HttpDelete("Delete/{deleteUserId}")]
        public async Task<IActionResult> Delete(int deleteUserId)
        {
            var user = await _context.Users.FindAsync(deleteUserId);
            if (user == null)
            {
                return NotFound();
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region Kullanıcı Deaktif Etme
        /// <summary>
        /// belirtilen id'ye sahip kullanıcıyı deaktif eder
        /// </summary>
        /// <param name="deactivateUserId">Deactif edilecek kullanıcının id'si</param>
        /// <returns>başarılı veya başarısız sonucunu döndürür</returns>
        [HttpPost("Deactivate/{deactivateUserId}")]
        public async Task<IActionResult> Deactivate(int deactivateUserId)
        {
            var user = await _context.Users.FindAsync(deactivateUserId);
            if (user == null)
            {
                return NotFound();
            }

            user.Uisactive = false;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region Kullanıcı Aktif Etme
        /// <summary>
        /// belirtilen id'ye sahip kullanıcıyı aktif eder
        /// </summary>
        /// <param name="activateUserId">Aktif edilecek kullanıcının id'si</param>
        /// <returns>başarılı veya başarısız sonucunu döndürür</returns>
        [HttpPost("Activate/{activateUserId}")]
        public async Task<IActionResult> Activate(int activateUserId)
        {
            var user = await _context.Users.FindAsync(activateUserId);
            if (user == null)
            {
                return NotFound();
            }

            user.Uisactive = true;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion

        #region Şifre Değiştirme
        /// <summary>
        /// Kullanıcı şifre değiştirme
        /// </summary>
        /// <param name="passModel">Eski ve yeni şifreyi içeren model</param>
        /// <returns>başarılı veya başarısız sonucunu döndürür</returns>
        [HttpPut("ChangePassword")]
        public async Task<IActionResult> ChangePassword([FromBody] UserChangePasswordModel passModel)
        {
            var user = await _context.Users
                .FirstOrDefaultAsync(u => u.UserId == passModel.UserId && u.Upassword == passModel.oldPassword);

            if (user == null)
            {
                return Unauthorized();
            }

            user.Upassword = passModel.newPassword;

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return NoContent();
        }
        #endregion

        #region Kullanıcı Bilgilerini Güncelleme
        /// <summary>
        /// Kullanıcı profili güncelleme
        /// </summary>
        /// <param name="updateUser">Güncellenmiş kullanıcı bilgilerini tutar</param>
        /// <returns>başarılı veya başarısız sonucunu döndürür</returns>
        [HttpPut("UpdateUser")]
        public async Task<IActionResult> UpdateProfile([FromBody] UpdateUserModel updateUser)
        {
            var user = await _context.Users.FindAsync(updateUser.UserId);
            if (user == null)
            {
                return NotFound();
            }

            var updatedUser = new User
            {
                
                Uname = Utilities.CapitalizeFirstLetter(updateUser.Uname),
                Usurname = updateUser.Usurname.ToUpper(),
                Uemail = updateUser.Uemail.ToLower(),
                Upassword = updateUser.Upassword,
                Uphone = updateUser.Uphone,
                Ubirthdate = updateUser.Ubirthdate,
                Ugender = updateUser.Ugender,
                Ulinkedin = updateUser.Ulinkedin,
                Ugithub = updateUser.Ugithub,
                Ucv = updateUser.Ucv,
                Udesc = updateUser.Udesc,
            };
            

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();
             
            return NoContent();
        }
        #endregion

        //Profil Photo Güncelleme Eklenecek Kullanıcıdan dosya alınması gerek

        //Kullanıcı mail ve telefon onayı gerekli

        #region Profil Fotoğrafı Silme
        /// <summary>
        /// belirtilen id'ye sahip kullanıcıyının fotoğrafını siler
        /// </summary>
        /// <param name="photoUserId">Fotoğrafı silinecek kullanıcının id'si</param>
        /// <returns>başarılı veya başarısız sonucunu döndürür</returns>
        [HttpPut("DeletePhoto/{photoUserId}")]
        public async Task<IActionResult> DeletePhoto(int photoUserId)
        {
            var user = await _context.Users.FindAsync(photoUserId);
            if (user == null)
            {
                return NotFound();
            }

            //profile photo düzenlenecek
            user.Uprofilephoto = "https://cdn.pixabay.com/photo/2015/10/05/22/37/blank-profile-picture-973460_960_720.png";

            _context.Entry(user).State = EntityState.Modified;
            await _context.SaveChangesAsync();

            return NoContent();
        }
        #endregion
    }
}

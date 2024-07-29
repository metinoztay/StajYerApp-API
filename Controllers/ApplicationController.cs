using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajYerApp_API.DTOs;
using StajYerApp_API.Models;

namespace StajYerApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApplicationController : ControllerBase
    {
        private readonly Db6761Context _context;

        /// <summary>
        /// UserController Db6761Context ile başlatır
        /// </summary>
        /// <param name="context">Uygulamanın veritabanı bağlantısı yapılıyor</param>
        public ApplicationController(Db6761Context context)
        {
            _context = context;
        }


        #region Kullanıcnın Tüm Başvurularını Listeleme
        [HttpGet("ListUsersAllApplications/{userId}")]
        public async Task<ActionResult> ListUsersAllApplications(int userId)
        {
            var apps = await _context.Applications.Where(a => a.UserId == userId).ToListAsync();

            if (apps == null)
            {
                return NotFound();
            }

            return Ok(apps);
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
                appliedUsers.Add(user);
            }
            return Ok(appliedUsers);
        }
        #endregion

        #region Kullanıcı ilana başvuru yapar
        [HttpPost("UserApplyAdvert")]
        public async Task<ActionResult> UserApplyAdvert([FromBody] UserApplyModel appAdvert)
        {
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
    }
}

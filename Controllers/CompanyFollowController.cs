using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajYerApp_API.DTOs;
using StajYerApp_API.Models;

namespace StajYerApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyFollowController : ControllerBase
    {
        private readonly Db6761Context _context;

        public CompanyFollowController(Db6761Context context)
        {
            _context = context;
        }

        #region Kullanıcı için şirket takibi ve takipten çıkma
        [HttpPost("UserFollowUnfollowCompany")]
        public async Task<ActionResult> UserFollowUnfollowCompany([FromBody] FollowCompanyModel model)
        {
            var follow = _context.UserFollowedCompanies.Where(u => u.UserId == model.UserId && u.CompId == model.CompId).FirstOrDefault();
            if (follow != null)
            {
                _context.UserFollowedCompanies.Remove(follow);
                await _context.SaveChangesAsync();
            }
            else
            {
                var comp = await _context.Companies.FindAsync(model.CompId);
                if (comp != null)
                {
                    var flw = new UserFollowedCompany
                    {
                        UserId = model.UserId,
                        CompId = model.CompId
                    };

                    _context.UserFollowedCompanies.Add(flw);
                    await _context.SaveChangesAsync();
                }
            }
            return Ok();
        }
        #endregion

        #region Kullanıcının takip ettiği şirketleri Listeleme
        [HttpGet("ListUsersFollowedCompanies/{userId}")]
        public async Task<ActionResult> ListUsersFollowedCompanies(int userId)
        {

            var follows = await _context.UserFollowedCompanies
                .Where(u => u.UserId == userId)
                .Select(u => u.Comp)
                .ToListAsync();

            if (follows == null || !follows.Any())
            {
                return NotFound("Kullanıcının takip ettiği şirket bulunmamaktadır");
            }
            
            return Ok(follows);
        }
        #endregion

        #region Kullanıcının takip ettiği şirketlerin ilanlarını Listeleme
        [HttpGet("ListUsersFollowedCompaniesAdverts/{userId}")]
        public async Task<ActionResult> ListUsersFollowedCompaniesAdverts(int userId)
        {

            var follows = await _context.UserFollowedCompanies
                .Where(u => u.UserId == userId)
                .Select(u => u.Comp).SelectMany(u => u.Advertisements)
                .ToListAsync();

            if (follows == null || !follows.Any())
            {
                return NotFound("Kullanıcının takip ettiği şirket bulunmamaktadır");
            }

            return Ok(follows);
        }
        #endregion

        #region Kullanıcının şirketi takip etme bilgisini alır
        [HttpGet("GetUserIsFollowed/{userId}/{compId}")]
        public async Task<ActionResult> GetUserIsFollowed(int userId,int compId)
        {

            bool isFollowing = await _context.UserFollowedCompanies.AnyAsync(u => u.UserId == userId && u.CompId == compId);

           
            return Ok(isFollowing);
        }
        #endregion
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajYerApp_API.DTOs;
using StajYerApp_API.Models;

namespace StajYerApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EducationController : ControllerBase
    {
        private readonly Db6761Context _context;
        /// <summary>
        /// EducationController Db6761Context ile başlatır
        /// </summary>
        /// <param name="context">Uygulamanın veritabanı bağlantısı yapılıyor</param>
        public EducationController(Db6761Context context)
        {
            _context = context;
        }

        #region Kullanıcının eğitimlerini listele
        [HttpGet("ListUserEducations/{userId}")]
        public async Task<ActionResult<IEnumerable<Experience>>> ListUserEducations(int userId)
        {
            var educations = await _context.Educations.Where<Education>(e => e.UserId == userId).ToListAsync();

            if (educations == null || educations.Count == 0)
            {
                return NotFound();
            }

            return Ok(educations);
        }
        #endregion


        #region id'ye göre eğitimleri listele

        [HttpGet("GetEducation/{eduId}")]
        public async Task<ActionResult<Education>> GetEducation(int eduId)
        {
            var education = await _context.Educations
                .Include(e => e.Uni)
                .Include(e => e.Prog)
                .FirstOrDefaultAsync(e => e.EduId == eduId);

            if (education == null)
            {
                return NotFound();
            }

            return Ok(education);
        }
        #endregion




        [HttpPost("AddEducation")]
        public async Task<ActionResult> AddEducation(NewEducationModel newEdu)
        {
            if (newEdu.EduFinishDate != null)
            {
                DateTime finishDate = DateTime.Parse(newEdu.EduFinishDate);

                if (finishDate < DateTime.Now)
                {
                    newEdu.EduSituation = "Mezun";
                }
            }

            var education = new Education
            {
                UserId = newEdu.UserId,
                UniId = newEdu.UniId,
                ProgId = newEdu.ProgId,
                EduStartDate = newEdu.EduStartDate,
                EduFinishDate = newEdu.EduFinishDate,
                
                EduGano = newEdu.EduGano,
                EduSituation = newEdu.EduSituation,
                EduDesc = newEdu.EduDesc
            };

            _context.Educations.Add(education);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete("DeleteEducation/{eduId}")]
        public async Task<ActionResult> DeleteEducation(int eduId)
        {
            var edu = await _context.Educations
                .FirstOrDefaultAsync(e => e.EduId == eduId);

            _context.Educations.Remove(edu);
            await _context.SaveChangesAsync();

            return Ok();
        }
        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProId == id);
        }
    }
}

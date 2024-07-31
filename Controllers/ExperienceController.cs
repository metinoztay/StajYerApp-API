using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajYerApp_API.DTOs;
using StajYerApp_API.Models;

namespace StajYerApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExperienceController : ControllerBase
    {
        private readonly Db6761Context _context;
        /// <summary>
        /// ExrienceController Db6761Context ile başlatır
        /// </summary>
        /// <param name="context">Uygulamanın veritabanı bağlantısı yapılıyor</param>
        public ExperienceController(Db6761Context context)
        {
            _context = context;
        }

        #region Tüm projeleri listele
        [HttpGet("ListUserExperiences/{userId}")]
        public async Task<ActionResult<IEnumerable<Experience>>> ListUserExperiences(int userId)
        {
            var experiences = await _context.Experiences.Where<Experience>(e => e.UserId == userId).ToListAsync();

            if (experiences == null || experiences.Count == 0)
            {
                return NotFound();
            }

            return Ok(experiences);
        }
        #endregion


        #region id'ye göre Tectübeleri listele
       
        [HttpGet("GetExperience/{ProId}")]
        public async Task<ActionResult<Project>> GetExperience(int expId)
        {
            var project = await _context.Experiences
                .FirstOrDefaultAsync(e => e.ExpId == expId);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }
        #endregion



        
        [HttpPost("AddExperience")]
        public async Task<ActionResult> AddExperience(NewExperienceModel newExperience)
        {
            var experience = new Experience
            {
                UserId = newExperience.UserId,
                ExpPosition = newExperience.ExpPosition,
                ExpCompanyName = newExperience.ExpCompanyName,
                ExpCity = newExperience.ExpCity,
                ExpStartDate = newExperience.ExpStartDate,
                ExpFinishDate = newExperience.ExpFinishDate,
                ExpWorkType = newExperience.ExpWorkType,
                ExpDesc = newExperience.ExpDesc
            };

            _context.Experiences.Add(experience);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete("DeleteExperience/{expId}")]
        public async Task<ActionResult> DeleteProject(int expId)
        {
            var exp = await _context.Experiences
                .FirstOrDefaultAsync(e => e.ExpId == expId);

            _context.Experiences.Remove(exp);
            await _context.SaveChangesAsync();

            return Ok();
        }
        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProId == id);
        }

    }
}

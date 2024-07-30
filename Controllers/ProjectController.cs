using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajYerApp_API.Models;

namespace StajYerApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProjectController : ControllerBase
    {
        private readonly Db6761Context _context;
        /// <summary>
        /// ProjectController Db6761Context ile başlatır
        /// </summary>
        /// <param name="context">Uygulamanın veritabanı bağlantısı yapılıyor</param>
        public ProjectController(Db6761Context context)
        {
            _context = context;
        }

        #region Tüm projeleri listele
        // api/Projects/user/{userId}
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Project>>> GetListUserProjects(int userId)
        {
            var projects = await _context.Projects
                .Where(p => p.UserId == userId)
                .ToListAsync();

            if (projects == null || projects.Count == 0)
            {
                return NotFound();
            }

            return Ok(projects);
        }
        #endregion


        #region id'ye göre projeleri listele
        // api/Projects/{userId}/{projectId}
        [HttpGet("{userId}/{projectId}")]
        public async Task<ActionResult<Project>> GetUserProject(int userId, int projectId)
        {
            var project = await _context.Projects
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId && p.ProId == projectId);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }
        #endregion
    }
}

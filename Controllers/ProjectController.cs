using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajYerApp_API.DTOs;
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
        [HttpGet("ListUserProjects/{userId}")]
        public async Task<ActionResult<IEnumerable<Project>>> ListUserProjects(int userId)
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
        // api/Projects/{userId}/{ProId}
        [HttpGet("GetProject/{ProId}")]
        public async Task<ActionResult<Project>> GetProject(int ProId)
        {
            var project = await _context.Projects
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.ProId == ProId);

            if (project == null)
            {
                return NotFound();
            }

            return Ok(project);
        }
        #endregion



        // POST: api/Project
        [HttpPost("AddProject")]
        public async Task<ActionResult> AddProject(ProjectsModel projectsDTO)
        {
            var newProject = new Project
            {
                UserId = projectsDTO.UserId,
                ProName = projectsDTO.ProName,
                ProGithub = projectsDTO.ProGithub,
                ProDesc = projectsDTO.ProDesc,
                
            };

            _context.Projects.Add(newProject);
            await _context.SaveChangesAsync();

            return Ok();
        }


        [HttpDelete("DeleteProject")]
        public async Task<ActionResult> DeleteProject(int projectId)
        {
            var project = await _context.Projects
                .FirstOrDefaultAsync(p => p.ProId == projectId);

            _context.Projects.Remove(project);
            await _context.SaveChangesAsync();

            return Ok();
        }



        private bool ProjectExists(int id)
        {
            return _context.Projects.Any(e => e.ProId == id);
        }
    }
}

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajYerApp_API.Models;

namespace StajYerApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UnivercityController : ControllerBase
    {
        private readonly Db6761Context _context;
        /// <summary>
        /// UnivercityController Db6761Context ile başlatır
        /// </summary>
        /// <param name="context">Uygulamanın veritabanı bağlantısı yapılıyor</param>
        public UnivercityController(Db6761Context context)
        {
            _context = context;
        }

        #region Universiteleri listele
        [HttpGet("ListUnivercities")]
        public async Task<ActionResult> ListUnivercities()
        {
            var univercities = await _context.Univercities.ToListAsync();

            if (univercities == null || univercities.Count == 0)
            {
                return NotFound();
            }

            return Ok(univercities);
        }
		#endregion

		#region Universiteleri listele
		[HttpGet("ListUnivercities{uniId}")]
		public async Task<ActionResult> ListUnivercitiesById(int uniId)
		{
			var univercities = await _context.Univercities.FirstOrDefaultAsync(u=>u.UniId==uniId);

			if (univercities == null)
			{
				return NotFound();
			}

			return Ok(univercities);
		}
		#endregion


		#region Programları listele
		[HttpGet("ListPrograms")]
        public async Task<ActionResult> ListPrograms()
        {
            var programs = await _context.Programs.ToListAsync();

            if (programs == null || programs.Count == 0)
            {
                return NotFound();
            }

            return Ok(programs);
        }
		#endregion

		#region Programları listele
		[HttpGet("ListPrograms{id}")]
		public async Task<ActionResult> ListProgramsById(int id)
		{
			var programs = await _context.Programs.FirstOrDefaultAsync(u => u.ProgId == id);

			if (programs == null)
			{
				return NotFound();
			}

			return Ok(programs);
		}
		#endregion
	}
}

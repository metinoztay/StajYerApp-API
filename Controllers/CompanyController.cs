using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajYerApp_API.DTOs;
using StajYerApp_API.Models;

namespace StajYerApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {

        private readonly Db6761Context _context;

        /// <summary>
        /// UserController Db6761Context ile başlatır
        /// </summary>
        /// <param name="context">Uygulamanın veritabanı bağlantısı yapılıyor</param>
        public CompanyController(Db6761Context context)
        {
            _context = context;
        }

        #region Tüm Şirketleri Listeleme
        [HttpGet("ListAllCompanies")]
        public async Task<ActionResult> ListAllCompanies()
        {
            var companies = await _context.Companies.ToListAsync();

            if (companies == null)
            {
                return NotFound();
            }

            return Ok(companies);
        }
        #endregion

        #region Id ile Şirket Bilgileri Çekme
        [HttpGet("GetCompanyById/{companyId}")]
        public async Task<ActionResult> GetCompanyById(int companyId)
        {
            var company = await _context.Companies.FindAsync(companyId);

            if (company == null)
            {
                return NotFound();
            }

            return Ok(company);
        }
        #endregion
    }
}

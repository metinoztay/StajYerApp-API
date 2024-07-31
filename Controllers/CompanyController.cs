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

        #region Şirket Ekleme
        [HttpPost("AddCompany")]
        public async Task<ActionResult> AddCompany(NewCompanyModel newCompany)
        {
            
            var company = new Company
            {
                CompName = newCompany.CompName,
                CompFoundationYear = newCompany.CompFoundationYear,
                CompWebSite = newCompany.CompWebSite,
                CompContactMail = newCompany.CompContactMail,
                CompAdress = newCompany.CompAdress + " " + newCompany.CompAddressTitle,
                CompAddressTitle = newCompany.CompAddressTitle,
                CompSektor = newCompany.CompSektor,
                CompDesc = newCompany.CompDesc,
                CompLogo = newCompany.CompLogo,
                ComLinkedin = newCompany.ComLinkedin,
                CompEmployeeCount = newCompany.CompEmployeeCount,
                CompUserId = newCompany.CompUserId
            };
 
            _context.Companies.Add(company);
            await _context.SaveChangesAsync();
            return Ok();
        }
        #endregion

        #region Şirket Bilgilerini Güncelleme
        [HttpPut("UpdateCompany")]
        public async Task<ActionResult> UpdateCompany(UpdateCompanyModel updateCompany)
        {

            var company = await _context.Companies.FindAsync(updateCompany.CompId);
            company.CompName = updateCompany.CompName;
            company.CompFoundationYear = updateCompany.CompFoundationYear;
            company.CompWebSite = updateCompany.CompWebSite;
            company.CompContactMail = updateCompany.CompContactMail;
            company.CompAdress = updateCompany.CompAdress + " " + updateCompany.CompAddressTitle;
            company.CompAddressTitle = updateCompany.CompAddressTitle;
            company.CompSektor = updateCompany.CompSektor;
            company.CompDesc = updateCompany.CompDesc;
            company.CompLogo = updateCompany.CompLogo;
            company.ComLinkedin = updateCompany.ComLinkedin;
            company.CompEmployeeCount = updateCompany.CompEmployeeCount;


            _context.Entry(company).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return Ok();
        }
        #endregion
    }
}

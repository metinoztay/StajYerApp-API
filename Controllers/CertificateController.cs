using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajYerApp_API.DTOs;
using StajYerApp_API.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StajYerApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CertificatesController : ControllerBase
    {
        private readonly Db6761Context _context;

        public CertificatesController(Db6761Context context)
        {
            _context = context;
        }

        #region Tüm sertifikaları listele
        [HttpGet("ListUserCertificates/{userId}")]
        public async Task<ActionResult<IEnumerable<Certificate>>> ListUserCertificates(int userId)
        {
            var certificates = await _context.Certificates
                .Where(p => p.UserId == userId)
                .ToListAsync();

            if (certificates == null || certificates.Count == 0)
            {
                return NotFound();
            }

            return Ok(certificates);
        }
        #endregion

        #region id'ye göre sertifikaları listele
        [HttpGet("GetCertificate/{certId}")]
        public async Task<ActionResult<Certificate>> GetCertificate( int certId)
        {
            var certificate = await _context.Certificates
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.CertId == certId);

            if (certificate == null)
            {
                return NotFound();
            }

            return Ok(certificate);
        }
        #endregion
        
        // POST: api/Certificates
        [HttpPost("AddCertificate")]
        public async Task<ActionResult<Certificate>> AddCertificate(CertificatesModel certificateDTO)
        {
            var newCertificate = new Certificate
            {
                UserId = certificateDTO.UserId,
                CertName = certificateDTO.CertName,
                CerCompanyName = certificateDTO.CerCompanyName,
                CertDesc = certificateDTO.CertDesc,
                CertDate = certificateDTO.CertDate
            };

            _context.Certificates.Add(newCertificate);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpDelete("DeleteCertificate")]
        public async Task<ActionResult<Certificate>> DeleteCertificate(int certificateId)
        {
            var certificate = await _context.Certificates.FindAsync(certificateId);

            _context.Certificates.Remove(certificate);
            await _context.SaveChangesAsync();

            return Ok();
        }



        private bool CertificateExists(int id)
        {
            return _context.Certificates.Any(e => e.CertId == id);
        }
    }
}

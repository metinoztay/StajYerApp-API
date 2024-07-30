﻿using Microsoft.AspNetCore.Mvc;
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
        [HttpGet("user/{userId}")]
        public async Task<ActionResult<IEnumerable<Certificate>>> GetListUserCertificates(int userId)
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
        [HttpGet("{userId}/{certId}")]
        public async Task<ActionResult<Certificate>> GetUserCertificate(int userId, int certId)
        {
            var certificate = await _context.Certificates
                .Include(p => p.User)
                .FirstOrDefaultAsync(p => p.UserId == userId && p.CertId == certId);

            if (certificate == null)
            {
                return NotFound();
            }

            return Ok(certificate);
        }
        #endregion

        // POST: api/Certificates
        [HttpPost]
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

            return CreatedAtAction(nameof(GetUserCertificate), new { userId = newCertificate.UserId, certId = newCertificate.CertId }, newCertificate);
        }

        // PUT: api/Certificates/{certId}
        [HttpPut("{certId}")]
        public async Task<IActionResult> UpdateCertificate(int certId, CertificatesModel certificateDTO)
        {
            if (certId != certificateDTO.CertId)
            {
                return BadRequest();
            }

            var certificate = await _context.Certificates.FindAsync(certId);
            if (certificate == null)
            {
                return NotFound();
            }

            certificate.UserId = certificateDTO.UserId;
            certificate.CertName = certificateDTO.CertName;
            certificate.CerCompanyName = certificateDTO.CerCompanyName;
            certificate.CertDesc = certificateDTO.CertDesc;
            certificate.CertDate = certificateDTO.CertDate;

            _context.Entry(certificate).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertificateExists(certId))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // DELETE: api/Certificates/{certId}
        [HttpDelete("{certId}")]
        public async Task<IActionResult> DeleteCertificate(int certId)
        {
            var certificate = await _context.Certificates.FindAsync(certId);
            if (certificate == null)
            {
                return NotFound();
            }

            _context.Certificates.Remove(certificate);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CertificateExists(int id)
        {
            return _context.Certificates.Any(e => e.CertId == id);
        }
    }
}

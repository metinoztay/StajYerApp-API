using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajYerApp_API.DTOs;
using StajYerApp_API.Models;

namespace StajYerApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdvertController : ControllerBase
    {
        private readonly Db6761Context _context;

        /// <summary>
        /// UserController Db6761Context ile başlatır
        /// </summary>
        /// <param name="context">Uygulamanın veritabanı bağlantısı yapılıyor</param>
        public AdvertController(Db6761Context context)
        {
            _context = context;
        }


        #region Tüm İlanları Listeleme
        [HttpGet("ListAllActiveAdverts")]
        public async Task<ActionResult<IEnumerable<Advertisement>>> ListAllActiveAdverts()
        {
            var adverts =  await _context.Advertisements.Where(a => a.AdvIsActive == true).ToListAsync();
            
            foreach (var advert in adverts) {
                var company = new Company();
                company = await _context.Companies.FindAsync(advert.CompId);
                company.Advertisements = null;
                advert.Comp = company;
            }

            if (adverts == null)
            {
                return NotFound();
            }

            return Ok(adverts);
        }
        #endregion

        #region Şirket ilan oluşturur
        [HttpPost("CompAddAdvert")]
        public async Task<ActionResult> CompAddAdvert([FromBody] NewAdvertModel newAdvert)
        {
            var advert = new Advertisement
            {
                CompId = newAdvert.CompId,
                AdvTitle = newAdvert.AdvTitle,
                AdvAdressTitle = newAdvert.AdvAdressTitle,
                AdvAdress = newAdvert.AdvAdress,
                AdvWorkType = newAdvert.AdvWorkType,
                AdvDepartment = newAdvert.AdvDepartment,                
                AdvJobDesc = newAdvert.AdvJobDesc,
                AdvQualifications = newAdvert.AdvQualifications,
                AdvAddInformation = newAdvert.AdvAddInformation,  
                AdvExpirationDate = newAdvert.AdvExpirationDate,
                AdvPhoto = newAdvert.AdvPhoto,
                AdvPaymentInfo = newAdvert.AdvPaymentInfo,
                AdvIsActive = true
            };

            _context.Advertisements.Add(advert);
            await _context.SaveChangesAsync();
            return Ok();
        }
        #endregion

        #region Kullanıcı için ilan kaydetme
        [HttpPost("UserSaveAdvert")]
        public async Task<ActionResult> UserSaveAdvert([FromBody] UserSaveAdvertModel saveAdvert)
        {
            var savedAdvert = new UsersSavedAdvert
            {
                UserId = saveAdvert.UserId,
                AdvertId = saveAdvert.AdvertId
            };

            _context.UsersSavedAdverts.Add(savedAdvert);
            await _context.SaveChangesAsync();
            return Ok();
        }
        #endregion

        #region Kullanıcı kaydettiği ilanı siler
        [HttpDelete("UserDeleteSavedAdvert")]
        public async Task<ActionResult> UserDeleteSavedAdvert([FromBody] UserSaveAdvertModel saveAdvert)
        {
            var savedAdvert = await _context.UsersSavedAdverts
                .FirstOrDefaultAsync(u => u.UserId == saveAdvert.UserId && u.AdvertId == saveAdvert.AdvertId);

            _context.UsersSavedAdverts.Remove(savedAdvert);
            await _context.SaveChangesAsync();
            return Ok();
        }
        #endregion

        #region Kullanıcının Kaydettiği İlanları Listeleme
        [HttpGet("ListUsersSavedAdverts/{userId}")]
        public async Task<ActionResult> ListUsersSavedAdverts(int userId)
        {
            List<Advertisement> adverts = new List<Advertisement>();
            var saves = await _context.UsersSavedAdverts.Where(a => a.UserId == userId).ToListAsync(); 
            
            if (adverts == null)
            {
                return NotFound();
            }

            foreach (var save in saves)
            {
                var advert = await _context.Advertisements.Where(a => a.AdvertId == save.AdvertId).FirstOrDefaultAsync();
                if (advert != null)
                {
                    adverts.Add(advert);
                }
            }

            return Ok(adverts);
        }
        #endregion

        #region İlan Idsi ile İlan Bilgilerini getirme
        [HttpGet("GetAdvertById/{advertId}")]
        public async Task<ActionResult> GetAdvertById(int advertId)
        {
            var advert = await _context.Advertisements.FindAsync(advertId);

            if (advert == null)
            {
                return NotFound();
            }

            return Ok(advert);
        }
        #endregion*/

        #region Şirket Id si ile İlanları Listeleme
        [HttpGet("ListCompanyAdverts/{companyId}")]
        public async Task<ActionResult> ListCompanyAdverts(int companyId)
        {
            var adverts = await _context.Advertisements.Where(a => a.CompId == companyId).ToListAsync();
            
            if (adverts == null)
            {
                return NotFound();
            }

            foreach (var advert in adverts)
            {
                int count = await _context.Applications
                    .CountAsync(a => a.AdvertId == advert.AdvertId);
                advert.AdvAppCount = count.ToString();
                _context.Entry(advert).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            adverts = await _context.Advertisements.Where(a => a.CompId == companyId).ToListAsync();
            return Ok(adverts);
        }
        #endregion
    }
}

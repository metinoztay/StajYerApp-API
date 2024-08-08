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
            var adverts = await _context.Advertisements.Where(a => a.AdvIsActive == true).ToListAsync();

            foreach (var advert in adverts)
            {
                if (advert.AdvExpirationDate < DateTime.Now)
                {
                    advert.AdvIsActive = false;
                    _context.Entry(advert).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                    continue;
                }

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

        #region Kullanıcı için ilan kaydedip silme
        [HttpPost("UserSaveDeleteAdvert")]
        public async Task<ActionResult> UserSaveDeleteAdvert([FromBody] UserSaveAdvertModel saveAdvert)
        {
            var advert = _context.UsersSavedAdverts.Where(u => u.UserId == saveAdvert.UserId && u.AdvertId == saveAdvert.AdvertId).FirstOrDefault();
            if (advert != null)
            {
                _context.UsersSavedAdverts.Remove(advert);
                await _context.SaveChangesAsync();
            }
            else
            {
                var adv = await _context.Advertisements.FindAsync(saveAdvert.AdvertId);
                if (adv != null && adv.AdvIsActive)
                {
                    var savedAdvert = new UsersSavedAdvert
                    {
                        UserId = saveAdvert.UserId,
                        AdvertId = saveAdvert.AdvertId
                    };

                    _context.UsersSavedAdverts.Add(savedAdvert);
                    await _context.SaveChangesAsync();
                }                
            }
            return Ok();
        }
        #endregion

        #region Kullanıcnın İlan Başvuru Bilgisini Alma
        [HttpGet("GetUserIsApplied/{userId}/{advertId}")]
        public async Task<ActionResult> GetUserIsApplied(int userId, int advertId)
        {
            bool isApplied = await _context.Applications.AnyAsync(a => a.UserId == userId && a.AdvertId == advertId);

            return Ok(isApplied);
        }
        #endregion

        #region Kullanıcının Kaydettiği İlanları Listeleme
        [HttpGet("ListUsersSavedAdverts/{userId}")]
        public async Task<ActionResult> ListUsersSavedAdverts(int userId)
        {
            
            var savedAdverts = await _context.UsersSavedAdverts
                .Where(u => u.UserId == userId)
                .Select(u => u.Advert)
                .ToListAsync();

            if (savedAdverts == null || !savedAdverts.Any())
            {
                return NotFound("Kullanıcının kaydettiği ilan bulunamadı.");
            }

            
            var advertDtos = new List<Advertisement>();

            foreach (var advert in savedAdverts)
            {
                if (advert.AdvExpirationDate < DateTime.Now)
                {
                    advert.AdvIsActive = false;
                    _context.Entry(advert).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

                var company = new Company();
                company = await _context.Companies.FindAsync(advert.CompId);
                company.Advertisements = null;
                advert.Comp = company;

                var advertDto = new Advertisement
                {
                    AdvertId=advert.AdvertId,
                    CompId = advert.CompId,
                    AdvTitle = advert.AdvTitle,
                    AdvAdressTitle = advert.AdvAdressTitle,
                    AdvAdress = advert.AdvAdress,
                    AdvIsActive = advert.AdvIsActive,
                    AdvWorkType = advert.AdvWorkType,
                    AdvDepartment = advert.AdvDepartment,
                    AdvJobDesc = advert.AdvJobDesc,
                    AdvQualifications = advert.AdvQualifications,
                    AdvAddInformation = advert.AdvAddInformation,
                    AdvExpirationDate = advert.AdvExpirationDate,
                    AdvPhoto = advert.AdvPhoto,
                    AdvPaymentInfo = advert.AdvPaymentInfo,
                    Comp = company 
                };

                advertDtos.Add(advertDto);
            }

            return Ok(advertDtos);
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

            if (advert.AdvExpirationDate < DateTime.Now)
            {
                advert.AdvIsActive = false;
                _context.Entry(advert).State = EntityState.Modified;
                await _context.SaveChangesAsync();
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
                if (advert.AdvExpirationDate < DateTime.Now)
                {
                    advert.AdvIsActive = false;
                    _context.Entry(advert).State = EntityState.Modified;
                    await _context.SaveChangesAsync();
                }

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

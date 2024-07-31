using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using StajYerApp_API.Models;

namespace StajYerApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CityController : ControllerBase
    {
        private readonly Db6761Context _context;

        /// <summary>
        /// UserController Db6761Context ile başlatır
        /// </summary>
        /// <param name="context">Uygulamanın veritabanı bağlantısı yapılıyor</param>
        public CityController(Db6761Context context)
        {
            _context = context;
        }

        #region Şehirleri Listeleme
        [HttpGet("ListCities")]
        public async Task<ActionResult> ListCities()
        {
            var cities = await _context.Cities.ToListAsync();

            if (cities == null)
            {
                return NotFound();
            }

            return Ok(cities);
        }
        #endregion

        #region İlçeleri Listeleme
        [HttpGet("ListDistricts/{cityId}")]
        public async Task<ActionResult> ListDistricts(int cityId)
        {
            var districts = await _context.Districts.Where(c => c.CityId == cityId).ToListAsync();

            if (districts == null)
            {
                return NotFound();
            }

            return Ok(districts);
        }
        #endregion

        #region Vergi Dairesi Listeleme
        [HttpGet("ListTaxOffices/{cityId}")]
        public async Task<ActionResult> ListTaxOffices(int cityId)
        {
            var taxOffices = await _context.TaxOffices.Where(c => c.CityId == cityId).ToListAsync();

            if (taxOffices == null)
            {
                return NotFound();
            }

            return Ok(taxOffices);
        }
        #endregion

    }
}

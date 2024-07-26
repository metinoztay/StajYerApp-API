using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StajYerApp_API.Models;

namespace StajYerApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TestController : ControllerBase
    {
        private readonly Db6761Context _context;

        public TestController(Db6761Context context)
        {
            _context = context;
        }
        [HttpGet]
        public IActionResult Test(int id)
        {
            string testValue;

            try
            {
                testValue = _context.TblTests.FirstOrDefault(x => x.Id == id)?.TestKolon;
            }
            catch (Exception ex)
            {
                // Handle exceptions or log them
                return StatusCode(500, "An error occurred while fetching the data.");
            }

            if (string.IsNullOrEmpty(testValue))
            {
                return NotFound("No data found for the given id.");
            }

            return Ok(testValue);
        }
    }
}

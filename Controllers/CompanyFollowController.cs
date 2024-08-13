using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StajYerApp_API.Models;

namespace StajYerApp_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyFollowController : ControllerBase
    {
        private readonly Db6761Context _context;

        public CompanyFollowController(Db6761Context context)
        {
            _context = context;
        }


    }
}

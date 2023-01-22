using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace MarkelClaimsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CompanyController : ControllerBase
    {
        private readonly MarkelTestDbContext _context;
        private readonly ILogger<CompanyController> _logger;
        public CompanyController(MarkelTestDbContext context, ILogger<CompanyController> logger)
        {
            this._context = context;
            _logger = logger;
        }

        [HttpGet("GetCompanyByCompanyName")]
        public async Task<IActionResult> GetCompanyByCompanyName(string CompanyName)
        {
            _logger.LogInformation("Calling GetCompanyByCompanyName");

            var companyDetails = await _context.Companies.
                                    Include(cl => cl.Claims).Where(co => co.Name == CompanyName).FirstOrDefaultAsync();

            if (companyDetails == null)
                return NotFound();

            var companyDetailsWithActiveIndicator = new
            {
                companyDetails = companyDetails,
                activePolicyIndicator = (Convert.ToDateTime(companyDetails.InsuranceEndDate).Date - DateTime.Now.Date).TotalDays > 0 ? 1 : 0
            };

            _logger.LogInformation("Returning response from GetCompanyByCompanyName");

            return Ok(companyDetailsWithActiveIndicator);
        }
    }
}

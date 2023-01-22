using Microsoft.AspNetCore.Mvc;

namespace MarkelClaimsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ClaimsController : ControllerBase
    {
        private readonly MarkelTestDbContext _context;
        private readonly ILogger<ClaimsController> _logger;
        public ClaimsController(MarkelTestDbContext context, ILogger<ClaimsController> logger)
        {
            this._context = context;
            _logger = logger;
        }

        [HttpGet("GetClaimsForACompany")]
        public async Task<IActionResult> GetClaimsForACompany(string CompanyName)
        {
            _logger.LogInformation("Calling GetClaimsForACompany");

            var claimDetailsForCompany = await _context.Claims.
                                   Include(cl => cl.Company).Where(co => co.Company.Name == CompanyName).ToListAsync();

            if (claimDetailsForCompany == null)
                return NotFound();
            else
            {
                _logger.LogInformation("Returning response from GetClaimsForACompany");
                return Ok(claimDetailsForCompany);
            }
        }

        [HttpPut("UpdateClaim")]
        public async Task<IActionResult> UpdateClaim(Claim claimModel)
        {
            _logger.LogInformation("Calling UpdateClaim");

            if (!ModelState.IsValid)
                return BadRequest("Not a valid model");

            var existingClaim = _context.Claims.Where(cl => cl.Ucr == claimModel.Ucr).FirstOrDefault<Claim>();

            if (existingClaim != null)
            {
                existingClaim.IncurredLoss = claimModel.IncurredLoss;
                existingClaim.AssuredName = claimModel.AssuredName;
                existingClaim.ClaimDate = claimModel.ClaimDate;
                existingClaim.LossDate = claimModel.LossDate;

                _context.SaveChanges();
            }
            else
                return NotFound();

            _logger.LogInformation("Returning response from UpdateClaim");

            return Ok(await _context.Claims.
                        Include(cl => cl.Company).Where(co => co.Ucr == claimModel.Ucr).FirstOrDefaultAsync());
        }

        [HttpGet("GetClaimDetailsByClaimNumber")]
        public async Task<IActionResult> GetClaimDetailsByClaimNumber(string ClaimNumber)
        {
            _logger.LogInformation("Calling GetClaimDetailsByClaimNumber");

            var claimDetails = await _context.Claims.
                                Include(cl => cl.Company).Where(co => co.Ucr == ClaimNumber).FirstOrDefaultAsync();

            if (claimDetails == null)
                return NotFound();

            var claimDetailsWithDays = new
            {
                claimDetails = claimDetails,
                ageOfClaim = (DateTime.Now.Date - Convert.ToDateTime(claimDetails.ClaimDate).Date).TotalDays
            };

            _logger.LogInformation("Returning response from GetClaimDetailsByClaimNumber");

            return Ok(claimDetailsWithDays);
        }

    }
}

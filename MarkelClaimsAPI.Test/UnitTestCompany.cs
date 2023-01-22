using MarkelClaimsAPI.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Logging;

namespace MarkelClaimsAPI.Test
{
    public class UnitTestCompany
    {
        private DbContextOptions<MarkelTestDbContext> _options;

        [SetUp]
        public void Setup()
        {
            _options = new DbContextOptionsBuilder<MarkelTestDbContext>()
                .UseInMemoryDatabase(databaseName: "MarkelTestDB")
                .Options;

            using (var context = new MarkelTestDbContext(_options))
            {
                var claim = new Claim()
                {
                    Ucr = "CL0001",
                    CompanyId = 1,
                    ClaimDate = DateTime.Now,
                    LossDate = DateTime.Now,
                    AssuredName = "ABC Fraklin",
                    IncurredLoss = 500,
                    Closed = false,
                    Company = new Company
                    {
                        Id = 1,
                        Name = "A Insurance Ltd",
                        Address1 = "A1 Street",
                        Postcode = "AB12BA",
                        Country = "UK",
                        Active = true,
                        InsuranceEndDate = DateTime.Now.AddDays(30)
                    }
                };

                context.SaveChanges();
            }
        }

        [Test]
        public void GetCompanyByCompanyName_Test()
        {
            // Arrange
            using var context = new MarkelTestDbContext(_options);

            ILogger<CompanyController> logger = new Logger<CompanyController>(new NullLoggerFactory());

            // Act
            CompanyController companyController = new CompanyController(context, logger);
            var company = companyController.GetCompanyByCompanyName("A Insurance Ltd");

            // Assert
            Assert.IsNotNull(company);

            Assert.Pass();
        }
    }
}

using System;
using System.Threading.Tasks;
using backend;
using backend.Entities;
using backend.Models;
using backend.Services;
using Microsoft.EntityFrameworkCore;
using Xunit; // Added to enable xUnit test framework

namespace UrlServices.Tests.UrlShorteningTests
{
    public class UrlShorteningServiceTests
    {
        private UrlsDB GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<UrlsDB>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // unique DB for each test
                .Options;

            return new UrlsDB(options);
        }

        [Fact]
        public async Task GetOrCreateCodeForUrl_ReturnsExistingCode_IfExists()
        {
            // Arrange - Get variables/classes/functions/data needed for the test
            var dbContext = GetInMemoryDbContext();
            var existingCode = "abc123";
            var url = "https://example.com";

            dbContext.ShortenedUrls.Add(new ShortenedUrl
            {
                Id = Guid.NewGuid(),
                LongUrl = url,
                Code = existingCode,
                ShortUrl = $"http://localhost/api/{existingCode}",
                UTCTime = DateTime.UtcNow
            });
            await dbContext.SaveChangesAsync();

            var service = new UrlShortningService(dbContext);

            // Act -  Execute the code that is being tested
            var resultCode = await service.GetOrCreateCodeForUrl(url);

            // Assert - Check if the code behaves as expected
            Console.WriteLine($"Retrieved prev_code + Code: {existingCode} + {resultCode}");
            Assert.Equal(existingCode, resultCode);
        }

        [Fact]
        public async Task GetOrCreateCodeForUrl_GeneratesNewCode_IfNotExists()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var newUrl = "https://newexample.com";
            var service = new UrlShortningService(dbContext);

            // Act
            var code = await service.GetOrCreateCodeForUrl(newUrl);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(code));
            Console.WriteLine($"Generated new code for URL: {code}");
            Assert.Equal(UrlShortningService.ShortLinkLength, code.Length);
        }

    }
}

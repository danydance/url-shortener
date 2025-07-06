using System;
using System.Threading.Tasks;
using backend;
using backend.Entities;
using backend.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace UrlServices.Tests.RedirectTests
{
    /// Unit tests for the RedirectService class.
    public class RedirectServiceTests
    {
        /// Creates a new database context for testing purposes.
        private UrlsDB GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<UrlsDB>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new UrlsDB(options);
        }

        /// Test: Returns a redirect result when a matching short code exists in the database.
        [Fact]
        public async Task GetLongUrlByCode_ReturnsCorrectUrl_WhenExists()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var code = "abc123";
            var longUrl = "https://example.com";

            dbContext.ShortenedUrls.Add(new ShortenedUrl
            {
                Id = Guid.NewGuid(),
                Code = code,
                LongUrl = longUrl,
                ShortUrl = $"http://localhost/api/{code}",
                UTCTime = DateTime.UtcNow
            });

            await dbContext.SaveChangesAsync();
            var service = new RedirectService(dbContext);

            // Act
            var result = await service.GetRedirectResultByCode(code);

            // Assert
            var redirectResult = Assert.IsType<RedirectHttpResult>(result); // Should return a redirect
            Assert.Equal(longUrl, redirectResult.Url); // Should redirect to the correct long URL
        }

        /// Test: Returns a NotFound result when the short code does not exist in the database.
        [Fact]
        public async Task GetLongUrlByCode_ReturnsNull_WhenCodeDoesNotExist()
        {
            // Arrange
            var dbContext = GetInMemoryDbContext();
            var service = new RedirectService(dbContext);

            // Act
            var result = await service.GetRedirectResultByCode("notfound");

            // Assert
            Assert.IsType<NotFound>(result); // Should return 404 Not Found
        }
    }
}

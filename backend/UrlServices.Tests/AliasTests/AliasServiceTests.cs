﻿using backend.Entities;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Features;
using Xunit;
using System.Security.Claims;
using Microsoft.AspNetCore.Http.Extensions;
using backend;
using Microsoft.AspNetCore.Http.HttpResults;

namespace UrlServices.Tests.AliasTests
{
    public class AliasServiceTests
    {
        /// Creates a new database context for testing purposes.
        private UrlsDB GetInMemoryDbContext()
        {
            var options = new DbContextOptionsBuilder<UrlsDB>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;

            return new UrlsDB(options);
        }

        /// Mocks/Fakes an HTTP context with a basic localhost request setup.
        private DefaultHttpContext GetMockHttpContext()
        {
            return new DefaultHttpContext
            {
                Request =
                {
                    Scheme = "http",
                    Host = new HostString("localhost")
                }
            };
        }

        /// Test: alias is created when it does not already exist in the database.
        [Fact]
        public async Task CreateOrChecksAlias_CreatesNewAlias_WhenNotExists()
        {
            // Arrange
            var db = GetInMemoryDbContext();
            var service = new AliasService(db);
            var httpContext = GetMockHttpContext();
            var request = new AliasRequest { Url = "https://google.com", Alias = "goog" };

            // Act
            var result = await service.CreateOrChecksAlias(request, httpContext);

            // Assert
            Assert.IsType<Ok<string>>(result); // Should return 200 OK
        }

        /// Test: alias creation fails if the alias is already used for a different URL.
        [Fact]
        public async Task CreateOrChecksAlias_ReturnsBadRequest_WhenAliasTakenByOtherUrl()
        {
            var db = GetInMemoryDbContext();
            var existing = new ShortenedUrl
            {
                Id = Guid.NewGuid(),
                Code = "used",
                LongUrl = "https://something.com",
                ShortUrl = "http://localhost/api/used",
                UTCTime = DateTime.UtcNow
            };
            db.ShortenedUrls.Add(existing);
            await db.SaveChangesAsync();

            var service = new AliasService(db);
            var httpContext = GetMockHttpContext();
            var request = new AliasRequest { Url = "https://other.com", Alias = "used" };

            var result = await service.CreateOrChecksAlias(request, httpContext);

            Assert.IsType<BadRequest<string>>(result); // Should return 400 Bad Request
        }

        /// Test: alias creation fails when the provided URL is not valid.
        [Fact]
        public async Task CreateOrChecksAlias_ReturnsBadRequest_WhenUrlIsInvalid()
        {
            var db = GetInMemoryDbContext();
            var service = new AliasService(db);
            var httpContext = GetMockHttpContext();
            var request = new AliasRequest { Url = "not-a-valid-url", Alias = "alias" };

            var result = await service.CreateOrChecksAlias(request, httpContext);

            Assert.IsType<BadRequest<string>>(result); // Should return 400 Bad Request
        }
    }
}

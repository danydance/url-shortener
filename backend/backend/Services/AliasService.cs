using Azure.Core;
using backend.Entities;
using backend.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class AliasService
    {
        private readonly UrlsDB _dbContext;
        public AliasService(UrlsDB dbcontext)
        {
            _dbContext = dbcontext;
        }
        public async Task<IResult> CreateOrChecksAlias(AliasRequest request, HttpContext httpContext)
        {
            // Checks if the provided URL is valid
            if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
            {
                return Results.BadRequest("This URL is invalid.");
            }

            // Check if this long URL already exists
            var existingByUrl = await _dbContext.ShortenedUrls.FirstOrDefaultAsync(s => s.LongUrl == request.Url);
            if (existingByUrl != null)
            {
                // If exists then update the alias of the same URL
                if (existingByUrl.Code != request.Alias)
                {
                    // Checks if the alias is taken and that it is not the same URL
                    var aliasTaken = await _dbContext.ShortenedUrls.AnyAsync(s => s.Code == request.Alias && s.LongUrl != request.Url);
                    if (aliasTaken)
                    {
                        return Results.BadRequest("Alias is already taken.");
                    }

                    existingByUrl.Code = request.Alias;
                    existingByUrl.ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{request.Alias}";
                    await _dbContext.SaveChangesAsync();
                }
                return Results.Ok(existingByUrl.ShortUrl);
            }

            // Checks if alias already exists
            var aliasExists = await _dbContext.ShortenedUrls.AnyAsync(s => s.Code == request.Alias);
            if (aliasExists)
            {
                return Results.BadRequest("Alias is already taken.");
            }

            // Creates new URL with alias
            var shortenedUrl = new ShortenedUrl
            {
                Id = Guid.NewGuid(),
                LongUrl = request.Url,
                Code = request.Alias,
                ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{request.Alias}",
                UTCTime = DateTime.UtcNow
            };

            _dbContext.ShortenedUrls.Add(shortenedUrl);
            await _dbContext.SaveChangesAsync();

            return Results.Ok(shortenedUrl.ShortUrl);
        }
    }
}

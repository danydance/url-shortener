using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    // Redirects requests to the long URL from the short url in the database
    public class RedirectService
    {
        private readonly UrlsDB _dbContext;
        public RedirectService(UrlsDB dbcontext)
        {
            _dbContext = dbcontext;
        }

        public async Task<IResult> GetRedirectResultByCode(string code)
        {
            // Attempt to find a URL mapping with the specified short code
            var shortenedUrl = await _dbContext.ShortenedUrls.FirstOrDefaultAsync(s => s.Code == code);

            // If not found, return 404 Not Found. else, redirect to the long URL
            return shortenedUrl is null
                ? Results.NotFound()
                : Results.Redirect(shortenedUrl.LongUrl);
        }
    }
}

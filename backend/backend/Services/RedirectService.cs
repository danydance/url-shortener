using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;

namespace backend.Services
{
    public class RedirectService
    {
        private readonly UrlsDB _dbContext;
        public RedirectService(UrlsDB dbcontext)
        {
            _dbContext = dbcontext;
        }
        public async Task<IResult> GetRedirectResultByCode(string code)
        {
            var shortenedUrl = await _dbContext.ShortenedUrls.FirstOrDefaultAsync(s => s.Code == code);
            return shortenedUrl is null
                ? Results.NotFound()
                : Results.Redirect(shortenedUrl.LongUrl);
        }
    }
}

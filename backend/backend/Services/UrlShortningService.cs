using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace backend.Services
{
    public class UrlShortningService
    {
        public const int ShortLinkLength = 6; // The length of the short link chars 

        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        private readonly Random _random = new();

        private readonly UrlsDB _dbContext;
        public UrlShortningService(UrlsDB dbcontext)
        {
            _dbContext = dbcontext;
        }
        public async Task<string> GenerateUniqCode()
        {
            var codeChars = new char[ShortLinkLength];
            while (true)
            {
                for (var i = 0; i < ShortLinkLength; i++)
                {
                    int randomIndex = _random.Next(Alphabet.Length - 1);

                    codeChars[i] = Alphabet[randomIndex];
                }

                var code = new string(codeChars);

                if (!await _dbContext.ShortenedUrls.AnyAsync(s => s.Code == code))
                {
                    return code;
                }
            }
        }
        public async Task<string> GetOrCreateCodeForUrl(string longUrl)
        {
            // 1. Check if the long URL already has a short code in the DB
            var existingEntry = await _dbContext.ShortenedUrls
                .FirstOrDefaultAsync(s => s.LongUrl == longUrl);

            if (existingEntry != null)
            {
                // Return the existing short code
                return existingEntry.Code;
            }

            // 2. If not found, generate a new unique code
            var codeChars = new char[ShortLinkLength];
            while (true)
            {
                for (var i = 0; i < ShortLinkLength; i++)
                {
                    int randomIndex = _random.Next(Alphabet.Length);
                    codeChars[i] = Alphabet[randomIndex];
                }

                var code = new string(codeChars);

                if (!await _dbContext.ShortenedUrls.AnyAsync(s => s.Code == code))
                {
                    // Optionally: save the new code and URL here, or let caller do it
                    return code;
                }
            }
        }

    }
}
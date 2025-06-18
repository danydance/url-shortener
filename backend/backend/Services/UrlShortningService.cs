using backend.Entities;
using backend.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xunit;

namespace backend.Services
{
    public class UrlShortningService
    {
        public const int ShortLinkLength = 6; // The length of the short link chars 

        // The characters used to generate the short link code
        private const string Alphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";

        // Random number generator to create unique short codes
        private readonly Random _random = new();

        // The database context for accessing the shortened URLs
        private readonly UrlsDB _dbContext;
        public UrlShortningService(UrlsDB dbcontext)
        {
            _dbContext = dbcontext;
        }
        /// <summary>
        /// Returns an existing short code for the given long URL,
        /// or generates a new unique code if one doesn't exist.
        /// </summary>
        public async Task<string> GetOrCreateCodeForUrl(string longUrl)
        {
            // Check if the long URL already has an associated short code
            var existingEntry = await _dbContext.ShortenedUrls
                .FirstOrDefaultAsync(s => s.LongUrl == longUrl);

            if (existingEntry != null)
            {
                // If found, return the existing code
                return existingEntry.Code;
            }

            // If not found, generate a new unique code
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
                    // Return the new unique code
                    return code;
                }
                // If not unique, loop again to try another random code
            }
        }

    }
}
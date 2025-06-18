using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Entities;
using backend.Services;


namespace backend
{
    /// The Entity Framework Core database context for the URL shortener application.
    public class UrlsDB : DbContext
    {
        // <summary>
        // Constructor that receives options for configuring the database context.
        // <summary>
        public UrlsDB(DbContextOptions<UrlsDB> options) : base(options) { }

        /// The DbSet representing the collection of shortened URLs in the database.
        public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

        /// <summary>
        // Configures the model for the UrlsDB context.
        /// </summary>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortenedUrl>(builder =>
            {
                // Set the maximum length of the Code
                builder.Property(s => s.Code).HasMaxLength(UrlShortningService.ShortLinkLength);
                //  Ensure that each Code is unique to avoid duplicate short links
                builder.HasIndex(s => s.Code).IsUnique();
            });
        }
    }
}
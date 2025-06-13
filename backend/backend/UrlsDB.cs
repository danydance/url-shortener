using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using backend.Entities;
using backend.Services;


namespace backend
{
    public class UrlsDB : DbContext
    {

        public UrlsDB(DbContextOptions<UrlsDB> options) : base(options) { }

        public DbSet<ShortenedUrl> ShortenedUrls { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ShortenedUrl>(builder =>
            {
                builder.Property(s => s.Code).HasMaxLength(UrlShortningService.ShortLinkLength);
                builder.HasIndex(s => s.Code).IsUnique();
            });
        }
    }
}
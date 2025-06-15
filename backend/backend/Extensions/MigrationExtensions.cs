
using Microsoft.EntityFrameworkCore;
using backend;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Web.Api.Extensions
{
    public static class MigrationExtensions
    {
        public static void ApplyMigrations(this WebApplication app)
        {
            using var scope = app.Services.CreateScope();

            var dbContext = scope.ServiceProvider.GetRequiredService<UrlsDB>();

            dbContext.Database.Migrate(); // Ensures database creation and migration (abilities)
        }
    }
}

using backend.Models;
using Microsoft.EntityFrameworkCore;
using backend;
using backend.Services;
using backend.Entities;
using Web.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<UrlsDB>(o => o.UseSqlite(builder.Configuration.GetConnectionString("Database")));

builder.Services.AddScoped<UrlShortningService>();

// Allows React app to communicate
builder.Services.AddControllers();
builder.Services.AddAuthorization();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowReactApp",
        policy => policy
            .WithOrigins("http://localhost:5000") 
            .AllowAnyHeader()
            .AllowAnyMethod());
});



var app = builder.Build();

app.UseCors("AllowReactApp"); // allows React app

app.UseAuthorization();
app.MapControllers();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();

    app.ApplyMigrations(); // This automatically applies migrations on startup and creates the database if it doesn't exist
}


app.MapPost("api/shorten", async (
    ShortenUrlRequest request,
    UrlShortningService urlShorteningService,
    UrlsDB dbContext,
    HttpContext httpContext) =>
{
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("This URL is invalid.");
    }

    var code = await urlShorteningService.GetOrCreateCodeForUrl(request.Url);

    var existing = await dbContext.ShortenedUrls.FirstOrDefaultAsync(s => s.Code == code);

    if (existing == null)
    {
        var shortenedUrl = new ShortenedUrl
        {
            Id = Guid.NewGuid(),
            LongUrl = request.Url,
            Code = code,
            ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{code}",
            UTCTime = DateTime.Now
        };
        dbContext.ShortenedUrls.Add(shortenedUrl);
        await dbContext.SaveChangesAsync();

        return Results.Ok(shortenedUrl.ShortUrl);
    }

    return Results.Ok(existing.ShortUrl);
    
});

app.MapGet("api/{code}", async (string code, UrlsDB dbContext) =>
{
    var shortenedUrl = await dbContext.ShortenedUrls.FirstOrDefaultAsync(s => s.Code == code);

    if (shortenedUrl is null)
    {
        return Results.NotFound();
    }
    return Results.Redirect(shortenedUrl.LongUrl);
});

app.MapPost("api/alias", async (
    AliasRequest request,
    UrlsDB dbContext,
    HttpContext httpContext) =>
{
    // Checks if the provided URL is valid
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _)) 
    {
        return Results.BadRequest("This URL is invalid.");
    }

    // Check if this long URL already exists
    var existingByUrl = await dbContext.ShortenedUrls.FirstOrDefaultAsync(s => s.LongUrl == request.Url);
    if (existingByUrl != null)
    {
        // If exists then update the alias of the same URL
        if (existingByUrl.Code != request.Alias)
        {
            // Checks if the alias is taken and that it is not the same URL
            var aliasTaken = await dbContext.ShortenedUrls.AnyAsync(s => s.Code == request.Alias && s.LongUrl != request.Url);
            if (aliasTaken)
            {
                return Results.BadRequest("Alias is already taken.");
            }

            existingByUrl.Code = request.Alias;
            existingByUrl.ShortUrl = $"{httpContext.Request.Scheme}://{httpContext.Request.Host}/api/{request.Alias}";
            await dbContext.SaveChangesAsync();
        }
        return Results.Ok(existingByUrl.ShortUrl);
    }

    // Checks if alias already exists
    var aliasExists = await dbContext.ShortenedUrls.AnyAsync(s => s.Code == request.Alias);
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

    dbContext.ShortenedUrls.Add(shortenedUrl);
    await dbContext.SaveChangesAsync();

    return Results.Ok(shortenedUrl.ShortUrl);
});



app.UseHttpsRedirection();



app.Run();



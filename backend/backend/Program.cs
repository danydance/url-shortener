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

// Configure Entity Framework Core to use SQLite and register UrlsDB as the database context
builder.Services.AddDbContext<UrlsDB>(o => o.UseSqlite(builder.Configuration.GetConnectionString("Database")));


builder.Services.AddScoped<UrlShortningService>();

// Allows React app to communicate
builder.Services.AddControllers();
builder.Services.AddAuthorization();

// Configure CORS to allow requests from the React frontend (running at http://localhost:5000)
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

// POST /api/shorten
// Shortens a given URL and returns a generated short link
app.MapPost("api/shorten", async (
    ShortenUrlRequest request,
    UrlShortningService urlShorteningService,
    UrlsDB dbContext,
    HttpContext httpContext) =>
{
    // Validate the incoming URL
    if (!Uri.TryCreate(request.Url, UriKind.Absolute, out _))
    {
        return Results.BadRequest("This URL is invalid.");
    }

    // Generate or fetch an existing short code for the URL
    var code = await urlShorteningService.GetOrCreateCodeForUrl(request.Url);

    // Check if a record already exists for this code
    var existing = await dbContext.ShortenedUrls.FirstOrDefaultAsync(s => s.Code == code);

    if (existing == null)
    {
        // Create and save a new shortened URL record
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

    // Return the existing short URL if already in the database
    return Results.Ok(existing.ShortUrl);
    
});

// GET /api/{code}
// Redirects to the original long URL if the code exists
app.MapGet("api/{code}", async (string code, UrlsDB dbContext) =>
{
    RedirectService redirectService = new(dbContext);
    return await redirectService.GetRedirectResultByCode(code);
});

// POST /api/alias
// Allows the user to define a custom alias instead of a generated one
app.MapPost("api/alias", async (
    AliasRequest request,
    UrlsDB dbContext,
    HttpContext httpContext) =>
{
    var aliasService = new AliasService(dbContext);
    return await aliasService.CreateOrChecksAlias(request, httpContext);
});


// Redirect HTTP requests to HTTPS
app.UseHttpsRedirection();


// Start application
app.Run();



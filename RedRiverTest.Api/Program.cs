using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using RedRiverTest.Api.Auth;
using Scalar.AspNetCore;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

// JWT settings (Issuer, Audience, SigningKey)
builder.Services.AddOptions<JwtOptions>()
	.Bind(builder.Configuration.GetSection(JwtOptions.SectionName))
	.Validate(o => !string.IsNullOrWhiteSpace(o.SigningKey), $"{JwtOptions.SectionName}:SigningKey is required")
	.ValidateOnStart();

var jwtOptions = builder.Configuration.GetSection(JwtOptions.SectionName).Get<JwtOptions>() ?? new JwtOptions();
var signingKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtOptions.SigningKey));

builder.Services
	.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
       // Check the token on each request.
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidIssuer = jwtOptions.Issuer,
			ValidateAudience = true,
			ValidAudience = jwtOptions.Audience,
			ValidateIssuerSigningKey = true,
			IssuerSigningKey = signingKey,
			ValidateLifetime = true,
			ClockSkew = TimeSpan.FromSeconds(30)
		};
	});

builder.Services.AddAuthorization();

// Simple in-memory users + token creator.
builder.Services.AddSingleton<IUserStore, InMemoryUserStore>();
builder.Services.AddSingleton<IJwtTokenService, JwtTokenService>();

// In-memory database for now.
builder.Services.AddDbContext<RedRiverTest.Api.Data.AppDbContext>(options =>
	options.UseSqlite("Data Source=books.db"));

var app = builder.Build();
using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<RedRiverTest.Api.Data.AppDbContext>();
    db.Database.Migrate();
	if (!db.Books.Any())
	{
       // Seed one book so the list is not empty.
		db.Books.Add(new RedRiverTest.Api.Models.Book
		{
			Title = "Clean Code",
			Author = "Robert Martin",
			PublishedDate = new DateTime(2010, 8, 7)
		});
		db.SaveChanges();
	}
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
	app.MapOpenApi();
 app.MapScalarApiReference(options =>
	{
		options.WithTitle("RedRiverTest API");
	});
}

app.UseHttpsRedirection();

// Read the JWT token from the Authorization header.
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

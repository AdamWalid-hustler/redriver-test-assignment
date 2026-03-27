using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

builder.Services.AddDbContext<RedRiverTest.Api.Data.AppDbContext>(options =>
	options.UseInMemoryDatabase("AppDb"));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
	var db = scope.ServiceProvider.GetRequiredService<RedRiverTest.Api.Data.AppDbContext>();
	if (!db.Books.Any())
	{
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

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();

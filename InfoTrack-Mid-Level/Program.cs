using InfoTrack_Mid_Level.Data;
using InfoTrack_Mid_Level.Repository;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers().AddNewtonsoftJson();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase(builder.Configuration.GetConnectionString("InfoTrackDB")));
builder.Services.AddRateLimiter(_ => _
  .AddFixedWindowLimiter(policyName: "rateLimit", options =>
  {
      // Set the limit to 10 requests per minute
      options.PermitLimit = 4;
      // Set the time window to 1 minute
      options.Window = TimeSpan.FromSeconds(1);
  }));
builder.Services.AddScoped<IBookingRepository, BookingRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

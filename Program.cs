using InvestmentSimulatorAPI.Models;
using InvestmentSimulatorAPI.Services;
using InvestmentSimulatorAPI.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

#region Logging
builder.Logging.ClearProviders();
builder.Logging.AddConsole();
#endregion

#region SQLITE
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite("Data Source=investment.db"));
#endregion

#region Favourite
builder.Services.AddScoped<FavouriteRepository>();
builder.Services.AddScoped<FavouriteService>();
#endregion

#region Portfolio
builder.Services.AddScoped<PortfolioRepository>();
builder.Services.AddScoped<PortfolioService>();
#endregion

#region Transaction
builder.Services.AddScoped<TransactionRepository>();
builder.Services.AddScoped<TransactionService>();
#endregion

#region Cors
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigin", policy =>
    {
        policy.WithOrigins("http://localhost:3000")
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
#endregion

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowSpecificOrigin");

app.UseAuthorization();

app.MapControllers();

app.Run();

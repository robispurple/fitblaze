using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore;
using FitBlaze.Data;
using FitBlaze.Features.Wiki.Repositories;
using FitBlaze.Features.Wiki.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddControllers();
builder.Services.AddSingleton<WeatherForecastService>();

// Configure database
var connectionString = builder.Configuration.GetConnectionString("WikiDb");
builder.Services.AddDbContext<WikiContext>(options =>
    options.UseSqlite(connectionString));

// Register repositories and services
builder.Services.AddScoped<IPageRepository, SqlitePageRepository>();
builder.Services.AddScoped<SlugService>();
builder.Services.AddScoped<PageService>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.MapControllers();
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

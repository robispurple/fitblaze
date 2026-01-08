using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.EntityFrameworkCore; // Added
using FitBlaze.Data;
using FitBlaze.Features.Wiki.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddSingleton<WeatherForecastService>();
// Added: Configure DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddScoped<PageService>();
builder.Services.AddScoped<IMarkupOrchestrator, MarkupOrchestrator>();
builder.Services.AddScoped<IMarkupRenderer, CommonMarkRenderer>();
builder.Services.AddScoped<IMarkupRenderer, LegacyFitNesseRenderer>();
builder.Services.AddControllers(); // Added for API controllers

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

app.MapControllers(); // Added for API controllers
app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();

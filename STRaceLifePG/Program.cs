using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using STRaceLifePG.Helpers;
using STRaceLifePG.Helpers.Services;
using STRaceLifePG.Interface;
using STRaceLifePG.Models;
using STRaceLifePG.Repository;
using StreetRaceLifeVK.Data;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

builder.Services.AddDbContext<AppContextDb>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddIdentity<User, IdentityRole<Guid>>()
    .AddEntityFrameworkStores<AppContextDb>()
    .AddDefaultTokenProviders();
builder.Services.AddScoped<IClubRepository, ClubRapository>();
builder.Services.AddScoped<IRaceRepository, RaceRepository>();
builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("CloudinarySettings"));
builder.Services.AddScoped<IPhotoSerice, PhotoService>();

builder.Services.AddAuthentication()
    .AddCookie(options =>
    {
        options.LoginPath = "/User/Login";
        options.AccessDeniedPath = "/User/AccessDenied";
    });

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

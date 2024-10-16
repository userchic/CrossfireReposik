using WebApplication1.DataBase;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Net;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services;
//builder.Services.AddTransient()
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<GameContext>(
    options =>
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
    );

builder.Services.AddAuthentication("Cookies")
    .AddCookie(options => 
    {
        options.LoginPath = "/Home/Login";
        options.AccessDeniedPath = "/Home/Main";
    });
builder.Services.AddAuthorization();
var app = builder.Build();

HomeController.user = new WebApplication1.Models.Users() { Role = new WebApplication1.Models.Role { Name = "Никто" } };

app.UseAuthentication();  
// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

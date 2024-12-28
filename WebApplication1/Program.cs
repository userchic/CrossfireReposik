using WebApplication1.DataBase;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Controllers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Net;
using WebApplication1.Models;
using WebApplication1.Reposiotories;
using WebApplication1.Abstractions;
using GameHubSpace;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services;
//builder.Services.AddTransient()
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<GameContext>(
    options =>
    {
        options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"));
    }
    );
builder.Services.AddAuthentication("Cookies")
    .AddCookie(options => 
    {
        options.LoginPath = "/Home/Login";
        options.AccessDeniedPath = "/Home/Main";
    });
builder.Services.AddAuthorization();

builder.Services.AddScoped<RoleRepository>();
builder.Services.AddScoped<ClassRepository>();
builder.Services.AddScoped<IGameRepository, GameRepository>();
builder.Services.AddScoped<ITaskRepository, TaskRepository>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IParticipationsRepository, ParticipationsRepository>();
builder.Services.AddScoped<IShotRepository, ShotsRepository>();
builder.Services.AddScoped<IUserGameStatusRepository, UserGameStatusRepository>();
builder.Services.AddScoped<ITeamRepository, TeamRepository>();
builder.Services.AddScoped<ISent_AnswersRepository, Sent_AnswersRepository>();

var app = builder.Build();


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

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    endpoints.MapHub<GameHub>("game");
});
app.Run();

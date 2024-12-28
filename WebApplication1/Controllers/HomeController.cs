using WebApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using WebApplication1.DataBase;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Mvc.Razor;
using WebApplication1.Reposiotories;
using WebApplication1.Abstractions;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public GameContext db;

		public IGameRepository gameRep;
        public ActionResult Main()
        {
            List<Game> games = (List<Game>)gameRep.GetGames();
            return View("Main",games);
        }
        RoleRepository roleRep;
        ClassRepository classRep;
        public HomeController(GameContext context,RoleRepository roles,ClassRepository classes,IGameRepository games)
        {
            db = context;
            roleRep = roles;
            classRep = classes;
            gameRep = games;
        }
        [HttpPost]
        public ActionResult LoginClick(string Login,string password)
        {
            if (!password.IsNullOrEmpty() & !Login.IsNullOrEmpty())
            {
                var us = db.Users.FirstOrDefault(u => u.Login == Login & u.Password == password);
                if (us is not null)
                {
                    us.Role = db.Roles.FirstOrDefault(x => x.ID == us.RoleID);
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType,us.Login),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType,us.Role.Name)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    HttpContext.SignOutAsync();
                    HttpContext.SignInAsync(claimsPrincipal);
                    return View("Success");
                }
                else
                {
                    ViewBag.Message = "Неверный пароль или логин";
                    return View("Login");
                }
            }
            else
            {
                ViewBag.Message = "Не все поля были заполнены, введите логин И пароль";
                return View("Login");
            }
        }
        public ActionResult Register()
        {
            return View("Register");
        }
        [HttpPost]
        public ActionResult RegisterClick(string Login,string Password,string Name,string SurName,string FatName,string Role,string Class)
        {
            //валидация
            if (Login.Trim().IsNullOrEmpty() || Password.Trim().IsNullOrEmpty() || Name.Trim().IsNullOrEmpty() || SurName.Trim().IsNullOrEmpty() || FatName.Trim().IsNullOrEmpty() || Role.Trim().IsNullOrEmpty() || Class.Trim().IsNullOrEmpty())
            {
                ViewBag.Message = "Какие то поля не заполнены";
                return View("Register");
            }
            if (db.Users.Where(u => u.Login == Login).Any())
            {
                ViewBag.Message = "Пользователь с таким именем уже существует";
                return View("Register");
            }
            if (Password.Length<5)
            {
                ViewBag.Message = "Сделайте пароль более чем 5 симболов";
                return View("Register");
            }
            
            Users user=Users.Create(Login,Password, Name, SurName, FatName, roleRep.GetRole(Role).ID,classRep.GetClass(Class).ID);

            db.Users.Add(user);
            db.SaveChanges();
            return View("Login",user);
        }
        public ActionResult LogOut()
        {
            HttpContext.SignOutAsync();
                return View("Success");
        }


        public ActionResult Login()
        {
            return View("Login");
        }

        public ActionResult Index()
        {
            List<Game> games = db.Games.Select(x => x).ToList();
            return View("Main",games);
        }
        public static bool UserIsAdmin(ClaimsPrincipal User) => User.IsInRole("Администратор");
        public static bool UserIsPlayer(ClaimsPrincipal User) =>  User.IsInRole("Игрок");
    }
}
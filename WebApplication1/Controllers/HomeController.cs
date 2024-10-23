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

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public GameContext db;
        public enum Role
        {
            Administrator,Player,None
        }
        public ActionResult Main()
        {
            return View("Main");
        }
        public HomeController(GameContext context)
        {
            db = context;
        }
        
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
                    return View("Login", new Users() { Login = Login, Password = password });
                }
            }
            else
            {
                ViewBag.Message = "Не все поля были заполнены, введите логин И пароль";
                return View("Login", new Users() { Login = Login, Password = password });
            }
        }
        public ActionResult Register()
        {
            return View("Register");
        }
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
            
            Users user=Users.Create(Login,Password, Name, SurName, FatName, Role,Class);

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
            return View("Login",new Users());
        }

        public ActionResult Index()
        {
            
            return View("Main");
        }
        public static bool UserIsAuthorized(ClaimsPrincipal User) => User.Identity.IsAuthenticated;

        public static bool UserIsAdmin(ClaimsPrincipal User) => User.IsInRole("Администратор");
        public static bool UserIsPlayer(ClaimsPrincipal User) =>  User.IsInRole("Игрок");
    }
}
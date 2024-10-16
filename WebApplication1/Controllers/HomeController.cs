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

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        public static Users user = new Users();
        public static GameContext db;
        public enum Role
        {
            Administrator,Player,None
        }
        public static Role role=Role.None;
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
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimsIdentity.DefaultNameClaimType,us.Login),
                        new Claim(ClaimsIdentity.DefaultRoleClaimType,us.Role.Name)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    HttpContext.SignOutAsync();
                    HttpContext.SignInAsync(claimsPrincipal);
                    ViewBag.Message = "";
                    switch(us.Role.Name)
                    {
                        case "Администратор":
                                role = Role.Administrator;
                                break;
                        case "Игрок":
                            role = Role.Player;
                                break;
                    }
                    user.RoleID= us.Role.ID;
                    return View("Main");
                }
                else
                {
                    ViewBag.Message = "Введены данные несуществующего пользователя, введите другие данные";
                    return View("Login");
                        //View("Login", new Users() { Login = "", Password = "" });
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
        public ActionResult RegisterClick(string Login,string Password,string Name,string SurName,string FatName,string Role)
        {
            if (db.Users.Where(u => u.Login == Login).Any())
            {
                ViewBag.Message = "Пользователь с таким именем уже существует";
                return View("Register");
            }
            if (Login.Trim().IsNullOrEmpty() ||Password.Trim().IsNullOrEmpty() || Name.Trim().IsNullOrEmpty() || SurName.Trim().IsNullOrEmpty() || FatName.Trim().IsNullOrEmpty() || Role.Trim().IsNullOrEmpty())
            {
                ViewBag.Message = "Какие то поля не заполнены";
                return View("Register");
            }
            if (Password.Length<5)
            {
                ViewBag.Message = "Сделайте пароль более чем 5 симболов";
                return View("Register");
            }
            int roleid = db.Roles.FirstOrDefault(el => el.Name == Role).ID;
            
            Users user=Users.Create(Login,Password, Name, SurName, FatName, roleid);
            db.Users.Add(user);
            db.SaveChanges();
            return View("Login");
        }
        public ActionResult LogOut()
        {
            HttpContext.SignOutAsync();

            return View("Main");
        }


        public ActionResult Login()
        {
            return View("Login");
        }

        public ActionResult Index()
        {
            return View("Main");
        }
    }
}
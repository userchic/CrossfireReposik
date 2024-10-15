using WebApplication3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Authentication;
using WebApplication3.DataBase;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication3.Controllers
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
                        new Claim(ClaimsIdentity.DefaultRoleClaimType,us.Role)
                    };
                    var claimsIdentity = new ClaimsIdentity(claims, "Cookies");
                    var claimsPrincipal = new ClaimsPrincipal(claimsIdentity);
                    HttpContext.SignOutAsync();
                    HttpContext.SignInAsync(claimsPrincipal);
                    ViewBag.Message = "";
                    switch(us.Role)
                    {
                        case "Администратор":
                                role = Role.Administrator;
                                break;
                        case "Игрок":
                            role = Role.Player;
                                break;
                    }
                    user.Role=role.ToString();
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

        public ActionResult LogOut()
        {
            HttpContext.SignOutAsync();
            user = new Users();
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
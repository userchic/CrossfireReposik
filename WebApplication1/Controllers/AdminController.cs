using WebApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication1.DataBase;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.IdentityModel.Tokens;

namespace WebApplication1.Controllers
{
    [Authorize(Roles ="Администратор")]
    public class AdminController : Controller
    {
        public static Game CurrentGame = new Game() ;

        public static GameContext db;
        static Random rand1 = new Random();
        static Random rand2 = new Random();
        public AdminController(GameContext context)
        {
            db = context;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Game()
        {
            return View("Game");
        }
        public ActionResult AddGame()
        {
            return View("GameMake", CurrentGame);
        }
        public ActionResult CancelMakeGame()
        {
            CurrentGame = new Game();
            return View("GameMake", CurrentGame);
        }
        public ActionResult AddTask(Game model)
        {
            UpdateGame(model);
            return View("TaskMake", new Tasks());
        }
        public void UpdateGame(Game game)
        {
            CurrentGame.GameName = game.GameName;
            CurrentGame.ID = game.ID;
            CurrentGame.Lenga = game.Lenga;
            CurrentGame.StartData = game.StartData;
        }
        public ActionResult Cancelmaketask()
        {
            return View("GameMake", CurrentGame);
        }
        public ActionResult Tasks()
        {
            return View("Tasks");
        }
        [HttpGet]
        public ActionResult TaskInfo(string TaskID)
        {
            int ID = int.Parse(TaskID);
            Tasks t = db.Tasks.FirstOrDefault(Task => Task.ID == ID);
            return View("Task", t);
        }
        [HttpPost]
        public ActionResult AddTaskClick(string Text, string Answer)
        {
            if (Text == null || Answer == null || Text.Length == 0 || Answer.Length == 0)
            {
                ViewBag.Message = "Текст или ответ задачи не введены";
                return View("TaskMake", new Tasks() { Answer = Answer, Text = Text });
            }
            else
            {
                db.Tasks.Add(new Tasks() { Answer = Answer, Text = Text });
                ViewBag.Message2 = "Задача создана!";
                return View("TaskMake", new Tasks());
            }
        }
        [HttpPost]
        public ActionResult AddGameClick(string GameName, string StartData, string Lenga, string StartTime,List<int> id, List<bool> tasks)
        {
            DateTime t;
            int num;
            if (!GameName.Trim().IsNullOrEmpty() &&
                StartData.Trim().IsNullOrEmpty() && DateTime.TryParse(StartData, out t) &&
                Lenga.Trim().IsNullOrEmpty() && int.TryParse(Lenga, out num) && num > 0 &&
                StartTime.Trim().IsNullOrEmpty() && DateTime.TryParse(StartTime, out t)
                && tasks.Count > 0 && DateTime.Parse(StartData + " " + StartTime) > DateTime.Now)
            {
                Game game = new Game()
                {
                    GameName = GameName,
                    Lenga = int.Parse(Lenga),
                    StartData = DateTime.Parse(StartData + StartTime)
                };
                for (int i=0;i<tasks.Count;i++)
                {
                    if (tasks[i])
                    {
                        //game.GameTasks db.Tasks.FirstOrDefault(elem => elem.ID == id[i]);
                    }
                }
                db.SaveChanges();
                ViewBag.Message = "Игра создана";
                return View("GameMake", new Game());
            }
            else
            {
                Game g = CurrentGame;
                    ViewBag.Message = "Некоторые поля были неправильно заполнены:";
                    if (GameName == "")
                        ViewBag.Message += "Не указано название игры ";
                    if (StartData == "" || StartTime == "")
                    {
                        if (StartData != "")
                        {
                            if (!DateTime.TryParse(StartData, out t))
                                ViewBag.Message += " Неправильно введена дата проведения";
                        }
                        else
                            ViewBag.Message += "Не указана дата начала игры ";
                        if (StartTime != "")
                        {
                            if (!DateTime.TryParse(StartTime, out t))
                                ViewBag.Message += "Неправильно введено время начала игры";
                        }
                        else
                            ViewBag.Message += "Не указано время начала игры";
                    }
                    else
                    {
                        if (DateTime.Parse(StartData + " " + StartTime) > DateTime.Now)
                            ViewBag.Message += "Указано прошедшее время/дата начала игры";
                    }
                    if (Lenga != "")
                    {
                        if (!int.TryParse(Lenga, out num) || int.Parse(Lenga) <= 0)
                            ViewBag.Message += "Неправильно введена длительность игры ";
                    }
                    else
                        ViewBag.Message += "Не указана длительность игры";

                    ViewBag.Message += " \n Введите информацию в эти поля ещё раз";
                if (tasks.Count == 0)
                    ViewBag.Message += " \n Не создано ни одной задачи! Создайте хотя бы одну задачу для создаваемой игры";
                return View("GameMake", g);
            }
        }
    }
}
using WebApplication3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using WebApplication3.DataBase;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;

namespace WebApplication3.Controllers
{
    [Authorize(Roles ="Администратор")]
    public class AdminController : Controller
    {
        public static Game CurrentGame = new Game() { Tasks = new List<Tasks>() };

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
            CurrentGame = new Game() { Tasks = new List<Tasks>() };
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
            CurrentGame.StartTime = game.StartTime;
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
                CurrentGame.Tasks.Add(new Tasks() { Answer = Answer, Text = Text });
                ViewBag.Message2 = "Задача создана!";
                return View("TaskMake", new Tasks());
            }
        }
        [HttpPost]
        public ActionResult AddGameClick(string GameName, string StartData, string Lenga, string StartTime)
        {
            DateTime t;
            int num;
            GameName = GameName + "";
            StartData = StartData + "";
            Lenga = Lenga + "";
            StartTime = StartTime + "";
            if (GameName.Trim(new char[1] { ' '}) != "" 
                && StartData != "" && DateTime.TryParse(StartData, out t) 
                && Lenga != "" &&  int.TryParse(Lenga, out num) && int.Parse(Lenga)>0
                && StartTime != "" &&  DateTime.TryParse(StartTime, out t)  
                && CurrentGame.Tasks.Count > 0 && DateTime.Parse(StartData + " " + StartTime) > DateTime.Now)
            {
                CurrentGame.GameName = GameName;
                CurrentGame.StartData = DateTime.Parse(StartData + " " + StartTime);
                CurrentGame.Lenga = int.Parse(Lenga);
                Game game = new Game() { GameName = GameName, Lenga = int.Parse(Lenga), StartData = DateTime.Parse(StartData), StartTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, int.Parse(StartTime.Split(new char[1] { ':' })[0]), int.Parse(StartTime.Split(new char[1] { ':' })[1]), 0) };
                db.Games.Add(game);
                foreach (Tasks task in CurrentGame.Tasks)
                {
                    db.Tasks.Add(task);
                    game.Tasks.Add(task);
                }
                CurrentGame.Tasks.Clear();
                db.SaveChanges();
                ViewBag.Message = "Игра создана";
                return View("GameMake",new Game());
            }
            else
            {
                Game g = CurrentGame;
                if (GameName.Trim(new char[1] { ' ' }) == "" 
                    || StartData == "" || !DateTime.TryParse(StartData, out t) 
                    || Lenga == ""     || !int.TryParse(Lenga, out num) || int.Parse(Lenga) > 0
                    || StartTime == "" || !DateTime.TryParse(StartTime, out t) 
                    || DateTime.Parse(StartData+" " + StartTime) > DateTime.Now)
                {
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
                        if (!int.TryParse(Lenga, out num)|| int.Parse(Lenga) <= 0)
                            ViewBag.Message += "Неправильно введена длительность игры ";
                    }
                    else
                        ViewBag.Message += "Не указана длительность игры";

                    ViewBag.Message += " \n Введите информацию в эти поля ещё раз";
                }
                if (CurrentGame.Tasks.Count == 0)
                    ViewBag.Message += " \n Не создано ни одной задачи! Создайте хотя бы одну задачу для создаваемой игры";
                return View("GameMake", g);
            }
        }
    }
}
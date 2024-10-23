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
using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstractions;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Администратор")]
    public class AdminController : Controller
    {
        public Game CurrentGame = new Game();

         Random rand1 = new Random();
         Random rand2 = new Random();
        public ITaskRepository taskRep;
        public IGameRepository gameRep;
        public AdminController(ITaskRepository tasks, IGameRepository games)
        {
            taskRep = tasks;
            gameRep = games;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Game()
        {
            return View("Game");
        }

        public ActionResult AddTask(Game model)
        {
            return View("TaskMake", new Tasks());
        }
        public ActionResult CreateTaskClick(string text, string answer)
        {
            Tasks task = new Tasks() { Text = text, Answer = answer };
            if (!task.Validation())
            {
                ViewBag.Message = "Некоторые поля неправильно заполнены";
                return View("TaskMake");
            }
            taskRep.CreateTask(task);
            taskRep.Save();
            ViewBag.Message2 = "Задача создана";
            return View("TaskMake");
        }
        public ActionResult UpdateTaskClick(int id, string text, string answer)
        {
            Tasks task = taskRep.GetTask(id);
            if (task is null)
            {
                ViewBag.Message = "Задача не существует";
                return View("Tasks");
            }
            if (!task.Validation())
            {
                ViewBag.Message = "Некоторые поля не заполнены";
                return View("TaskEdit");
            }
            taskRep.UpdateTask(task);
            taskRep.Save();
            ViewBag.Message2 = "Задача создана";
            return View("TaskEdit");
        }
        public ActionResult DeleteTaskClick(int taskId)
        {
            Tasks task = taskRep.GetTask(taskId);
            if (task is null)
            {
                ViewBag.Message = "Задача не существует";
                return View("Tasks");
            }
            taskRep.DeleteTask(task);
            taskRep.Save();
            ViewBag.Message = "Задача удалена";
            return View("Tasks");
        }
        public ActionResult AddGame()
        {
            return View("GameMake");
        }
        public ActionResult CancelMakeGame()
        {
            CurrentGame = new Game();
            return View("GameMake", CurrentGame);
        }
        [HttpPost]
        public ActionResult CreateGameClick(string Name, DateTime StartDate,DateTime StartTime, int Lenga)
        {
            if (CurrentGame.Tasks.Count == 0)
            {
                ViewBag.Message = "Задачи не выбраны";
                return View("GameMake");
            }
            CurrentGame.Name = Name;
            CurrentGame.StartData = StartDate.AddHours(StartTime.Hour).AddMinutes(StartTime.Minute).AddSeconds(StartTime.Second);
            CurrentGame.Lenga = Lenga;
            if (!CurrentGame.Validation())
            {
                ViewBag.Message = "Некоторые поля неверно заполнены";
                return View("Admin", "GameMake");
            }
            gameRep.CreateGame(CurrentGame);
            gameRep.Save();
            CurrentGame = new Game();
            return View("GameMake");
        }
        [HttpPut]
        public ActionResult UpdateGameClick(int gameId, string GameName,int Lenga,DateTime StartDate,DateTime StartTime)
        {
            Game game = gameRep.GetGame(gameId);
            if (game is null)
            {
                ViewBag.Message = "Игра не существует";
                return View("Games");
            }
            if (game.Started())
            {
                ViewBag.Message = "Игра уже началась";
                return View("Games");
            }
            game.Name = GameName;
            game.StartData = StartDate.AddHours(StartTime.Hour).AddMinutes(StartTime.Minute).AddSeconds(StartTime.Second);
            game.Lenga = Lenga;
            if (!game.Validation())
            {
                ViewBag.Message = "Некоторые поля неверно заполнены";
                return View( "UpdateGame",game);
            }
            gameRep.UpdateGame(game);
            gameRep.Save();
            return View("Games");
        }
        [HttpDelete]
        public ActionResult DeleteGameClick(int gameId)
        {
            Game game = gameRep.GetGame(gameId);
            if (game is null)
            {
                ViewBag.Message = "Игра не существует";
                return View("Games");
            }
            if (game.Ongoing())
            {
                ViewBag.Message = "Игра идёт";
                return View("Games");
            }
            gameRep.DeleteGame(game);
            gameRep.Save();
            return View("Games");
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
        public ActionResult TaskInfo(int taskID)
        {
            Tasks t = taskRep.GetTask(taskID);
            return View("Task", t);
        }
    }
}
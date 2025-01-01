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
using Microsoft.AspNetCore.Mvc.Rendering;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Администратор")]
    public class AdminController : Controller
    {

         Random rand1 = new Random();
         Random rand2 = new Random();
        public ITaskRepository taskRep;
		public IGameManager gameManager;
        public IGameRepository gameRep;
        public AdminController(IGameManager manager,
            ITaskRepository tasks,
            IGameRepository games)
        {
            taskRep = tasks;
            gameRep = games;
            gameManager = manager;
        }
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult AddGame()
        {
            return View("GameMake",taskRep.GetTasks());
        }
        public ActionResult AddTask()
        {
            return View("TaskMake", new Tasks());
        }
        public ActionResult TaskList()
        {
            List<Tasks> tasks = (List<Tasks>)taskRep.GetTasks();
            return View("Tasks",tasks);
        }
        public ActionResult Main()
        {
            List<Game> games = gameRep.GetGames().ToList();
            foreach (Game game in games)
                game.StartData = game.StartData.ToLocalTime();
            return View("Main", games);
        }
        [HttpGet]
        public ActionResult TaskInfo(int taskId)
        {
            Tasks task=taskRep.GetTask(taskId);
            if (task is null)
            {
                return NotFound();
            }
            return View("Task", task);
        }
        [HttpGet]
        public ActionResult UpdateTask(int taskId)
        {
            Tasks task = taskRep.GetTask(taskId);
            if (task is null)
            {
                return NotFound();
            }
            return View("UpdateTask", task);
        }
        [HttpGet]
        public ActionResult UpdateGame(int gameId)
        {
            GameViewModel game= new GameViewModel { game = gameRep.GetGame(gameId), tasks = (List<Tasks>)taskRep.GetTasks() };
            if (game is null)
            {
                return NotFound();
            }
            return View("UpdateGame", game);
        }
        [HttpPost]
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
        [HttpPut]
        
        public ActionResult UpdateTaskClick(int id,  string Text,string Answer)
        {
            Tasks task = taskRep.GetTask(id);
            task.Text = Text;
            task.Answer = Answer;
            if (!task.Validation())
            {
                ViewBag.Message = "Некоторые поля не заполнены";
                return View("TaskEdit");
            }
            taskRep.UpdateTask(task);
            taskRep.Save();
            ViewBag.Message2 = "Задача создана";
            return View("UpdateTask",task);
        }
        [HttpDelete]
        public ActionResult DeleteTask(int id)
        {
            Tasks task = taskRep.GetTask(id);
            taskRep.DeleteTask(task);
            taskRep.Save();
            ViewBag.Message = "Задача удалена";
            return View("Tasks",taskRep.GetTasks());
        }
        [HttpPost]
        public ActionResult CreateGameClick(string Name, DateTime StartDate,DateTime StartTime, int Lenga,List<int> tasks)
        {
            Game game = new Game();
            if (tasks.Count == 0)
            {
                ViewBag.Message = "Задачи не выбраны";
                return View("GameMake", taskRep.GetTasks());
            }
            game.TasksAmount = tasks.Count;
            for (int i=0;i<tasks.Count;i++)
                game.Tasks.Add(taskRep.GetTask(tasks[i]));
            game.Name = Name;
            game.StartData = StartDate.AddHours(StartTime.Hour).AddMinutes(StartTime.Minute).AddSeconds(StartTime.Second);
            game.Lenga = Lenga;
            if (!game.Validation())
            {
                ViewBag.Message = "Некоторые поля неверно заполнены";
                return View( "GameMake", taskRep.GetTasks());
            }
            game.StartData = game.StartData.ToUniversalTime();
            gameRep.CreateGame(game);
            gameRep.Save();
            ViewBag.Message2 = "Игра создана";
            gameManager.AddGame(game);
            return View("GameMake",taskRep.GetTasks());
        }
        [HttpPut]
        public ActionResult UpdateGameClick(int gameId, string GameName,int Lenga,DateTime StartDate,DateTime StartTime)
        {
            Game game = gameRep.GetGame(gameId);
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
            gameManager.UpdateGame(StartDate, Lenga, gameId);
            return View("Games");
        }
        [HttpDelete]
        public ActionResult DeleteGameClick(int ID)
        {
            Game game = gameRep.GetGame(ID);
            if (game.Ongoing())
            {
                ViewBag.Message = "Игра идёт";
                return Main();
            }
            gameRep.DeleteGame(game);
            gameRep.Save();
            gameManager.RemoveGame(game.ID); 
            return Ok();
        }
    }
}
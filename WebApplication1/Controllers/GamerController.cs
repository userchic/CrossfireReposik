using WebApplication3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using WebApplication3.DataBase;

namespace WebApplication3.Controllers
{
    [Authorize (Roles ="Игрок")]
    public class GamerController : Controller
    {
        public static GameProcess game { get; set; }
        
        public static GameContext db ;
        static Random rand1 = new Random();
        static Random rand2 = new Random();
        public static int GameLenga { get; set; }
        public static int Seconds { get; set; }
        public static GameTask Task { get; set; }
        public GamerController(GameContext context)
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
        public ActionResult Play(string GameID)
        {
            int ID = int.Parse(GameID);
            Game g = db.Games.FirstOrDefault(game => game.ID == ID);
            game = new GameProcess() { Tasks = new List<GameTask>() };
            GameLenga=g.Lenga;
            Seconds=60;
            var req=db.Tasks.Select(x=>x).Where(x=>x.GameId==ID);
            foreach (var task in req)
                game.Tasks.Add(new GameTask() { Text = task.Text, Answer = task.Answer, Result = "Не решено", ID = task.ID });

            return View("Game", game);
        }
        public ActionResult CancelGame()
        {
            return View("Main");
        }
        [HttpGet]
        public ActionResult Answer(string ID, string seconds, string minutes)
        {
            Task = game.Tasks.FirstOrDefault(task=>task.ID==int.Parse(ID));
            Seconds = int.Parse(seconds);
            GameLenga = int.Parse(minutes);
            return View("Answer");
        }
        public ActionResult EnemySendAnswer()
        {
            if (game.RNG.SolvedTasks < game.Tasks.Count)
            {
                game.RNG.SolveTask(ref rand1);
                if (rand2.NextDouble() > 0.5)
                {
                    game.RNG.Shots++;
                    game.RNG.Hits++;
                    game.Player.GetHit();
                }
                else
                    game.RNG.Misses++;
            }
            return Json(new { PScore = game.RNG.Score, EScore = game.Player.Score });
        }
        [HttpPost]
        public ActionResult SendAnswer(string SentAnswer, string time, string seconds)
        {
            GameLenga = int.Parse(time);
            Seconds = int.Parse(seconds);
            db.UsersAnswers.Add(new Sent_Answers()
            {
                Answer = Task.Answer,
                Task = db.Tasks.First(x => x.ID == Task.ID),
                TaskID = db.Tasks.FirstOrDefault(x => x.ID == Task.ID).ID,
                User = db.Users.FirstOrDefault(x=>x.ID== WebApplication3.Controllers.HomeController.user.ID),
                UserID = WebApplication3.Controllers.HomeController.user.ID,
                SentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, GameLenga / 60, GameLenga, Seconds)
            });
            db.SaveChanges();
            if (Task.Answer ==SentAnswer)
            {
                if (game.Player.SolveTask(ref rand1))
                {
                    game.Player.Hits++;
                    game.RNG.GetHit();
                }
                else
                    game.Player.Misses++;
                game.Tasks.FirstOrDefault(task => task.ID == Task.ID).Result = "Решено верно";
            }
            else
            {
                game.Player.MistakedTasks++;
                game.Tasks.FirstOrDefault(task => task.ID == Task.ID).Result = "Решено неверно";
            }
                return View("Game");
        }
        [HttpGet]
        public ActionResult CancelAnswer(string time,string seconds)
        {
            GameLenga = int.Parse(time);
            Seconds = int.Parse(seconds);
                return View("Game", game);
        }
        public ActionResult ShowResults()
        {
            return View("Results",game);
        }
    }
}
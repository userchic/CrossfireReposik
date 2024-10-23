using WebApplication1.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using System.Threading;
using Microsoft.AspNetCore.Authorization;
using WebApplication1.DataBase;
using Microsoft.IdentityModel.Tokens;
using System.Timers;
using WebApplication1.Abstractions;

namespace WebApplication1.Controllers
{
    [Authorize(Roles = "Игрок")]
    public class GamerController : Controller
    {
        public GameProcess gameProcess { get; set; }

        System.Timers.Timer timer = new System.Timers.Timer();

        static Random rand1 = new Random();
        static Random rand2 = new Random();

        public ITaskRepository taskRep;
        public IGameRepository gameRep;
        public ITeamRepository teamRep;
        public IUserRepository userRep;
        public IUserGameStatusRepository userGameStatusRep;
        public IParticipationsRepository ParticipationsRep;
        public static InGameTask Task { get; set; }
        public GamerController( ITaskRepository tasks, IGameRepository games, ITeamRepository teams, IUserRepository users, IUserGameStatusRepository userGameStatuses, IParticipationsRepository participations)
        {
            taskRep = tasks;
            gameRep = games;
            teamRep = teams;
            userRep = users;
            userGameStatusRep= userGameStatuses;
            ParticipationsRep = participations;
        }
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Game()
        {
            return View("Game");
        }
        public ActionResult CreateTeamClick(int gameId, string teamName)
        {
            Game game = gameRep.GetGame(gameId);
            if (game is null)
            {
                ViewBag.Message = "Игра не существует";
                return View("TeamCreate");
            }
            if (game.Ongoing())
            {
                ViewBag.Message = "Игра началась";
                return View("TeamCreate");
            }
            Users user = GetUser();
            Teams team = Teams.Create(  teamName, new List<Users>() { user } );
            if (!team.Validation())
            {
                ViewBag.Message = "Название команды не введено";
                return View("TeamCreate");
            }
            teamRep.CreateTeam(team);
            teamRep.Save();
            ViewBag.Message = "Команда создана";
            return View("Teams");
        }
        public ActionResult UpdateTeamClick(int teamId, string teamName)
        {
            Teams team=teamRep.GetTeam(teamId);
            if (team is null)
            {
                ViewBag.Message = "Команда не существует";
                return View("Teams");
            }
            Game game = gameRep.GetGame(team.GameID); 
            if (game is null)
            {
                ViewBag.Message = "Игра не существует";
                return View("TeamUpdate");
            }
            if (game.Ongoing())
            {
                ViewBag.Message = "Игра началась";
                return View("TeamUpdate");
            }
            Users user = GetUser();
            if (!team.Validation())
            {
                ViewBag.Message = "Название команды не введено";
                return View("TeamUpdate");
            }
            teamRep.UpdateTeam(team);
            teamRep.Save();
            ViewBag.Message = "Команда изменена";
            return View("Teams");
        }
        public ActionResult JoinTeam(int teamId)
        {
            Teams team = teamRep.GetTeam(teamId);
            if (team is not null)
            {
                ViewBag.Message = "Команда не существует";
                return View("Teams");
            }
            Game game = gameRep.GetGame(team.GameID);
            if (game is null)
            {
                ViewBag.Message = "Игра не существует";
                return View("Teams", game);
            }
            if (game.Started())
            {
                ViewBag.Message = "Игра началась";
                return View("TeamUpdate");
            }
            Users user = GetUser();
            if (team.Users.Contains(user))
            {
                ViewBag.Message = "Пользователь уже в этой команде";
                return View("Teams,");
            }
            if (teamRep.UserInGame(user,game) is not null)
            {
                ViewBag.Message = "Пользователь уже команде";
                return View("Teams,");
            }
            team.Users.Add(user);
            teamRep.UpdateTeam(team);
            teamRep.Save();
            return View("Game", game);
        }
        public ActionResult LeaveTeam(int teamId)
        {
            Teams team = teamRep.GetTeam(teamId);
            if (team is null)
            {
                ViewBag.Message = "Команда не существует";
                return View("Main");
            }
            Users user = GetUser();
            Game game = gameRep.GetGame(team.GameID);
            if (game is null)
            {
                ViewBag.Message = "Игра не существует";
                return View("Main");
            }
            if (!team.Users.Contains(user))
            {
                ViewBag.Message = "Пользователя нет в этой команде";
                return View("Main");
            }
            if (game.Started())
            {
                ViewBag.Message = "Игра началась, нельзя уходить";
                return View("Teams", game);
            }
            team.Users.Remove(user);
            user.Teams.Remove(team);
            teamRep.UpdateTeam(team);
            teamRep.Save();
            return View();
        }



        private Users GetUser() => userRep.GetUser(User.Identity.Name);
        public ActionResult JoinGame(int gameId)
        {
            Game game = gameRep.GetGame(gameId);
            Users user = GetUser();
            if (game is null)
            {
                ViewBag.Message = "Игра не существует";
                return View("Main");
            }
            if (!game.Ongoing())
            {
                ViewBag.Message = "Игра не началась";
                return View("Main");
            }
            if (teamRep.UserInGame(user, game) is null)
            {
                ViewBag.Message = "Пользователь не в команде этой игры";
                return View("Main");
            }
            gameProcess = new GameProcess { Tasks = (List<InGameTask>)game.Tasks, Player = new InGameUser() };
            UserGameStatus status = userGameStatusRep.GetStatus(0);
            ParticipationsRep.CreateParticipation(new UserParticipation { GameID = gameId, User = GetUser(), UserGameStatus = status });
            gameRep.Save();
            return View("Game", game);
        }
        public ActionResult LeaveGame()
        {
            if (gameProcess is null)
            {
                ViewBag.Message = "Вы не участвуете ни в какой игре";
                return View("Main", null);
            }
            UserParticipation participation = ParticipationsRep.GetParticipation(gameProcess.ID , GetUser().Login);
            participation.UserGameStatus = userGameStatusRep.GetStatus(3);
            ParticipationsRep.UpdateParticipation(participation);
            ParticipationsRep.Save();
            gameProcess = null;
            return View("Main", null);
        }
        public ActionResult Play(string GameID)
        {
            //int ID = int.Parse(GameID);
            //Game g = db.Games.FirstOrDefault(game => game.ID == ID);
            //game = new GameProcess() { Tasks = new List<InGameTask>() };
            //GameLenga=g.Lenga;
            //Seconds=60;
            //var req=db.Tasks.Select(x=>x).Where(x=>x.GameId==ID);
            //foreach (var task in req)
            //    game.Tasks.Add(new InGameTask() { Text = task.Text, Answer = task.Answer, Result = "Не решено", ID = task.ID });

            return View("Game", gameProcess);
        }
        public ActionResult CancelGame()
        {
            return View("Main");
        }
        [HttpGet]
        public ActionResult Answer(int ID, string answer)
        {
            if (gameProcess is null)
            {
                ViewBag.Message = "Вы не находитесь в процессе игры";
                return View("Main");
            }
            Sent_Answers ans = new Sent_Answers() { Answer==answer};
            if (!ans.Validation())
            {
                ViewBag.Message = "Ответ не указан";
                return View("Answer", Task);
            }

                Task = gameProcess.Tasks.FirstOrDefault(task => task.ID == ID);
            if (Task is null)
            {
                ViewBag.Message = "задача не существует";
                return View("Answer", Task);
            }
            if (Task.Answer==answer)

            return View("Answer");
        }

        [HttpPost]
        public ActionResult SendAnswer(string SentAnswer, string time, string seconds)
        {
            
            //GameLenga = int.Parse(time);
            //Seconds = int.Parse(seconds);
            //db.UsersAnswers.Add(new Sent_Answers()
            //{
            //    Answer = Task.Answer,
            //    Task = db.Tasks.First(x => x.ID == Task.ID),
            //    TaskID = db.Tasks.FirstOrDefault(x => x.ID == Task.ID).ID,
            //    User = db.Users.FirstOrDefault(x=>x.ID== WebApplication1.Controllers.HomeController.user.ID),
            //    UserID = WebApplication1.Controllers.HomeController.user.ID,
            //    SentTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day, GameLenga / 60, GameLenga, Seconds)
            //});
            //db.SaveChanges();
            //if (Task.Answer ==SentAnswer)
            //{
            //    if (game.Player.SolveTask(ref rand1))
            //    {
            //        game.Player.Hits++;
            //        game.RNG.GetHit();
            //    }
            //    else
            //        game.Player.Misses++;
            //    game.Tasks.FirstOrDefault(task => task.ID == Task.ID).Result = "Решено верно";
            //}
            //else
            //{
            //    game.Player.MistakedTasks++;
            //    game.Tasks.FirstOrDefault(task => task.ID == Task.ID).Result = "Решено неверно";
            //}
            //    return View("Game");
            return View();
        }
        [HttpGet]
        public ActionResult CancelAnswer(string time,string seconds)
        {
            
                return View("Game", gameProcess);
        }
        public ActionResult ShowResults()
        {
            return View("Results", gameProcess);
        }
    }
}
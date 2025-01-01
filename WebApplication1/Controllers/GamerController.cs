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
using WebApplication1.GamerViewModels;
using System.Security.Claims;
using WebApplication1.Reposiotories;
using Microsoft.EntityFrameworkCore.Metadata;

namespace WebApplication1.Controllers
{
	[Authorize(Roles = "Игрок")]
	public class GamerController : Controller
	{


		static Random rand1 = new Random();
		static Random rand2 = new Random();


		public IShotRepository shotRep;
		public ITaskRepository taskRep;
		public IGameRepository gameRep;
		public ITeamRepository teamRep;
		public IUserRepository userRep;
		public IUserGameStatusRepository userGameStatusRep;
		public IParticipationsRepository participationsRep;
		public ISent_AnswersRepository sentAnswersRep;
		public IGameManager gameManager;
		public GamerController(IGameManager manager,
			ITaskRepository tasks,
			IGameRepository games,
			ITeamRepository teams,
			IUserRepository users,
			IUserGameStatusRepository userGameStatuses,
			IParticipationsRepository participations,
			ISent_AnswersRepository sent_Answers,
			IShotRepository shots)
		{
			taskRep = tasks;
			gameRep = games;
			teamRep = teams;
			userRep = users;
			userGameStatusRep = userGameStatuses;
			participationsRep = participations;
			sentAnswersRep = sent_Answers;
			shotRep = shots;
			gameManager = manager;
		}
		public ActionResult Index()
		{
			return View();
		}
		public ActionResult TeamsList(int gameId)
		{
			Game game = gameRep.GetGame(gameId);
			if (game is null) return NotFound();
			List<Teams> teams = (List<Teams>)teamRep.GetTeams(gameId);
			TeamsModel model = new TeamsModel(gameId, teams);
			return View("Teams", model);
		}
		
        public ActionResult Team(int teamId)
		{
			Teams team = teamRep.GetTeamWithUsers(teamId);
			if (team is null)
				return NotFound();
			return View("Team", team);
		} 
	
		public ActionResult UpdateTeam(int teamId)
		{
			Teams team = teamRep.GetTeam(teamId);
			if (team is null)
				return NotFound();
			return View("UpdateTeam",team);
		}
        public ActionResult CancelAnswer(string time, string seconds)
        {
            return View("Game");
        }
        public ActionResult ShowResults()
        {
            return View("Results");
        }
		public ActionResult CreateTeam(int gameId)
		{
			return View("TeamMake", gameId);
		}
		public ActionResult Main()
		{
			List<Game> games= gameRep.GetGames().ToList();
			foreach (Game game in games)  
				game.StartData = game.StartData.ToLocalTime();
			return View("Main",games);
		}
		[HttpPost]
        public ActionResult CreateTeamClick(int gameId, string teamName)
		{
			Game game = gameRep.GetGame(gameId);

			if (game.Ongoing())
			{
				ViewBag.Message = "Игра началась";
				return View("TeamCreate",gameId);
			}
			Users user = GetUser();
			Teams team = Teams.Create(  teamName,gameId, new List<Users>() { user } );
			if (!team.Validation())
			{
				ViewBag.Message = "Название команды не введено";
				return View("TeamCreate", gameId);
			}
            if (teamRep.UserInGame(user, game) is not null)
            {
                ViewBag.Message = "Пользователь уже в другой команде";
                return View("TeamMake",gameId);
            }
            teamRep.CreateTeam(team);
			teamRep.Save();
			ViewBag.Message = "Команда создана";
			return TeamsList(gameId);
		}
		//[HttpPut]
		//public ActionResult UpdateTeamClick(int teamId, string teamName)
		//{
		//	Teams team=teamRep.GetTeam(teamId);
		//	Game game = gameRep.GetGame(team.GameID); 
		//	if (game is null)
		//	{
		//		ViewBag.Message = "Игра не существует";
		//		return View("TeamUpdate");
		//	}
		//	if (game.Ongoing())
		//	{
		//		ViewBag.Message = "Игра началась";
		//		return View("TeamUpdate");
		//	}
		//	Users user = GetUser();
		//	if (!team.Validation())
		//	{
		//		ViewBag.Message = "Название команды не введено";
		//		return View("TeamUpdate");
		//	}
		//	teamRep.UpdateTeam(team);
		//	teamRep.Save();
		//	ViewBag.Message = "Команда изменена";
		//	return View("Teams");
		//}
		[HttpPost]
		public ActionResult JoinTeam(int teamId)
		{
            /*
			Находим команду чтобы проверить что она существует, подгружаем членов этой команды
			Получаем игру
			Проверяем что игра ещё не начиналась
			Получаем пользователя
			Проверяем что пользователь в рамках этой игры ещё не присоединился ни к какой команде
			Добавляем к членам команды пользователя
			обновляем команду в БД
			  */
            Teams team = teamRep.GetTeamWithUsers(teamId);
			if (team is null)
			{
				ViewBag.Message = "Команда не существует";
				return Main();
			}
			Game game = gameRep.GetGame(team.GameID);
            if (game is null)
            {
                ViewBag.Message = "Игра не существует";
                return Main();
            }
            if (game.Started())
			{
				ViewBag.Message = "Игра началась";
				return Main();
			}
			Users user = GetUser();
            if (teamRep.UserInGame(user, game) is not null)
            {
                ViewBag.Message = "Пользователь уже в другой команде";
                return Team(teamId);
            }
            if (team.Users.Contains(user))
			{
				ViewBag.Message = "Пользователь уже в этой команде";
				return Team(teamId);
			}
			team.Users.Add(user);
			team.UsersAmount++;
			teamRep.UpdateTeam(team);
			teamRep.Save();
			return Team(teamId);
		}
		[HttpPost]
		public ActionResult LeaveTeam(int teamId)
		{
            /*
			Находим команду чтобы проверить что она существует, подгружаем членов этой команды
			Получаем игру
			Проверяем что игра ещё не начиналась
			Получаем пользователя
			Проверяем что пользователь находится в этой команде
			Удаляем из членов команды пользователя
			обновляем команду в БД
			  */
            Teams team = teamRep.GetTeamWithUsers(teamId);
			if(team is null)
			{
                ViewBag.Message = "Команда не существует";
                return Main();
            }
			Game game = gameRep.GetGame(team.GameID);
			if (game is null)
			{
				ViewBag.Message = "Игра не существует";
				return Main();
			}
			Users user = GetUser();
			if (!team.Users.Contains(user))
            {
				ViewBag.Message = "Пользователя нет в этой команде";
				return Team(teamId);
			}
			if (game.Started())
			{
				ViewBag.Message = "Игра началась, нельзя уходить";
				return Main();//переход к текущей игре TO DO
			}
			team.Users.Remove(user);
			team.UsersAmount--;
			if (team.UsersAmount == 0)
			{
				teamRep.DeleteTeam(team);
				teamRep.Save();
				return Main();
            }
			teamRep.UpdateTeam(team);
			teamRep.Save();
			return Team(teamId);
		}
		[HttpPost]
		public ActionResult JoinGame(int gameId)
		{
			Game game = gameRep.GetGame(gameId);
			if (game is null)
			{
				ViewBag.Message = "Игра не существует";
				return Main();
			}
			Users user = GetUser();
			if (!game.Ongoing())
			{
				if (!game.Started())
				ViewBag.Message = "Игра не началась";
				if (game.Ended())
				ViewBag.Message = "Игра закончилась";
				return Main();
            }
			UserParticipation participation =participationsRep.GetParticipation(gameId, user.Login);
			if(participation is not null)
			{
				if(participation.UserGameStatus.ID!=3)
                {
                    participation.UserGameStatus = userGameStatusRep.GetStatus(1);
                    participationsRep.UpdateParticipation(participation);
                    participationsRep.Save();
                }
				return GameView(game, user);
            }
			if (teamRep.UserInGame(user, game) is null)
			{
				ViewBag.Message = "Пользователь не в команде этой игры";
				return Main();
			}
			UserGameStatus status = userGameStatusRep.GetStatus(1);
			participationsRep.CreateParticipation(new UserParticipation { GameID = gameId, User = GetUser(), UserGameStatus = status });
            participationsRep.Save();

			return GameView(game, user);
		}

		private ActionResult GameView(Game game,Users user)
		{
			
			List<Tasks> tasks = game.Tasks.Select(x => x).ToList();
			List<InGameTask> inGameTasks = new List<InGameTask>();
			Teams team = teamRep.UserInGame(user, game);
			for (int i = 0; i < tasks.Count; i++)
			{
				InGameTask task = new InGameTask() { Text = tasks[i].Text, ID = tasks[i].ID };
				Sent_Answers answer = tasks[i].UsersAnswers.FirstOrDefault(x => x.TeamID == team.ID);
				if (answer is not null)
				{
					task.SentAnswer = true;
					if (answer.Answer == tasks[i].Answer)
					{
						task.Result = true;
						if (answer.Shot.isSuccessful)
							task.ShotResult = true;
					}
				}
				inGameTasks.Add(task);
			}
			DateTime gameEnd = game.StartData.ToLocalTime().AddMinutes(game.Lenga);
			GameModel model = new GameModel() {teamID=team.ID, tasks = inGameTasks, teams = (List<Teams>)game.Teams, time = TimeOnly.FromTimeSpan(gameEnd - DateTime.Now) };
			return View("Game", model);
		}

		[HttpPost]
		public ActionResult LeaveGame()
		{
            Users user = GetUser();
            UserParticipation participation = participationsRep.GetCurrentParticipation(user.Login);
            if (participation is null || participation.UserGameStatus.ID==2)
            {
                ViewBag.Message = "Игрок не участвует сейчас ни в какой игре";
                return Main();
            }
            Game game = gameRep.GetGame(participation.GameID);
            if (game is null)
            {
                ViewBag.Message = "Вы не участвуете ни в какой игре в данный момент";
                return Main();
            }
			participation.UserGameStatus = userGameStatusRep.GetStatus(2);
			participationsRep.UpdateParticipation(participation);
			participationsRep.Save();
			return Main();
		}

		[HttpPost]
		public ActionResult AnswerClick(int taskId, string answer,int teamId,int targetTeamId)
		{
			Users user=GetUser();
			UserParticipation participation=participationsRep.GetCurrentParticipation(user.Login);
			if (participation is null)
				return Json(new {isNotParticipating = true });
			Tasks task = taskRep.GetTask(taskId);
			if (task is null)
				return NotFound();
			Teams team = teamRep.GetTeam(teamId);
			if (team is null)
				return NotFound();
			Game game = gameRep.GetGame(participation.GameID);
			if (game is null)
				return NotFound();
			if (!game.Ongoing())
				return GameResults();
			if (teamRep.GetTeams(game.ID).Where(x => x.ID == teamId).Count() == 0)
				return Json(new { UnknownTeam = true });
			if (!game.Tasks.Contains(task))
                return Json(new { UnknownTask = true });
			if (sentAnswersRep.GetAnswerByTeamAndTask(taskId, teamId) is not null)
				return Json(new { AnswerAllReadySent = true });
			Sent_Answers sentAnswer = new Sent_Answers() {TaskID=taskId ,Answer=answer, UserLogin = user.Login,TeamID=teamId };
			Shots shot=null;

            if (task.Answer==answer)
            {
				team.ShotsAmount++;
                shot = new Shots() {TargetTeamID=targetTeamId };
				if (rand1.Next() > 0.5)
				{
					shot.isSuccessful = true;
					team.Hits++;
                    Teams targetTeam = teamRep.GetTeam(targetTeamId);
					targetTeam.Score--;
					teamRep.UpdateTeam(targetTeam);
				}
				else
					team.Misses++;
                sentAnswer.Correctness = true;
				sentAnswer.SentTime = DateTime.Now.ToUniversalTime();
				sentAnswersRep.CreateAnswer(sentAnswer);
				sentAnswersRep.Save();
				shot.AnswerID = sentAnswer.ID;
				team.SolvedTasks++;
				team.Score++;
                shotRep.CreateShot(shot);
                shotRep.Save();
            }
            else
			{
                sentAnswersRep.CreateAnswer(sentAnswer);
                team.MistakedTasks++;
            }
			teamRep.UpdateTeam(team);
			teamRep.Save();
			gameManager.RecieveAnswer(game.ID, sentAnswer, shot);
            return Json(new { });
		}

        private ActionResult GameResults()
        {
			Users user = GetUser();
			UserParticipation participation = participationsRep.GetCurrentParticipation(user.Login);
			if (participation is null)
				return Main();
			Game game=gameRep.GetGame(participation.GameID);
			if (game is null)
				return Main();
			ResultsModel model= new ResultsModel();
			model.game = game;
			Teams team=teamRep.UserInGame(user, game);
			model.UserTeamID = team.ID;
			return View("Results", model);
        }

        //[HttpPost]
        //public ActionResult ShootClick(int targetId,int answerId)
        //{
        //	Sent_Answers answer= sentAnswersRep.GetAnswer(answerId);
        //	if (answer is null)
        //	{
        //		return NotFound();
        //	}
        //	Tasks task = sentAnswersRep.GetTask(answer);
        //	if (task.Answer!=answer.Answer)
        //	{
        //		ViewBag.Message = "Ответ был неправильный";
        //		return View("Game");
        //	}
        //	Shots shot=shotRep.GetShot(answerId);
        //	if (shot is not null)
        //	{
        //		ViewBag.Message = "Выстрел уже произведён";
        //		return View("Game");
        //	}
        //	Teams team = teamRep.GetTeam(targetId);
        //	if (team is null)
        //	{
        //		ViewBag.Message = "Целевая команда не существует";
        //		return View("Game");
        //	}
        //	double pick = rand1.NextDouble();
        //	Teams targetTeam= teamRep.GetTeam(targetId);
        //	team.ShotsAmount++;
        //	if (pick > 0.5)
        //	{
        //		targetTeam.Score--;
        //		team.Hits++;
        //	}
        //	teamRep.UpdateTeam(team);
        //	teamRep.UpdateTeam(targetTeam);
        //	teamRep.Save();
        //	return View("Game");
        //}

        private Users GetUser() => userRep.GetUser(User.Identity.Name);
		public static bool TeamIncludesUser(ClaimsPrincipal User, Teams team) => team.Users.FirstOrDefault(x=>x.Login==User.Identity.Name) is not null;
    }
}
using GameHubSpace;
using Microsoft.EntityFrameworkCore.Query.Internal;
using WebApplication1.Abstractions;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class GameManager:IGameManager
    {
        public List<GameProcess> games;
        public IHubContext<GameHub,IGameClient> Hub;
        public GameManager(IHubContext<GameHub, IGameClient> gameHub)
        {
            games = new List<GameProcess>();
            Hub= gameHub;
        }
        public void AddGame(Game game)
        {
            List<InGameTask> InGameTasks = new List<InGameTask>();
            List<Tasks> tasks =game.Tasks.ToList();
            for(int i = 0;i<game.TasksAmount;i++)
                InGameTasks.Add(tasks[i].ToInGameTask());
            GameProcess process = new GameProcess(InGameTasks,game.Teams.ToList(),game.StartData, game.ID, game.Lenga)
            {
                ID = game.ID,
                manager = this
            };
        }
        public void RecieveAnswer(int gameId,Sent_Answers answer,Shots? shot)
        {
            games.Find(x => x.ID == gameId).AddAnswer(answer);
            bool shotCorrectness=false;
            if (shot != null) 
                shotCorrectness = shot.isSuccessful;
            Hub.Clients.Group(gameId.ToString()).SolvedTaskMessage(answer.TaskID, answer.Correctness, shotCorrectness);
        }
        public void UpdateGame(DateTime StartData,int Lenga,int gameId)
        {
            GameProcess game = games.First(x => x.ID == gameId);
            game.SetStartTimer(StartData, Lenga);
        }
        public void RemoveEndedGame(object gameId)
        {
            int GameId = (int)gameId;
            games.Remove(games.Find(x=>x.ID==GameId));
            Hub.Clients.Group(GameId.ToString()).EndGameMessage();
        }
        public void RemoveGame (int gameId)
        {
            GameProcess proc = games.Find(x => x.ID == gameId);
            proc.StopTimers();
            games.Remove(proc);
            Hub.Clients.Group(gameId.ToString()).EndGameMessage();
        }
    }
}

using GameHubSpace;
using Microsoft.EntityFrameworkCore.Query.Internal;
using WebApplication1.Abstractions;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class GameManager:IGameManager
    {
        public List<GameProcess> games;
        public GameHub Hub;
        public GameManager(GameHub gameHub)
        {
            games = new List<GameProcess>();
            Hub= gameHub;
        }
        public void AddGame(Game game)
        {
            List<InGameTask> InGameTasks = new List<InGameTask>();
            List<Tasks> tasks =new List<Tasks>();
            for(int i = 0;i<game.TasksAmount;i++)
                InGameTasks.Add(tasks[i].ToInGameTask());
            GameProcess process = new GameProcess(InGameTasks,game.Teams.ToList(),game.StartData, game.ID, game.Lenga)
            {
                ID = game.ID,
                manager = this
            };
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
            Hub.EndGame(GameId);
        }
    }
}

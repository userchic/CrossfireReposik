using WebApplication1.Models;

namespace WebApplication1.Abstractions
{
    public interface IGameManager
    {
        public void AddGame(Game game);
        public void RemoveEndedGame(object gameId);
        public void UpdateGame(DateTime StartData, int Lenga, int gameId);
    }
}

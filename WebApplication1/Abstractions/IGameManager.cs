using WebApplication1.Models;

namespace WebApplication1.Abstractions
{
    public interface IGameManager
    {
        public void AddGame(Game game);
        public void RemoveEndedGame(object gameId);
        public void RemoveGame(int gameId);
        public void UpdateGame(DateTime StartData, int Lenga, int gameId);
        public void RecieveAnswer(int gameId, Sent_Answers answer, Shots? shot);
    }
}

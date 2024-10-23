using WebApplication1.Abstractions;
using WebApplication1.DataBase;
using WebApplication1.Models;

namespace WebApplication1.Reposiotories
{
    public class GameRepository :IGameRepository
    {
        public GameContext _context;
        public GameRepository(GameContext context)
        {
            _context=context;
        }
        public Game GetGame(int id)
        {
            return _context.Games.FirstOrDefault(x=>x.ID==id);
        }
        public ICollection<Game> GetGames()
        {
            return _context.Games.Select(x=>x).ToList();
        }
        public void CreateGame(Game game)
        {
            _context.Games.Add(game);
        }
        public void UpdateGame(Game game)
        {
            _context.Games.Update(game);
        }
        public void DeleteGame(Game game)
        {
            _context.Games.Remove(game);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

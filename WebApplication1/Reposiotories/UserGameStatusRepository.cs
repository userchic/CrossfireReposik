using WebApplication1.Abstractions;
using WebApplication1.DataBase;
using WebApplication1.Models;

namespace WebApplication1.Reposiotories
{
    public class UserGameStatusRepository : IUserGameStatusRepository
    {
        public GameContext _context;
        public UserGameStatusRepository(GameContext context)
        {
            _context = context;
        }
        public UserGameStatus GetStatus(int id)
        {
            return _context.UserGameStatuses.Find(id);
        }
    }
}

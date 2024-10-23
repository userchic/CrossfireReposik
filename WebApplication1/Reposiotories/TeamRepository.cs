using WebApplication1.Abstractions;
using WebApplication1.DataBase;
using WebApplication1.Models;

namespace WebApplication1.Reposiotories
{
    public class TeamRepository:ITeamRepository
    {
        public GameContext _context;
        public TeamRepository(GameContext context)
        {
            _context = context;
        }
        public Teams GetTeam(int id)
        {
            return _context.Teams.Find(id);
        }
        public ICollection<Teams> GetTeams()
        {
            return _context.Teams.Select(x => x).ToList();
        }
        public Teams UserInGame(Users user,Game game)
        {
            return _context.Teams.FirstOrDefault(x => x.GameID == game.ID && x.Users.Contains(user));
        }
        public void CreateTeam(Teams Team)
        {
            _context.Teams.Add(Team);
        }
        public void UpdateTeam(Teams Team)
        {
            _context.Teams.Update(Team);
        }
        public void DeleteTeam(Teams Team)
        {
            _context.Teams.Remove(Team);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

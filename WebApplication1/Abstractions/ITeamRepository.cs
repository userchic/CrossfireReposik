using WebApplication1.Models;

namespace WebApplication1.Abstractions
{
    public interface ITeamRepository
    {
        Teams GetTeam(int id);
        ICollection<Teams> GetTeams();
        void CreateTeam(Teams teams);
        void UpdateTeam(Teams teams);
        void DeleteTeam(Teams teams);
        void Save();
        Teams UserInGame(Users user, Game game);
    }
}

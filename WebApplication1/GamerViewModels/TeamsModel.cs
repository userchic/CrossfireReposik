using WebApplication1.Models;

namespace WebApplication1.GamerViewModels
{
    public class TeamsModel
    {
        public TeamsModel(int GameID, List<Teams> Teams)
        {
            this.gameID = GameID;
            this.teams = Teams;
        }

        public int gameID { get; set; }
        public List<Teams> teams { get; set; } 
    }
}


using Microsoft.IdentityModel.Tokens;

namespace WebApplication1.Models
{
    public class Teams
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public string Name { get; set; } = "";
        public int Score { get; set; } = 0;
        public int UsersAmount { get; set; } = 0;
        public Game Game { get; set; }
        public int SolvedTasks { get; set; } = 0;
        public int MistakedTasks { get; set; } = 0;
        public int ShotsAmount { get; set; } = 0;
        public int Hits { get; set; } = 0;
        public int Misses { get; set; } = 0;
        public ICollection<Users> Users { get; set; }
        public ICollection<Shots> Shots { get; set; }

        public static Teams Create(string teamName,int gameID, List<Users> users)
        {
            return new Teams() { GameID=gameID,Name=teamName,UsersAmount=1,Users=users};
        }
        public bool Validation()
        {
            return !Name.Trim().IsNullOrEmpty(); 
        }
    }
}
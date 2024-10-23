
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
        public ICollection<Users> Users { get; set; }
        public ICollection<Shots> Shots { get; set; }

        public static Teams Create(string teamName, List<Users> users)
        {
            return new Teams() { Name=teamName,UsersAmount=1,Users=users};
        }
        public bool Validation()
        {
            return !Name.Trim().IsNullOrEmpty(); 
        }
    }
}
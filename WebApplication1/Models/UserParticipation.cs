using System.ComponentModel.DataAnnotations.Schema;

namespace WebApplication1.Models
{
    public class UserParticipation
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public UserGameStatus UserGameStatus { get; set; }
        public Game Game { get; set; }
        public Users User { get; set; }
    }
}
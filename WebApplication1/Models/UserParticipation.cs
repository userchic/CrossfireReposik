namespace WebApplication3.Models
{
    public class UserParticipation
    {
        public int UserID { get; set; }
        public int GameID { get; set; }
        public string? StatusID { get; set; }
        public UserGameStatus Status { get; set; }
        public Game Game { get; set; }
        public Users User { get; set; }
    }
}
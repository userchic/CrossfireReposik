namespace WebApplication3.Models
{
    public class UserGameStatus
    {
        public int ID   { get; set; }
        public string Name { get; set; } = "";
        public UserParticipation UserParticipation { get; set; }
    }
}
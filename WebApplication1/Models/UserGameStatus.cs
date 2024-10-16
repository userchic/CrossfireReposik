namespace WebApplication1.Models
{
    public class UserGameStatus
    {
        public int ID   { get; set; }
        public string Name { get; set; } = "";
        public ICollection<UserParticipation> UserParticipation { get; set; }
    }
}
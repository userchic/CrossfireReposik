namespace WebApplication3.Models
{
    public class UserTeam
    {
        public int TeamID { get; set; }
        public int UserID { get; set; }
        public Teams Team { get; set; }
        public Users User { get; set; }
    }
}
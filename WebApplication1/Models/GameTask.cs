namespace WebApplication3.Models
{
    public class GameTask
    {
        public int GameID { get; set; }
        public int TaskID { get; set; }
        public Tasks Task { get; set; }
        public Game game { get; set; }

    }
}
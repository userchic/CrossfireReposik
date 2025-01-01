using WebApplication1.Models;

namespace WebApplication1.GamerViewModels
{
    public class GameModel
    {
        public int ID { get; set; }
        public int teamID { get; set; }
        public TimeOnly time;
        public List<InGameTask> tasks;
        public List<Teams> teams;
    }
}

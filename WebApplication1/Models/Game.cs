using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Game
    {
        public string GameName { get; set; }
        public DateTime StartData { get; set; }
        public DateTime StartTime { get; set; }
        public int Lenga { get; set; }
        public int ID { get; set; }
        public ICollection<UserParticipation> UserParticipation { get; set; }
        public ICollection<GameTask> GameTasks { get; set; }
        public ICollection<Teams> Teams { get; set; }
        public Game()
        {
            UserParticipation= new List<UserParticipation>();
            GameTasks = new List<GameTask>();
            Teams = new List<Teams>();
        }
    }
}
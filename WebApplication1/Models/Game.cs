using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class Game
    {
        public string GameName { get; set; }
        public DateTime StartData { get; set; }
        public int Lenga { get; set; }
        public int ID { get; set; }
        public ICollection<UserParticipation> UserParticipation { get; set; }
        public ICollection<Tasks> Tasks { get; set; }
        public ICollection<Teams> Teams { get; set; }
        public Game()
        {
            UserParticipation= new List<UserParticipation>();
            Tasks = new List<Tasks>();
            Teams = new List<Teams>();
        }
    }
}
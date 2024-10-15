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
        public ICollection<Tasks> Tasks { get; set; }
        public Game()
        {
            Tasks = new List<Tasks>();
        }
    }
}
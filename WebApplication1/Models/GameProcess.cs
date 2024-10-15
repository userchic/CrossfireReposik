using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication3.Models;

namespace WebApplication3.Models
{
    public class GameProcess
    {
        public int ID { get; set; }
        public List<GameTask> Tasks;
        public InGameUser Player = new InGameUser();
        public InGameUser RNG = new InGameUser();
    }
}
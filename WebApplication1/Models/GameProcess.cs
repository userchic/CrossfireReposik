using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Models
{
    public class GameProcess
    {
        public int ID { get; set; }
        public List<InGameTask> Tasks;
        public InGameUser Player = new InGameUser();
    }
}
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;

namespace WebApplication1.Models
{
    public class Game
    {
        public string Name { get; set; }
        public DateTime StartData { get; set; }
        public int Lenga { get; set; }
        public int ID { get; set; }
        public int TasksAmount { get; set; } = 0;
        public ICollection<UserParticipation> UserParticipation { get; set; }
        public ICollection<Tasks> Tasks { get; set; }
        public ICollection<Teams> Teams { get; set; }
        public Game()
        {
            UserParticipation= new List<UserParticipation>();
            Tasks = new List<Tasks>();
            Teams = new List<Teams>();
        }
        public bool Validation()
        {
            int num;
            DateTime t;
            return !Name.Trim().IsNullOrEmpty() 
                && Lenga > 0
                && StartData > DateTime.Now;
        }
        public bool Started()
        {
            return StartData <= DateTime.Now? true : false;
        }
        public bool Ended()
        {
            return StartData.AddMinutes(Lenga) >= DateTime.Now ? true : false;
        }
        public bool Ongoing()
        {
            return Started() & !Ended() ? true : false;
        }
    }
}
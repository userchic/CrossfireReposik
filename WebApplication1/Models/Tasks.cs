using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Tasks
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
        public ICollection<GameTask> GameTask { get; set; }
        public ICollection<Sent_Answers> UsersAnswers { get; set; }
        public Tasks()
        {
            GameTask= new List<GameTask>();
            UsersAnswers = new List<Sent_Answers>();
        }
    }
}
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Configurations;

namespace WebApplication1.Models
{
    public class Tasks
    {
        public int ID { get; set; }
        public string Text { get; set; }
        public string Answer { get; set; }
        public ICollection<Game> Game { get; set; }
        public ICollection<Sent_Answers> UsersAnswers { get; set; }
        public Tasks()
        {
            Game= new List<Game>();
            UsersAnswers = new List<Sent_Answers>();
        }
        public bool Validation()
        {
            return !Answer.Trim().IsNullOrEmpty()&!Text.Trim().IsNullOrEmpty();
        }
    }
}
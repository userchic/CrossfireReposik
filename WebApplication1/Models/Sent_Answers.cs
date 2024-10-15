using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Sent_Answers
    {
        public int ID { get; set; }
        public string Answer { get; set; } = "";
        public int? TaskID { get; set; }
        public int? UserID { get; set; }
        public int? TeamID { get; set; }
        public DateTime SentTime { get; set; }
        public Tasks Task { get; set; }
        public Users User { get; set; }
        public Shots Shot { get; set; }
        public Teams Team { get; set; }
    }
}
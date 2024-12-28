using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class InGameTask
    {
        public string Text { get; set; }
        public bool SentAnswer { get; set; }
        public bool Solved { get; set; }
        public bool Result { get; set; }
        public bool ShotResult { get; set; }
        public int ID { get; set; }
    }
}
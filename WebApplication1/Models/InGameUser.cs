using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class InGameUser
    {
        public int Score { get; set; }
        public int SolvedTasks { get; set; }
        public int MistakedTasks { get; set; }
        public int Shots { get; set; }
        public int Hits { get; set; }
        public int Misses { get; set; }
        public void GetHit() { Score--; }
        public bool SolveTask(ref Random rand)
        {
            SolvedTasks++;
            Score++;
            Shots++;
            if (rand.NextDouble() > 0.5) { Hits++; return true; }
            else { Misses++; return false; }
        }
    }
}
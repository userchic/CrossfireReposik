﻿namespace WebApplication3.Models
{
    public class Teams
    {
        public int ID { get; set; }
        public int GameID { get; set; }
        public string Name { get; set; } = "";
        public int Score { get; set; } = 0;
        public Game Game { get; set; }
    }
}
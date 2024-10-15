using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using WebApplication3.Models;

namespace WebApplication3.DataBase
{
    public class GameContext:DbContext
    {
        public DbSet<Users> Users { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Sent_Answers> UsersAnswers { get; set; }
        public DbSet<Game> Games { get; set; }
        //protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
        //    modelBuilder.Entity<Users>().HasData(
        //        new Users { Name = "Rus", Surname = "Gab", Fatname = "Mar", Class = 1, ID = 1, RegDate = DateTime.Now, Login = "GabRus2", Password = "1234", Role = "Администратор" },
        //        new Users { Name = "Rus", Surname = "Gab", Fatname = "Mar", Class = 1, ID = 1, RegDate = DateTime.Now, Login = "GabRus", Password = "1234", Role = "Игрок" }
        //    );
        //}
        public GameContext(DbContextOptions<GameContext> options)
        : base(options)
        {
            Database.EnsureCreated();   // создаем базу данных при первом обращении
        }
    }
}

﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Web;
using Microsoft.AspNetCore.Mvc;
using WebApplication1.Models;

namespace WebApplication1.DataBase
{
    public class GameContext:DbContext
    {
        public GameContext(DbContextOptions<GameContext> options)
: base(options)
        {
            Database.EnsureDeleted();
            Database.EnsureCreated();   // создаем базу данных при первом обращении
            Role role1 = new Role() { Name = "Администратор" };
            Role role2 = new Role() { Name = "Игрок" };
            UserGameStatus UserGameStatus1 = new UserGameStatus() { Name = "Идёт" };
            UserGameStatus UserGameStatus2 = new UserGameStatus() { Name = "Прервана" };
            UserGameStatus UserGameStatus3 = new UserGameStatus() { Name = "Завершилась" };
            Class class1 = new Class() { Name = "1" };
            Class class2 = new Class() { Name = "2" };
            Class class3 = new Class() { Name = "3" };
            Class class4 = new Class() { Name = "4" };
            Class class5 = new Class() { Name = "5" };
            Class class6 = new Class() { Name = "6" };
            Class class7 = new Class() { Name = "7" };
            Class class8 = new Class() { Name = "8" };
            Class class9 = new Class() { Name = "9" };
            Class class10 = new Class() { Name = "10" };
            Class class11 = new Class() { Name = "11" };
            Class classn = new Class() { Name = "not class" };
            Roles.AddRange(role1, role2);
            UserGameStatuses.AddRange(UserGameStatus1, UserGameStatus2, UserGameStatus3);
            Classes.AddRange(class1, class2, class3, class4, class5, class6, class7, class8, class9, class10, class11, classn);
            SaveChanges();
        }
        public DbSet<Users> Users { get; set; }
        public DbSet<Class> Classes { get; set; }
        public DbSet<Tasks> Tasks { get; set; }
        public DbSet<Sent_Answers> UsersAnswers { get; set; }
        public DbSet<Game> Games { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Shots> Shots { get; set; }
        public DbSet<UserGameStatus> UserGameStatuses { get; set; }
        public DbSet<Teams> Teams { get; set; }
        public DbSet<UserParticipation> Participations { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Users>().HasKey("Login");
            modelBuilder.Entity<Shots>().HasOne(x => x.TargetTeam).WithMany(x => x.Shots);
            
        }
    }
}

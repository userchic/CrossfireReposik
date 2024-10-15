using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication3.Models
{
    public class Users
    {
        public string Login { get; set; }
        public string Password { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Fatname { get; set; }
        public DateTime RegDate { get; set; }
        public int RoleID { get; set; }
        public Role Role { get; set; } 
        public int Class { get; set; }
        public ICollection<UserParticipation> UserParticipation { get; set; }
        public ICollection<UserTeam> UserTeams { get; set; }
        public int ID { get; set; }
        public Users()
        {
            UserParticipation=new List<UserParticipation>();
            UserTeams=new List<UserTeam>();
        }
    }
}
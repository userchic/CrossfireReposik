using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Controllers;

namespace WebApplication1.Models
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
        public int ClassID { get; set; }
        public Class Class { get; set; }
        public ICollection<UserParticipation> UserParticipation { get; set; }
        public ICollection<Teams> Teams { get; set; }
        public Users()
        {
            UserParticipation=new List<UserParticipation>();
            Teams=new List<Teams>();
        }

        public static Users Create(string login, string password, string name, string surName, string fatName, string Role,string Class)
        {
            int classid = HomeController.db.Classes.FirstOrDefault(x => x.Name == Class.ToString()).ID;
            int roleid = HomeController.db.Roles.FirstOrDefault(x => x.Name == Role.ToString()).ID;
            return new Users { Login = login, Password = password, Name = name, Surname = surName, Fatname = fatName, RoleID = roleid, ClassID = classid, RegDate = DateTime.UtcNow };
        }

    }
}
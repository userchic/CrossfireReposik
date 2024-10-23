﻿using WebApplication1.Abstractions;
using WebApplication1.DataBase;
using WebApplication1.Models;

namespace WebApplication1.Reposiotories
{
    public class UserRepository : IUserRepository
    {
        public GameContext _context;
        public UserRepository(GameContext context)
        {
            _context = context;
        }
        public Users GetUser(string login)
        {
            return _context.Users.Find(login);
        }
        public ICollection<Users> GetUsers()
        {
            return _context.Users.Select(x => x).ToList();
        }
        public void CreateUser(Users User)
        {
            _context.Users.Add(User);
        }
        public void UpdateUser(Users User)
        {
            _context.Users.Update(User);
        }
        public void DeleteUser(Users User)
        {
            _context.Users.Remove(User);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

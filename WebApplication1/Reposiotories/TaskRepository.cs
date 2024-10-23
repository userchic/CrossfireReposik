﻿using System.Threading.Tasks;
using WebApplication1.Abstractions;
using WebApplication1.DataBase;
using WebApplication1.Models;

namespace WebApplication1.Reposiotories
{
    public class TaskRepository :ITaskRepository
    {
        public GameContext _context;
        public TaskRepository(GameContext context)
        {
            _context = context;
        }
        public Tasks GetTask(int id)
        {
            return _context.Tasks.FirstOrDefault(x => x.ID == id);
        }
        public ICollection<Tasks> GetTasks()
        {
            return _context.Tasks.Select(x => x).ToList();
        }
        public void CreateTask(Tasks task)
        {
            _context.Tasks.Add(task);
        }
        public void UpdateTask(Tasks task)
        {
            _context.Tasks.Update(task);
        }
        public void DeleteTask(Tasks task)
        {
            _context.Tasks.Remove(task);
        }

        public void Save()
        {
            _context.SaveChanges();
        }
    }
}

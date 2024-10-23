using WebApplication1.Models;

namespace WebApplication1.Abstractions
{
    public interface ITaskRepository
    {
        Tasks GetTask(int id);
        ICollection<Tasks> GetTasks();
        void CreateTask(Tasks task);
        void UpdateTask(Tasks task);
        void DeleteTask(Tasks task);
        void Save();
    }
}
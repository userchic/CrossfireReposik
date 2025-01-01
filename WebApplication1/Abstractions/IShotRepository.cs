using WebApplication1.Models;

namespace WebApplication1.Abstractions
{
    public interface IShotRepository
    {
        Shots GetShot(int answerId);
        void CreateShot(Shots shot);
        void Save();
    }
}
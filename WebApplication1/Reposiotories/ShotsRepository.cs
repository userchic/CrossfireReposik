using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstractions;
using WebApplication1.DataBase;
using WebApplication1.Models;

namespace WebApplication1.Reposiotories
{
    public class ShotsRepository: IShotRepository
    {
        public GameContext _context;
        public ShotsRepository(GameContext context)
        {
            _context = context;
        }

        public void CreateShot(Shots shot)
        {
            _context.Shots.Add(shot);
        }
        public Shots GetShot(int answerId)
        {
            return _context.Shots.Where(x => x.AnswerID == answerId ).Include(x=>x.Sent_answer).First();
        }
    }
}

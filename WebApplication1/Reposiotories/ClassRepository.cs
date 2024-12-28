using WebApplication1.DataBase;
using WebApplication1.Models;

namespace WebApplication1.Reposiotories
{
    public class ClassRepository
    {
        public GameContext _context;
        public ClassRepository(GameContext context)
        {
            _context = context;
        }
        public Class GetClass(string className)
        {
            return _context.Classes.FirstOrDefault(x => x.Name == className);
        }
    }
}

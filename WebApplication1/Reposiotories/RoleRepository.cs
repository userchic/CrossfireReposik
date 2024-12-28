using WebApplication1.DataBase;
using WebApplication1.Models;

namespace WebApplication1.Reposiotories
{
    public class RoleRepository
    {
        public GameContext _context;
        public RoleRepository(GameContext context)
        {
            _context = context;
        }
        public Role GetRole(string roleName)
        {
            return _context.Roles.FirstOrDefault(x=>x.Name == roleName);
        }
    }
}

using WebApplication1.Models;

namespace WebApplication1.Abstractions
{
    public interface IUserRepository
    {
        Users GetUser(string id);
        ICollection<Users> GetUsers();
        void CreateUser(Users user);
        void UpdateUser(Users user);
        void DeleteUser(Users user);
        void Save();
    }
}

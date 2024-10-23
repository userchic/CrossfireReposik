using Microsoft.EntityFrameworkCore;
using WebApplication1.DataBase;
using WebApplication1.Models;

namespace WebApplication1.Abstractions
{
    public interface IUserGameStatusRepository
    {
        public UserGameStatus GetStatus(int id);
    }
}
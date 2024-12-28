using WebApplication1.DataBase;
using WebApplication1.Models;

namespace WebApplication1.Abstractions
{
    public interface IParticipationsRepository
    {
        public UserParticipation GetParticipation(int gameId, string login);
        UserParticipation GetCurrentParticipation(string login);
        public void CreateParticipation(UserParticipation participation);
        public void UpdateParticipation(UserParticipation participation);
        public void Save();
    }
}
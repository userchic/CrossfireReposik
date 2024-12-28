using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.Abstractions;
namespace GameHubSpace
{
    public interface IGameClient
    {
        Task SolvedTaskMessage(int taskId,bool Correctness,bool IsShotSuccessful);
        Task EndGameMessage(int gameID);
    }
    [Authorize (Roles="Игрок")]
    public class GameHub:Hub<IGameClient>
    {
        IGameRepository gameRep;
        IParticipationsRepository participationsRep;
        public GameHub(IGameRepository GameRep,IParticipationsRepository ParticipationsRep)
        {
            gameRep = GameRep;
            participationsRep = ParticipationsRep;
        }
        public void ConnectToTheGame(int gameId)
        {
            Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        }
        public void EndGame(int gameId)
        {
            Clients.Group(gameId.ToString()).EndGameMessage(gameId);
        }
        public void AnswerMessage(int gameId,int taskId,bool isSuccessful,bool IsShotSuccessful)
        {
            Clients.Group(gameId.ToString()).SolvedTaskMessage(taskId, isSuccessful, IsShotSuccessful);
        }
    }
}
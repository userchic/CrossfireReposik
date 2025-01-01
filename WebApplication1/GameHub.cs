using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using WebApplication1.Abstractions;
namespace GameHubSpace
{
    public interface IGameClient
    {
        Task SolvedTaskMessage(int taskId,bool Correctness,bool IsShotSuccessful);
        Task EndGameMessage();
    }
    [Authorize (Roles="Игрок")]
    public class GameHub:Hub<IGameClient>
    {
        public async Task ConnectToTheGame(int gameId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, gameId.ToString());
        }


    }
}
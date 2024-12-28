using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Abstractions;
using WebApplication1.Models;
using WebApplication1.Services;

namespace WebApplication1.Models
{
    public class GameProcess
    {
        public int ID { get; set; }
        DateTime StartData { get; set; }
        public int Lenga { get; set; }
        
        public IGameManager manager { get; set; }

        //public GameState State { get; set; }

        public List<InGameTask> Tasks;
        public List<Teams> Teams;

        private Timer timerToStart;
        public Timer timerToEnd;

        public GameProcess(List<InGameTask> tasks,List<Teams> teams,DateTime startData,int lenga, int gameId)
        {
            ID = gameId;
            Tasks = tasks;
            Teams = teams;
            SetStartTimer(startData,lenga);
        }
        public void StartGame(object? gameId)
        {
            TimeSpan dueTime = StartData.AddMinutes(Lenga) - DateTime.Now;
            TimerCallback callback = manager.RemoveGame; callback += EndGame;

            timerToEnd = new Timer(callback, ID, TimeSpan.Zero, dueTime);
            timerToStart.Dispose();

        }
        public void EndGame(object? gameId)
        {
            timerToEnd.Dispose();
        }

        public void SetStartTimer(DateTime startData, int lenga)
        {
            StartData = startData;
            Lenga = lenga;
            TimeSpan dueTime = startData - DateTime.Now;
            timerToStart = new Timer(StartGame, null, TimeSpan.Zero, dueTime);
        }
    }
    //public enum GameState
    //{
    //    NotStarted,
    //    Started,
    //    Ended
    //}
}
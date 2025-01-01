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
        DateTime startTime;
        DateTime StartData 
        { 
            get 
            {
                return startTime.ToLocalTime();
            }
            set 
            { 
                startTime = value;
            } 
        }
        public int Lenga { get; set; }
        
        public IGameManager manager { get; set; }

        //public GameState State { get; set; }

        public List<InGameTask> Tasks;
        public List<Sent_Answers> answers = new List<Sent_Answers>();
        public List<Teams> Teams;

        public int EndGameAmountOfAnswers;

        private Timer timerToStart;
        public Timer timerToEnd;

        public GameProcess(List<InGameTask> tasks,List<Teams> teams,DateTime startData,int lenga, int gameId)
        {
            ID = gameId;
            Tasks = tasks;
            Teams = teams;
            EndGameAmountOfAnswers = Teams.Count * Tasks.Count;
            SetStartTimer(startData,lenga);
        }
        public void AddAnswer(Sent_Answers answer)
        {
            answers.Add(answer);
            if (answers.Count == EndGameAmountOfAnswers)
                manager.RemoveEndedGame(ID);
        }
        public void StartGame(object? gameId)
        {
            TimeSpan dueTime = StartData.AddMinutes(Lenga) - DateTime.Now;
            TimerCallback callback = manager.RemoveEndedGame; callback += EndGame;

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
            TimeSpan dueTime = StartData - DateTime.Now;
            timerToStart = new Timer(StartGame, null, TimeSpan.Zero, dueTime);
        }

        public void StopTimers()
        {
            timerToEnd.Dispose();
            timerToStart.Dispose();
        }
    }
    //public enum GameState
    //{
    //    NotStarted,
    //    Started,
    //    Ended
    //}
}
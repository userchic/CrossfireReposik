namespace WebApplication1.Models
{
    public class Shots
    {
        public int ID { get; set; }
        public int AnswerID { get; set; }
        public int TargetTeamID { get; set; }
        public bool isSusseccful { get; set; }
        public Sent_Answers Sent_answer { get; set; }
        public Teams TargetTeam { get; set; }
    }
}
using Microsoft.EntityFrameworkCore;
using WebApplication1.DataBase;
using WebApplication1.Models;

namespace WebApplication1.Abstractions
{
    public interface ISent_AnswersRepository
    {
        public Sent_Answers GetAnswer(int id);
        public Tasks GetTask(Sent_Answers answer);
    }
}
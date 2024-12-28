using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;
using WebApplication1.Abstractions;
using WebApplication1.DataBase;
using WebApplication1.Models;

namespace WebApplication1.Reposiotories
{
    public class Sent_AnswersRepository :ISent_AnswersRepository
    {
        GameContext _context;
        public Sent_AnswersRepository(GameContext context) 
        {
            _context = context;
        }
        public Sent_Answers GetAnswer(int id)
        {
            return _context.UsersAnswers.FirstOrDefault(x => x.ID == id);
        }
        public Tasks GetTask(Sent_Answers answer)
        {
            return _context.Tasks.FirstOrDefault(x => x.ID == answer.TaskID);
        }
    }
}

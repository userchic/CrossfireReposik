﻿using Microsoft.EntityFrameworkCore;
using WebApplication1.Abstractions;
using WebApplication1.DataBase;
using WebApplication1.Models;

namespace WebApplication1.Reposiotories
{
    public class ParticipationsRepository : IParticipationsRepository
    {
        public GameContext _context;
        public ParticipationsRepository(GameContext context)
        {
            _context = context;
        }
        public UserParticipation GetParticipation(int gameId,string login)
        {
            return _context.Participations.FirstOrDefault(x => x.GameID == gameId&& x.User.Login==login);
        }
        public void CreateParticipation(UserParticipation participation)
        {
            _context.Participations.Add(participation);
        }
        public void UpdateParticipation(UserParticipation participation)
        {
            _context.Participations.Update(participation);
        }
        public void Save()
        {
            _context.SaveChanges();
        }
    }
}
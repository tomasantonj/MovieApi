using Movie.Contracts;
using Movie.Core.Models;
using Movie.Core.DTOs;
using Movie.Data;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Movie.Services
{
    public class ActorService : IActorService
    {
        private readonly Movie.Data.MovieApiContext _context;
        public ActorService(Movie.Data.MovieApiContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Actor>> GetActorsAsync()
        {
            return await _context.Actors.ToListAsync();
        }

        public async Task<Actor?> GetActorByIdAsync(int id)
        {
            return await _context.Actors.FindAsync(id);
        }

        public async Task<Actor> CreateActorAsync(ActorCreateDto dto)
        {
            var actor = new Actor { Name = dto.Name };
            _context.Actors.Add(actor);
            await _context.SaveChangesAsync();
            return actor;
        }

        public async Task<bool> UpdateActorAsync(int id, Actor actor)
        {
            if (id != actor.Id)
                return false;
            _context.Entry(actor).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _context.Actors.AnyAsync(e => e.Id == id))
                    return false;
                throw;
            }
        }

        public async Task<bool> DeleteActorAsync(int id)
        {
            var actor = await _context.Actors.FindAsync(id);
            if (actor == null)
                return false;
            _context.Actors.Remove(actor);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}

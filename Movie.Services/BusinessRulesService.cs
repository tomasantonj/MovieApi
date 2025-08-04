using Movie.Core.Models;
using Movie.Core.DTOs;
using Movie.Data;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Movie.Services
{
    public class BusinessRulesService : IBusinessRulesService
    {
        private readonly MovieApiContext _context;
        public BusinessRulesService(MovieApiContext context)
        {
            _context = context;
        }

        // 1. Movies cannot have more than 50 reviews
        public async Task<bool> CanAddReviewToMovie(int movieId)
        {
            int reviewCount = await _context.MovieReviews.CountAsync(r => r.VideoMovieId == movieId);
            return reviewCount < 50;
        }

        // 2. Actors cannot be assigned to the same movie twice
        public async Task<bool> CanAssignActorToMovie(int movieId, int actorId)
        {
            bool alreadyAssigned = await _context.MovieActors.AnyAsync(ma => ma.VideoMovieId == movieId && ma.ActorId == actorId);
            return !alreadyAssigned;
        }

        // 3. Budget cannot be negative
        public bool IsBudgetValid(decimal budget)
        {
            return budget >= 0;
        }

        // 4. Documentaries cannot have more than 10 actors
        public async Task<bool> CanAddActorToDocumentary(int movieId)
        {
            var movie = await _context.VideoMovies.Include(m => m.Genre).Include(m => m.MovieActors).FirstOrDefaultAsync(m => m.Id == movieId);
            if (movie?.Genre?.Name == "Documentary")
            {
                int actorCount = movie.MovieActors.Count;
                return actorCount < 10;
            }
            return true;
        }

        // 5. Documentaries cannot have budgets above 10 million
        public async Task<bool> IsDocumentaryBudgetValid(int movieId, decimal budget)
        {
            var movie = await _context.VideoMovies.Include(m => m.Genre).FirstOrDefaultAsync(m => m.Id == movieId);
            if (movie?.Genre?.Name == "Documentary")
            {
                return budget <= 10000000;
            }
            return true;
        }
    }
}

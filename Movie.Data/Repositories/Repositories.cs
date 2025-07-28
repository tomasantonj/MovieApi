using MovieApi.Movie.Core.DomainContracts;
using Movie.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Movie.Data;

namespace MovieApi.Movie.Data.Repositories
{
    public class VideoMovieRepository : IVideoMovieRepository
    {
        private readonly MovieApiContext _context;
        public VideoMovieRepository(MovieApiContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<VideoMovie>> GetAllAsync() => await _context.VideoMovies.ToListAsync();
        public async Task<VideoMovie> GetAsync(int id) => await _context.VideoMovies.FindAsync(id);
        public async Task<bool> AnyAsync(int id) => await _context.VideoMovies.AnyAsync(m => m.Id == id);
        public void Add(VideoMovie movie) => _context.VideoMovies.Add(movie);
        public void Update(VideoMovie movie) => _context.VideoMovies.Update(movie);
        public void Remove(VideoMovie movie) => _context.VideoMovies.Remove(movie);
    }

    public class ReviewRepository : IReviewRepository
    {
        private readonly MovieApiContext _context;
        public ReviewRepository(MovieApiContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<MovieReview>> GetAllAsync() => await _context.MovieReviews.ToListAsync();
        public async Task<MovieReview> GetAsync(int id) => await _context.MovieReviews.FindAsync(id);
        public async Task<bool> AnyAsync(int id) => await _context.MovieReviews.AnyAsync(r => r.Id == id);
        public void Add(MovieReview review) => _context.MovieReviews.Add(review);
        public void Update(MovieReview review) => _context.MovieReviews.Update(review);
        public void Remove(MovieReview review) => _context.MovieReviews.Remove(review);
    }

    public class ActorRepository : IActorRepository
    {
        private readonly MovieApiContext _context;
        public ActorRepository(MovieApiContext context)
        {
            _context = context;
        }
        public async Task<IEnumerable<Actor>> GetAllAsync() => await _context.Actors.ToListAsync();
        public async Task<Actor> GetAsync(int id) => await _context.Actors.FindAsync(id);
        public async Task<bool> AnyAsync(int id) => await _context.Actors.AnyAsync(a => a.Id == id);
        public void Add(Actor actor) => _context.Actors.Add(actor);
        public void Update(Actor actor) => _context.Actors.Update(actor);
        public void Remove(Actor actor) => _context.Actors.Remove(actor);
    }
}
